# NovaContext Components

NovaContext components are the building blocks of Nova's configuration system. They define configurable objects in your game with properties that can be controlled remotely.

## 🎯 What is NovaContext?

A **NovaContext** component represents a logical group of related configurable properties in your game. Think of it as a "configuration object" that can be updated remotely without app updates.

### Key Concepts

- **Object Name**: Unique identifier for the context
- **Properties**: Configurable parameters with types and default values
- **Runtime Access**: Values can be retrieved during gameplay
- **Real-time Updates**: Changes sync from backend automatically

## 🏗️ Creating NovaContext Components

### 1. Basic Setup

1. **Create a GameObject**
   ```
   Create Empty GameObject → Name it (e.g., "PlayerConfig")
   ```

2. **Add NovaContext Component**
   ```
   Add Component → Search "Nova Context" → Add
   ```

3. **Configure Object Name**
   ```
   Set Object Name to a unique identifier (e.g., "player_config")
   ```

### 2. Adding Properties

Properties define what can be configured in your context:

1. **Click the "+" button** in the Properties list
2. **Fill in property details**:
   - **Property Name**: Unique identifier (e.g., "player_speed")
   - **Type**: Data type (Number, Boolean, Text, JSON)
   - **Default Value**: Fallback value when remote value is unavailable
   - **Description**: Documentation for the property

### 3. Example Configuration

```csharp
// Example NovaContext configuration
Object Name: "player_config"
Properties:
  - Property Name: "player_speed"
    Type: Number
    Default Value: "5.0"
    Description: "Player movement speed in units per second"
    
  - Property Name: "show_tutorial"
    Type: Boolean
    Default Value: "true"
    Description: "Whether to show the tutorial for new players"
    
  - Property Name: "starting_coins"
    Type: Number
    Default Value: "100"
    Description: "Number of coins given to new players"
```

## 📋 Property Types

Nova supports several property types for different use cases:

### Number Type

Use for numeric values like speeds, prices, timers, etc.

```csharp
// Configuration
Property Name: "player_speed"
Type: Number
Default Value: "5.0"

// Runtime access
float speed = NovaSDK.Instance.GetValue<float>("player_config", "player_speed");
```

**Supported C# Types**: `float`, `int`, `double`

### Boolean Type

Use for true/false flags like feature toggles, tutorials, etc.

```csharp
// Configuration
Property Name: "show_tutorial"
Type: Boolean
Default Value: "true"

// Runtime access
bool showTutorial = NovaSDK.Instance.GetValue<bool>("player_config", "show_tutorial");
```

**Supported C# Types**: `bool`

### Text Type

Use for string content like messages, names, descriptions, etc.

```csharp
// Configuration
Property Name: "welcome_message"
Type: Text
Default Value: "Welcome to the game!"

// Runtime access
string message = NovaSDK.Instance.GetValue<string>("player_config", "welcome_message");
```

**Supported C# Types**: `string`

### JSON Type

Use for complex data structures like arrays, objects, configurations, etc.

```csharp
// Configuration
Property Name: "level_config"
Type: JSON
Default Value: '{"enemies": 5, "time_limit": 120, "rewards": ["coin", "gem"]}'

// Runtime access
string jsonConfig = NovaSDK.Instance.GetValue<string>("player_config", "level_config");
// Parse JSON as needed
var config = JsonUtility.FromJson<LevelConfig>(jsonConfig);
```

**Supported C# Types**: `string` (JSON string that you parse)

## 🎮 Best Practices

### 1. Naming Conventions

**Object Names**:
- Use lowercase with underscores
- Be descriptive and specific
- Avoid spaces and special characters
- Examples: `player_config`, `ui_settings`, `game_balance`

**Property Names**:
- Use lowercase with underscores
- Be specific about what the property controls
- Use consistent naming patterns
- Examples: `player_speed`, `show_tutorial`, `starting_coins`

### 2. Property Design

**Default Values**:
- Always provide sensible defaults
- Consider offline scenarios
- Test with default values
- Document the expected range/format

**Descriptions**:
- Explain what the property controls
- Include units where applicable
- Mention any dependencies
- Provide usage examples

### 3. Organization

**Logical Grouping**:
- Group related properties together
- Keep contexts focused on specific features
- Avoid mixing unrelated properties
- Use clear, descriptive object names

**Property Count**:
- Keep contexts manageable (5-20 properties)
- Split large contexts into smaller ones
- Consider performance implications
- Balance granularity with usability

## 🔧 Advanced Features

### Property Validation

The SDK validates properties automatically:

```csharp
// Check if a NovaContext is valid
var context = GetComponent<NovaContext>();
if (context.IsValid())
{
    Debug.Log("NovaContext is properly configured");
}
else
{
    Debug.LogWarning("NovaContext has validation issues");
}
```

### Property Names Access

Get all property names from a context:

```csharp
// Get all property names
var propertyNames = context.GetPropertyNames();
foreach (var name in propertyNames)
{
    Debug.Log($"Property: {name}");
}
```

### Local Default Values

Access default values directly from the context:

```csharp
// Get default value for a property
float defaultSpeed = context.GetLocalDefaultValue<float>("player_speed");
bool defaultTutorial = context.GetLocalDefaultValue<bool>("show_tutorial");
```

## 🚀 Runtime Usage

### Getting Values

Retrieve property values at runtime:

```csharp
public class PlayerController : MonoBehaviour
{
    private NovaContext playerConfig;
    
    void Start()
    {
        playerConfig = GetComponent<NovaContext>();
        LoadConfiguration();
    }
    
    void LoadConfiguration()
    {
        // Get values from NovaContext
        float speed = playerConfig.GetValue<float>("player_speed");
        bool showTutorial = playerConfig.GetValue<bool>("show_tutorial");
        int startingCoins = playerConfig.GetValue<int>("starting_coins");
        
        // Apply to game
        playerSpeed = speed;
        if (showTutorial) ShowTutorial();
        playerCoins = startingCoins;
    }
}
```

### Value Caching

Values are cached for performance:

```csharp
// Values are cached after first access
float speed1 = playerConfig.GetValue<float>("player_speed"); // Fetches from backend
float speed2 = playerConfig.GetValue<float>("player_speed"); // Uses cached value
```

### Error Handling

Handle missing or invalid values:

```csharp
try
{
    float speed = playerConfig.GetValue<float>("player_speed");
    // Use speed value
}
catch (Exception ex)
{
    Debug.LogWarning($"Failed to get player_speed: {ex.Message}");
    // Use fallback value
    float fallbackSpeed = 5.0f;
}
```

## 🔄 Integration with NovaExperience

NovaContext components are grouped into NovaExperience assets:

1. **Create NovaExperience asset**
2. **Add NovaContext GameObjects** to the experience
3. **Push schema** to sync with backend
4. **Fetch experience** at runtime to get values

```csharp
// Fetch experience containing your contexts
var experience = Resources.Load<NovaExperience>("MainGameExperience");
await NovaSDK.Instance.FetchExperience(experience);

// Now you can get values from contexts in the experience
float speed = NovaSDK.Instance.GetValue<float>("player_config", "player_speed");
```

## 📊 Monitoring and Debugging

### Validation Status

Check the validation status in the inspector:

- ✅ **Valid**: All properties are properly configured
- ⚠️ **Warning**: Some issues but still functional
- ❌ **Error**: Critical issues that need fixing

### Debug Information

Enable verbose logging to see detailed information:

```csharp
// In NovaSettings, enable Verbose Logging
// This will show property access, caching, and error details
```

### Property Access Logging

The SDK logs property access for debugging:

```
Nova: Getting value 'player_speed' from 'player_config'
Nova: Value not found in cache, using default: 5.0
Nova: Cached value for 'player_speed': 5.0
```

## 🔗 Related Documentation

- **[NovaExperience Assets](./novaexperience.md)** - Grouping contexts into experiences
- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[Examples](./examples.md)** - Practical implementation examples
- **[Troubleshooting](./troubleshooting.md)** - Common issues and solutions 