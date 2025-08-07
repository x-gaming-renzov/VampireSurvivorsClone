# NovaExperience Assets

NovaExperience assets group related NovaContext components into logical experiences. This guide covers creating, configuring, and managing NovaExperience assets for organizing your game's configurable content.

## 🎯 What is NovaExperience?

A **NovaExperience** asset represents a logical grouping of NovaContext components that work together to define a specific user experience or feature set. Think of it as a "configuration package" that can be managed and experimented on as a unit.

### Key Concepts

- **Experience Name**: Unique identifier for the experience
- **Description**: Documentation of what the experience represents
- **GameObjects**: List of GameObjects that have NovaContext components
- **Schema Management**: Experiences are pushed to Nova backend as part of the schema

## 🏗️ Creating NovaExperience Assets

### 1. Basic Creation

1. **Create the Asset**
   ```
   Right-click in Project window → Create → Nova → Experience
   ```

2. **Configure Basic Settings**
   - Set **Experience Name** (e.g., "MainGameExperience")
   - Add **Description** explaining the experience's purpose

### 2. Adding GameObjects

1. **Add GameObjects with NovaContext**
   - Click the **"+"** button in the GameObjects list
   - Drag GameObjects from the hierarchy that have NovaContext components
   - Or use the object picker to select GameObjects

2. **Verify Contexts**
   - Ensure all added GameObjects have NovaContext components
   - Check that NovaContext components have valid ObjectNames

### 3. Example Configuration

```csharp
// Example NovaExperience configuration
Experience Name: "MainGameExperience"
Description: "Core gameplay configuration including player settings, UI, and game balance"

GameObjects:
  - PlayerConfig (GameObject with NovaContext)
    - ObjectName: "player_config"
    - Properties: player_speed, show_tutorial, starting_coins
    
  - UISettings (GameObject with NovaContext)
    - ObjectName: "ui_settings"
    - Properties: ui_scale, show_ads, theme_color
    
  - GameBalance (GameObject with NovaContext)
    - ObjectName: "game_balance"
    - Properties: enemy_health, coin_rewards, difficulty_multiplier
```

## 🎮 Experience Organization Strategies

### 1. Feature-Based Organization

Group contexts by game features:

```csharp
// Combat Experience
Experience Name: "CombatExperience"
Description: "Combat system configuration"
GameObjects:
  - CombatConfig (player_damage, weapon_damage, armor_rating)
  - EnemyConfig (enemy_health, enemy_damage, spawn_rate)
  - PowerUpConfig (powerup_duration, powerup_effectiveness)

// Shop Experience
Experience Name: "ShopExperience"
Description: "In-game shop configuration"
GameObjects:
  - ShopConfig (item_prices, discount_percentage, featured_items)
  - CurrencyConfig (coin_value, gem_value, exchange_rate)
```

### 2. User Journey Organization

Group contexts by user progression:

```csharp
// Tutorial Experience
Experience Name: "TutorialExperience"
Description: "New player tutorial configuration"
GameObjects:
  - TutorialConfig (tutorial_steps, skip_enabled, hand_pointer)
  - OnboardingConfig (welcome_message, starting_items)

// Endgame Experience
Experience Name: "EndgameExperience"
Description: "Advanced player content configuration"
GameObjects:
  - EndgameConfig (challenge_modes, leaderboards, special_rewards)
  - CompetitiveConfig (ranking_system, matchmaking, tournaments)
```

### 3. Platform-Specific Organization

Group contexts by platform or device:

```csharp
// Mobile Experience
Experience Name: "MobileExperience"
Description: "Mobile-specific configuration"
GameObjects:
  - MobileUI (touch_controls, screen_adaptation, battery_optimization)
  - MobilePerformance (graphics_quality, frame_rate, memory_usage)

// PC Experience
Experience Name: "PCExperience"
Description: "PC-specific configuration"
GameObjects:
  - PCUI (keyboard_controls, mouse_sensitivity, resolution_options)
  - PCPerformance (graphics_settings, audio_quality, network_settings)
```

## 🔄 Schema Management

### 1. Pushing Schema

Push your experiences to the Nova backend:

```csharp
// Push schema from Unity menu
Nova → Push Schema to Backend

// This will:
// 1. Build schema from all NovaExperience assets
// 2. Include all NovaContext components from GameObjects
// 3. Send to Nova backend for configuration
```

### 2. Schema Validation

The SDK validates experiences before pushing:

```csharp
// Check if experience is valid
var experience = Resources.Load<NovaExperience>("MainGameExperience");

if (experience.IsValid())
{
    Debug.Log("Experience is properly configured");
    // Experience can be pushed to backend
}
else
{
    Debug.LogError("Experience has validation issues");
    // Fix issues before pushing
}
```

### 3. Schema Structure

The pushed schema includes:

```json
{
  "organisation_id": "your-org",
  "app_id": "your-app",
  "objects": {
    "player_config": {
      "type": "Param",
      "keys": {
        "player_speed": { "type": "number", "default": 5.0 },
        "show_tutorial": { "type": "boolean", "default": true }
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

## 🚀 Runtime Usage

### 1. Loading Experiences

Load experiences at runtime:

```csharp
public class ExperienceManager : MonoBehaviour
{
    void Start()
    {
        LoadExperiences();
    }
    
    async void LoadExperiences()
    {
        // Load experience assets
        var mainExperience = Resources.Load<NovaExperience>("MainGameExperience");
        var shopExperience = Resources.Load<NovaExperience>("ShopExperience");
        
        // Fetch from backend
        await NovaSDK.Instance.FetchExperience(mainExperience);
        await NovaSDK.Instance.FetchExperience(shopExperience);
        
        Debug.Log("Experiences loaded successfully");
    }
}
```

### 2. Accessing Experience Data

Get values from contexts in experiences:

```csharp
// Get values from contexts in the experience
float playerSpeed = NovaSDK.Instance.GetValue<float>("player_config", "player_speed");
bool showTutorial = NovaSDK.Instance.GetValue<bool>("player_config", "show_tutorial");
float uiScale = NovaSDK.Instance.GetValue<float>("ui_settings", "ui_scale");
```

### 3. Experience Validation

Validate experiences at runtime:

```csharp
public class ExperienceValidator : MonoBehaviour
{
    public void ValidateExperience(NovaExperience experience)
    {
        if (!experience.IsValid())
        {
            Debug.LogError($"Experience '{experience.name}' is not valid");
            return;
        }
        
        var validContexts = experience.GetValidContexts();
        Debug.Log($"Experience '{experience.name}' has {validContexts.Count} valid contexts");
        
        foreach (var context in validContexts)
        {
            Debug.Log($"  - {context.ObjectName} on {context.gameObject.name}");
        }
    }
}
```

## 📊 Experience Analytics

### 1. Experience Usage Tracking

Track which experiences are being used:

```csharp
public class ExperienceTracker : MonoBehaviour
{
    public void TrackExperienceLoaded(NovaExperience experience)
    {
        NovaSDK.Instance.TrackEvent("experience_loaded", new Dictionary<string, object>
        {
            ["experience_name"] = experience.ExperienceName,
            ["context_count"] = experience.GetValidContexts().Count,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
    
    public void TrackExperienceError(NovaExperience experience, string error)
    {
        NovaSDK.Instance.TrackEvent("experience_error", new Dictionary<string, object>
        {
            ["experience_name"] = experience.ExperienceName,
            ["error"] = error,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
```

### 2. Context Usage Analytics

Track which contexts are being accessed:

```csharp
public class ContextTracker : MonoBehaviour
{
    public void TrackContextAccess(string objectName, string propertyName)
    {
        NovaSDK.Instance.TrackEvent("context_accessed", new Dictionary<string, object>
        {
            ["object_name"] = objectName,
            ["property_name"] = propertyName,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
```

## 🔧 Advanced Features

### 1. Dynamic Experience Loading

Load experiences based on user profile:

```csharp
public class DynamicExperienceLoader : MonoBehaviour
{
    public async Task LoadExperiencesForUser(Dictionary<string, object> userProfile)
    {
        // Determine which experiences to load based on user profile
        var experiencesToLoad = GetExperiencesForUser(userProfile);
        
        foreach (var experienceName in experiencesToLoad)
        {
            var experience = Resources.Load<NovaExperience>(experienceName);
            if (experience != null)
            {
                await NovaSDK.Instance.FetchExperience(experience);
                Debug.Log($"Loaded experience: {experienceName}");
            }
        }
    }
    
    private List<string> GetExperiencesForUser(Dictionary<string, object> profile)
    {
        var experiences = new List<string> { "MainGameExperience" };
        
        // Add experiences based on user level
        if (profile.ContainsKey("level"))
        {
            int level = Convert.ToInt32(profile["level"]);
            if (level < 10)
            {
                experiences.Add("TutorialExperience");
            }
            else if (level > 50)
            {
                experiences.Add("EndgameExperience");
            }
        }
        
        // Add experiences based on platform
        if (Application.isMobilePlatform)
        {
            experiences.Add("MobileExperience");
        }
        else
        {
            experiences.Add("PCExperience");
        }
        
        return experiences;
    }
}
```

### 2. Experience Dependencies

Handle experience dependencies:

```csharp
public class ExperienceDependencyManager : MonoBehaviour
{
    private Dictionary<string, List<string>> experienceDependencies = new Dictionary<string, List<string>>
    {
        ["ShopExperience"] = new List<string> { "MainGameExperience" },
        ["EndgameExperience"] = new List<string> { "MainGameExperience", "CombatExperience" }
    };
    
    public async Task LoadExperienceWithDependencies(string experienceName)
    {
        // Load dependencies first
        if (experienceDependencies.ContainsKey(experienceName))
        {
            foreach (var dependency in experienceDependencies[experienceName])
            {
                await LoadExperience(dependency);
            }
        }
        
        // Load the target experience
        await LoadExperience(experienceName);
    }
    
    private async Task LoadExperience(string experienceName)
    {
        var experience = Resources.Load<NovaExperience>(experienceName);
        if (experience != null)
        {
            await NovaSDK.Instance.FetchExperience(experience);
        }
    }
}
```

## 🎯 Best Practices

### 1. Experience Design

**Logical Grouping**:
- Group related contexts together
- Keep experiences focused on specific features or user journeys
- Avoid mixing unrelated contexts

**Size Management**:
- Keep experiences manageable (5-20 contexts per experience)
- Split large experiences into smaller, focused ones
- Consider performance implications

### 2. Naming Conventions

**Experience Names**:
- Use descriptive, PascalCase names
- Include "Experience" suffix for clarity
- Examples: `MainGameExperience`, `TutorialExperience`, `ShopExperience`

**Descriptions**:
- Clearly explain the experience's purpose
- Mention key features or contexts included
- Include usage guidelines

### 3. Organization

**File Structure**:
```
Assets/
├── Nova/
│   ├── Experiences/
│   │   ├── MainGameExperience.asset
│   │   ├── TutorialExperience.asset
│   │   └── ShopExperience.asset
│   └── Contexts/
│       ├── PlayerConfig.prefab
│       ├── UISettings.prefab
│       └── GameBalance.prefab
```

### 4. Validation

**Pre-Push Validation**:
- Always validate experiences before pushing
- Check that all contexts have valid ObjectNames
- Ensure descriptions are clear and helpful

**Runtime Validation**:
- Validate experiences when loading
- Handle missing or invalid experiences gracefully
- Log validation issues for debugging

## 🔗 Related Documentation

- **[NovaContext Components](./novacontext.md)** - Creating configurable objects
- **[Schema Management](./schema-management.md)** - Pushing configurations to backend
- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[Examples](./examples.md)** - Experience management examples
- **[Troubleshooting](./troubleshooting.md)** - Common experience issues 