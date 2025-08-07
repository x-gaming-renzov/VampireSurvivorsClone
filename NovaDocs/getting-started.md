# Getting Started

This guide will walk you through setting up the Nova Unity SDK in your project and making your first API calls.

## 📋 Prerequisites

- Unity 2021.3 LTS or later
- A Nova account and organization
- Your organization ID and app ID from the Nova dashboard

## 🚀 Quick Start

### 1. Install the SDK

1. **Download the SDK package** from the Nova dashboard
2. **Import the package** into your Unity project
3. **Verify installation** by checking that Nova-related scripts appear in your project

### 2. Create NovaSettings

1. **Create a NovaSettings asset**:
   - Right-click in the Project window
   - Select `Create → Nova → Settings`
   - Name it `NovaSettings`

2. **Configure the settings**:
   ```
   Organization ID: Your organization ID from Nova dashboard
   App ID: Your app ID from Nova dashboard
   API Base URL: https://api.nova.com (or your custom endpoint)
   Enable Verbose Logging: true (for debugging)
   Cache Expiration Minutes: 30
   ```

3. **Place in Resources folder**:
   - Move the `NovaSettings` asset to a `Resources` folder in your project
   - The SDK will automatically load it from there

### 3. Create Your First Experience

1. **Create a NovaExperience asset**:
   - Right-click in the Project window
   - Select `Create → Nova → Experience`
   - Name it `VampireSurvivalExperience`

2. **Configure the experience**:
   ```
   Experience Name: VampireSurvivalExperience
   Description: Main game experience for Vampire Survival
   ```

3. **Add GameObjects with NovaContext components**:
   - Create a GameObject named `GameBalanceConfig`
   - Add a `NovaContext` component
   - Set Object Name to `game_balance_config`
   - Add properties like `monster_spawn_rate_multiplier`, `monster_health_multiplier`, etc.

4. **Link to the experience**:
   - Select your `VampireSurvivalExperience` asset
   - Add the `GameBalanceConfig` GameObject to the GameObjects list

### 4. Sync Your Schema

1. **Open the Nova menu**:
   - Go to `Nova → Sync Schema to Backend`

2. **Verify the sync**:
   - Check the console for success messages
   - Verify in the Nova dashboard that your objects and experiences appear

### 5. Initialize the SDK in Code

```csharp
using Nova.SDK;
using UnityEngine;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Nova Configuration")]
    public NovaExperience gameExperience;
    
    async void Start()
    {
        // 1. Initialize the SDK
        NovaSDK.Instance.Initialize();
        
        // 2. Create a user
        bool userCreated = await NovaSDK.Instance.CreateUser("player_123");
        if (!userCreated)
        {
            Debug.LogError("Failed to create user");
            return;
        }
        
        // 3. Fetch the experience
        if (gameExperience != null)
        {
            bool experienceLoaded = await NovaSDK.Instance.FetchExperience(gameExperience);
            if (experienceLoaded)
            {
                Debug.Log("Experience loaded successfully!");
                
                // 4. Get configuration values
                float spawnRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
                Debug.Log($"Monster spawn rate: {spawnRate}");
            }
        }
    }
}
```

### 6. Test Your Setup

1. **Run your game** in the Unity editor
2. **Check the console** for initialization messages
3. **Verify values are loaded** from the backend
4. **Test event tracking**:

```csharp
// Track a test event
await NovaSDK.Instance.TrackEvent("game_started", new Dictionary<string, object>
{
    ["version"] = "1.0.0",
    ["platform"] = Application.platform.ToString()
});
```

## 🔧 Configuration Details

### NovaSettings Properties

| Property | Description | Required |
|----------|-------------|----------|
| `OrganisationId` | Your Nova organization ID | Yes |
| `AppId` | Your Nova app ID | Yes |
| `ApiBaseUrl` | Nova API endpoint | Yes |
| `EnableVerboseLogging` | Enable debug logging | No |
| `CacheExpirationMinutes` | How long to cache data | No |

### NovaContext Properties

| Property | Description | Required |
|----------|-------------|----------|
| `ObjectName` | Unique identifier for the object | Yes |
| `Properties` | List of configurable properties | Yes |

### Property Types

| Type | C# Type | Example |
|------|---------|---------|
| `Number` | `float`, `int` | `5.0`, `100` |
| `Boolean` | `bool` | `true`, `false` |
| `Text` | `string` | `"Hello World"` |
| `JSON` | `string` | `"{\"key\":\"value\"}"` |

## 🚨 Common Issues

### "NovaSettings asset is missing or invalid"

**Solution**: Make sure you have a `NovaSettings` asset in a `Resources` folder and that all required fields are filled.

### "Failed to create user"

**Solution**: Check your internet connection and verify that your `OrganisationId` and `AppId` are correct.

### "Experience not found"

**Solution**: Make sure you've synced your schema to the backend using the Nova menu.

### "Value not found in cache"

**Solution**: Ensure you've called `FetchExperience()` before trying to get values.

## 📚 Next Steps

- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[NovaContext Components](./novacontext.md)** - Creating configurable objects
- **[Event Tracking](./event-tracking.md)** - Tracking game events
- **[Examples](./examples.md)** - Practical implementation examples
- **[Troubleshooting](./troubleshooting.md)** - Common issues and solutions 