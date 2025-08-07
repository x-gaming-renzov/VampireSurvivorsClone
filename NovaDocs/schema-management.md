# Schema Management

Schema management is the process of defining, validating, and pushing your game's configuration structure to the Nova backend. This guide covers the complete schema management workflow.

## 🎯 What is a Schema?

A **schema** defines the structure of your game's configurable content:

- **Objects**: Your NovaContext components with their properties
- **Experiences**: How objects are grouped into logical experiences
- **Types**: Data types for each configurable property
- **Defaults**: Fallback values when remote configuration is unavailable

## 🏗️ Schema Structure

### Schema Components

```json
{
  "organisation_id": "your-organization",
  "app_id": "your-app-id",
  "objects": {
    "player_config": {
      "type": "Param",
      "keys": {
        "player_speed": {
          "type": "number",
          "description": "Player movement speed",
          "default": 5.0
        },
        "show_tutorial": {
          "type": "boolean",
          "description": "Show tutorial for new players",
          "default": true
        }
      }
    }
  },
  "experiences": {
    "MainGameExperience": {
      "description": "Core gameplay configuration",
      "objects": {
        "player_config": true,
        "ui_settings": true
      }
    }
  }
}
```

### Schema Elements

| Element | Description | Example |
|---------|-------------|---------|
| `organisation_id` | Your Nova organization identifier | `"my-company"` |
| `app_id` | Your application identifier | `"my-game-v1"` |
| `objects` | Dictionary of configurable objects | `{"player_config": {...}}` |
| `experiences` | Dictionary of experience configurations | `{"MainGameExperience": {...}}` |

## 🔄 Schema Workflow

### 1. Design Phase

1. **Identify Configurable Elements**
   - Game parameters that need remote control
   - UI elements that should be configurable
   - Feature flags and toggles

2. **Create NovaContext Components**
   - Define ObjectNames for each context
   - Add properties with appropriate types
   - Set sensible default values

3. **Organize into Experiences**
   - Group related contexts together
   - Create NovaExperience assets
   - Add GameObjects with NovaContext components

### 2. Validation Phase

1. **Validate NovaContext Components**
   ```csharp
   // Check if context is valid
   var context = GetComponent<NovaContext>();
   if (context.IsValid())
   {
       Debug.Log("Context is properly configured");
   }
   ```

2. **Validate NovaExperience Assets**
   ```csharp
   // Check if experience is valid
   var experience = Resources.Load<NovaExperience>("MainGameExperience");
   if (experience.IsValid())
   {
       Debug.Log("Experience is properly configured");
   }
   ```

3. **Validate NovaSettings**
   ```csharp
   // Check if settings are valid
   var settings = NovaSDK.Instance.Settings;
   if (settings.IsValid())
   {
       Debug.Log("Settings are properly configured");
   }
   ```

### 3. Push Phase

1. **Build Schema**
   - Unity automatically builds schema from your assets
   - Includes all NovaContext components from NovaExperience GameObjects
   - Validates schema structure and data types

2. **Push to Backend**
   ```
   Nova → Push Schema to Backend
   ```

3. **Verify Success**
   - Check console for success messages
   - Verify in Nova dashboard that objects appear
   - Test configuration in dashboard

## 🚀 Pushing Schema

### Manual Push

Push schema from Unity menu:

```
Nova → Push Schema to Backend
```

### Programmatic Push

Push schema from code:

```csharp
using Nova.SDK.Editor.Internal;

public class SchemaPusher : MonoBehaviour
{
    public async void PushSchema()
    {
        try
        {
            bool success = await SchemaBuilder.BuildAndPushSchema();
            
            if (success)
            {
                Debug.Log("Schema pushed successfully!");
            }
            else
            {
                Debug.LogError("Failed to push schema");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Schema push error: {ex.Message}");
        }
    }
}
```

### Automated Push

Set up automated schema pushing:

```csharp
[UnityEditor.CustomEditor(typeof(NovaExperience))]
public class NovaExperienceEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Push Schema"))
        {
            PushSchemaToBackend();
        }
    }
    
    private async void PushSchemaToBackend()
    {
        bool success = await SchemaBuilder.BuildAndPushSchema();
        
        if (success)
        {
            Debug.Log("Schema updated successfully!");
        }
        else
        {
            Debug.LogError("Schema update failed");
        }
    }
}
```

## 📊 Schema Validation

### Pre-Push Validation

The SDK validates your schema before pushing:

```csharp
// Validation checks performed:
// 1. NovaSettings are properly configured
// 2. All NovaContext components have valid ObjectNames
// 3. All properties have valid types and defaults
// 4. All NovaExperience assets reference valid GameObjects
// 5. No duplicate ObjectNames across contexts
```

### Validation Errors

Common validation errors and solutions:

| Error | Cause | Solution |
|-------|-------|----------|
| `Missing organisation_id` | NovaSettings not configured | Set Organization ID in NovaSettings |
| `Missing app_id` | NovaSettings not configured | Set App ID in NovaSettings |
| `Invalid ObjectName` | Empty or invalid ObjectName | Set valid ObjectName in NovaContext |
| `Duplicate ObjectName` | Multiple contexts with same name | Use unique ObjectNames |
| `Invalid property type` | Unsupported property type | Use supported types (number, boolean, text, JSON) |
| `Missing default value` | Property has no default | Set default value for property |

### Custom Validation

Add custom validation logic:

```csharp
public class SchemaValidator : MonoBehaviour
{
    public bool ValidateSchema()
    {
        bool isValid = true;
        
        // Validate all NovaContext components
        var contexts = FindObjectsOfType<NovaContext>();
        foreach (var context in contexts)
        {
            if (!ValidateContext(context))
            {
                isValid = false;
            }
        }
        
        // Validate all NovaExperience assets
        var experiences = Resources.LoadAll<NovaExperience>("");
        foreach (var experience in experiences)
        {
            if (!ValidateExperience(experience))
            {
                isValid = false;
            }
        }
        
        return isValid;
    }
    
    private bool ValidateContext(NovaContext context)
    {
        if (string.IsNullOrEmpty(context.ObjectName))
        {
            Debug.LogError($"Context on {context.gameObject.name} has no ObjectName");
            return false;
        }
        
        if (context.Properties == null || context.Properties.Count == 0)
        {
            Debug.LogWarning($"Context {context.ObjectName} has no properties");
        }
        
        return true;
    }
    
    private bool ValidateExperience(NovaExperience experience)
    {
        if (string.IsNullOrEmpty(experience.ExperienceName))
        {
            Debug.LogError($"Experience {experience.name} has no ExperienceName");
            return false;
        }
        
        if (experience.GameObjects == null || experience.GameObjects.Count == 0)
        {
            Debug.LogWarning($"Experience {experience.ExperienceName} has no GameObjects");
        }
        
        return true;
    }
}
```

## 🔄 Schema Versioning

### Version Control

Nova automatically tracks schema versions:

- **Automatic versioning**: Each push creates a new version
- **Backward compatibility**: Old versions remain accessible
- **Rollback capability**: Revert to previous versions if needed
- **Change tracking**: See what changed between versions

### Schema Evolution

Best practices for schema evolution:

```csharp
// 1. Add new properties (safe)
// Add new properties to existing contexts
context.Properties.Add(new PropertyDefinition
{
    PropertyName = "new_feature_enabled",
    Type = PropertyType.Boolean,
    DefaultValue = "false",
    Description = "Enable new feature"
});

// 2. Modify existing properties (careful)
// Only change default values, not types or names
// context.Properties[0].DefaultValue = "new_default";

// 3. Remove properties (dangerous)
// Only remove properties that are no longer used
// context.Properties.RemoveAt(propertyIndex);

// 4. Add new contexts (safe)
// Create new NovaContext components as needed

// 5. Remove contexts (dangerous)
// Only remove contexts that are no longer referenced
```

## 📈 Schema Analytics

### Push Tracking

Track schema push events:

```csharp
public class SchemaAnalytics : MonoBehaviour
{
    public void TrackSchemaPush(bool success, string error = null)
    {
        var eventData = new Dictionary<string, object>
        {
            ["success"] = success,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["schema_version"] = GetSchemaVersion()
        };
        
        if (!success && !string.IsNullOrEmpty(error))
        {
            eventData["error"] = error;
        }
        
        NovaSDK.Instance.TrackEvent("schema_pushed", eventData);
    }
    
    private string GetSchemaVersion()
    {
        // Get current schema version
        // This would be implemented based on your versioning strategy
        return "1.0.0";
    }
}
```

### Usage Analytics

Track schema usage:

```csharp
public class SchemaUsageTracker : MonoBehaviour
{
    public void TrackObjectUsage(string objectName, string propertyName)
    {
        NovaSDK.Instance.TrackEvent("schema_object_used", new Dictionary<string, object>
        {
            ["object_name"] = objectName,
            ["property_name"] = propertyName,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
    
    public void TrackExperienceUsage(string experienceName)
    {
        NovaSDK.Instance.TrackEvent("schema_experience_used", new Dictionary<string, object>
        {
            ["experience_name"] = experienceName,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
```

## 🔧 Advanced Features

### Schema Templates

Create reusable schema templates:

```csharp
[CreateAssetMenu(fileName = "SchemaTemplate", menuName = "Nova/Schema Template")]
public class SchemaTemplate : ScriptableObject
{
    public List<NovaContextTemplate> contextTemplates;
    public List<NovaExperienceTemplate> experienceTemplates;
    
    [System.Serializable]
    public class NovaContextTemplate
    {
        public string objectName;
        public List<PropertyTemplate> properties;
    }
    
    [System.Serializable]
    public class PropertyTemplate
    {
        public string propertyName;
        public PropertyType type;
        public string defaultValue;
        public string description;
    }
    
    [System.Serializable]
    public class NovaExperienceTemplate
    {
        public string experienceName;
        public string description;
        public List<string> objectNames;
    }
}
```

### Schema Migration

Handle schema migrations:

```csharp
public class SchemaMigration : MonoBehaviour
{
    public async Task MigrateSchema(string fromVersion, string toVersion)
    {
        // Load old schema
        var oldSchema = await LoadSchema(fromVersion);
        
        // Apply migration rules
        var newSchema = ApplyMigrationRules(oldSchema, fromVersion, toVersion);
        
        // Push new schema
        await PushSchema(newSchema);
    }
    
    private Dictionary<string, object> ApplyMigrationRules(
        Dictionary<string, object> oldSchema, 
        string fromVersion, 
        string toVersion)
    {
        var newSchema = new Dictionary<string, object>(oldSchema);
        
        // Apply version-specific migration rules
        switch (fromVersion)
        {
            case "1.0.0" when toVersion == "1.1.0":
                // Add new properties
                AddNewProperties(newSchema);
                break;
                
            case "1.1.0" when toVersion == "2.0.0":
                // Major version changes
                ApplyMajorVersionChanges(newSchema);
                break;
        }
        
        return newSchema;
    }
}
```

## 🎯 Best Practices

### 1. Schema Design

**Naming Conventions**:
- Use consistent, descriptive names
- Follow snake_case for ObjectNames and property names
- Use PascalCase for ExperienceNames

**Organization**:
- Group related properties in contexts
- Keep contexts focused and manageable
- Use experiences to organize contexts logically

### 2. Validation

**Pre-Push Validation**:
- Always validate before pushing
- Check for common errors
- Test with sample data

**Runtime Validation**:
- Validate schema when loading
- Handle missing or invalid data gracefully
- Log validation issues for debugging

### 3. Versioning

**Schema Evolution**:
- Plan schema changes carefully
- Maintain backward compatibility
- Document breaking changes
- Test migrations thoroughly

### 4. Performance

**Optimization**:
- Keep schemas lean and focused
- Avoid unnecessary properties
- Use appropriate data types
- Cache frequently accessed values

## 🔗 Related Documentation

- **[NovaContext Components](./novacontext.md)** - Creating configurable objects
- **[NovaExperience Assets](./novaexperience.md)** - Grouping contexts into experiences
- **[Configuration](./configuration.md)** - Setting up NovaSettings
- **[Examples](./examples.md)** - Schema management examples
- **[Troubleshooting](./troubleshooting.md)** - Common schema issues 