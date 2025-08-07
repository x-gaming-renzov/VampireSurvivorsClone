# Runtime API

This guide covers the complete runtime API for the Nova Unity SDK, including initialization, user management, experience fetching, value retrieval, and event tracking.

## 🚀 Initialization

### Basic Initialization

```csharp
using Nova.SDK;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Initialize the Nova SDK
        NovaSDK.Instance.Initialize();
    }
}
```

### Initialization Status

```csharp
// Check if SDK is initialized
if (NovaSDK.Instance.IsInitialized)
{
    Debug.Log("Nova SDK is ready to use");
}
else
{
    Debug.LogWarning("Nova SDK is not initialized");
}
```

**Note**: The SDK automatically loads `NovaSettings` from the Resources folder. Make sure you have a `NovaSettings` asset in your `Resources` folder.

## 👥 User Management

### Creating Users

```csharp
// Create a user with basic ID
bool success = await NovaSDK.Instance.CreateUser("player_123");

if (success)
{
    Debug.Log("User created successfully");
}
else
{
    Debug.LogError("Failed to create user");
}
```

### Creating Users with Profile

```csharp
// Create user profile data
var userProfile = new Dictionary<string, object>
{
    ["level"] = 5,
    ["country"] = "US",
    ["premium"] = true,
    ["play_time_hours"] = 25.5f
};

// Create user with profile
bool success = await NovaSDK.Instance.CreateUser("player_123", userProfile);
```

### User ID Access

```csharp
// Get current user ID
string userId = NovaSDK.Instance.NovaUserId;
Debug.Log($"Current user: {userId}");
```

## 🎮 Experience Management

### Fetching Experiences

```csharp
// Load experience asset
var experience = Resources.Load<NovaExperience>("VampireSurvivalExperience");

// Fetch experience from backend
bool success = await NovaSDK.Instance.FetchExperience(experience);

if (success)
{
    Debug.Log("Experience fetched and cached successfully");
}
else
{
    Debug.LogError("Failed to fetch experience");
}
```

### Experience Validation

```csharp
// Check if experience is valid
var experience = Resources.Load<NovaExperience>("VampireSurvivalExperience");

if (experience.IsValid())
{
    Debug.Log("Experience is properly configured");
    await NovaSDK.Instance.FetchExperience(experience);
}
else
{
    Debug.LogError("Experience has validation issues");
}
```

## 📊 Value Retrieval

### Getting Values

The primary way to get configuration values is through the `GetValue<T>()` method:

```csharp
// Get different types of values
float spawnRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
float healthMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_health_multiplier");
float damageMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_damage_multiplier");
float expDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "exp_gem_drop_rate");
float coinDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "coin_drop_rate");
```

### Error Handling

```csharp
try
{
    float spawnRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
    // Use spawnRate value
}
catch (Exception ex)
{
    Debug.LogWarning($"Failed to get monster_spawn_rate_multiplier: {ex.Message}");
    // Use fallback value
    float fallbackSpawnRate = 1.0f;
}
```

**Important**: Values are only available after calling `FetchExperience()`. If you try to get a value before fetching the experience, you'll get the default value for the type (0 for numbers, false for booleans, null for strings).

## 📈 Event Tracking

### Basic Event Tracking

```csharp
// Track a simple event
await NovaSDK.Instance.TrackEvent("level_completed");

// Track event with data
var eventData = new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["time_spent"] = 120.5f
};
await NovaSDK.Instance.TrackEvent("level_completed", eventData);
```

### Common Event Types

```csharp
// Game progression events
await NovaSDK.Instance.TrackEvent("level_started", new Dictionary<string, object>
{
    ["level"] = 5,
    ["difficulty"] = "normal"
});

await NovaSDK.Instance.TrackEvent("level_completed", new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["stars"] = 3,
    ["time_spent"] = 120.5f
});

// Monetization events
await NovaSDK.Instance.TrackEvent("purchase_attempted", new Dictionary<string, object>
{
    ["item_id"] = "coin_pack_100",
    ["price"] = 0.99f,
    ["currency"] = "USD"
});

await NovaSDK.Instance.TrackEvent("purchase_completed", new Dictionary<string, object>
{
    ["item_id"] = "coin_pack_100",
    ["price"] = 0.99f,
    ["currency"] = "USD",
    ["coins_received"] = 100
});

// User engagement events
await NovaSDK.Instance.TrackEvent("app_opened");
await NovaSDK.Instance.TrackEvent("app_closed", new Dictionary<string, object>
{
    ["session_duration"] = 1800.0f
});

await NovaSDK.Instance.TrackEvent("feature_used", new Dictionary<string, object>
{
    ["feature"] = "shop",
    ["action"] = "browse_items"
});
```

## 🔄 Cache Management

### Cache Information

```csharp
// Get cache information
var cacheInfo = NovaSDK.Instance.GetCacheInfo();
Debug.Log($"Cached objects: {cacheInfo["Cached_Object_Count"]}");
```

### Cache Management

```csharp
// Clear all cached data
NovaSDK.Instance.ClearCache();
```

## 🔧 Complete Example

Here's a complete example showing the typical flow:

```csharp
using Nova.SDK;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Nova Configuration")]
    public NovaExperience gameExperience;
    
    [Header("Game Balance")]
    public float spawnRateMultiplier = 1.0f;
    public float healthMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;
    public float expDropRate = 0.1f;
    public float coinDropRate = 0.15f;

    async void Start()
    {
        // 1. Initialize Nova SDK
        NovaSDK.Instance.Initialize();
        
        // 2. Create or get user
        bool userCreated = await NovaSDK.Instance.CreateUser("player_" + SystemInfo.deviceUniqueIdentifier);
        if (!userCreated)
        {
            Debug.LogError("Failed to create user");
            return;
        }
        
        // 3. Fetch experience configuration
        if (gameExperience != null)
        {
            bool experienceLoaded = await NovaSDK.Instance.FetchExperience(gameExperience);
            if (experienceLoaded)
            {
                // 4. Load configuration values
                LoadGameBalance();
            }
            else
            {
                Debug.LogWarning("Failed to load experience, using default values");
            }
        }
    }
    
    void LoadGameBalance()
    {
        spawnRateMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
        healthMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_health_multiplier");
        damageMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_damage_multiplier");
        expDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "exp_gem_drop_rate");
        coinDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "coin_drop_rate");
        
        Debug.Log($"Game balance loaded: Spawn={spawnRateMultiplier}, Health={healthMultiplier}, Damage={damageMultiplier}");
    }
    
    public async void OnLevelCompleted(int level, int score)
    {
        await NovaSDK.Instance.TrackEvent("level_completed", new Dictionary<string, object>
        {
            ["level"] = level,
            ["score"] = score,
            ["spawn_rate"] = spawnRateMultiplier
        });
    }
}
```

## ⚠️ Important Notes

1. **Initialization Order**: Always call `Initialize()` before any other SDK methods
2. **User Creation**: Call `CreateUser()` before fetching experiences
3. **Experience Fetching**: Call `FetchExperience()` before trying to get values
4. **Error Handling**: Always handle potential failures in async operations
5. **Default Values**: The SDK returns default values (0, false, null) when values are not available
6. **Threading**: All async methods should be awaited properly 