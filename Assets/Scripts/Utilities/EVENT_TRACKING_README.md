# Event Tracking System - Vampire Survival Game

## 🎯 Overview

This event tracking system integrates with NovaSDK to track player behavior, game progression, and performance metrics. It provides a centralized way to monitor user engagement and optimize your game.

## 🚀 Quick Start

### 1. Automatic Setup
The `EventTracker` is automatically created when your `NovaManager` initializes. No manual setup required!

### 2. Basic Usage
```csharp
// Track a simple event
EventTracker.Instance.TrackEventSafely("player_moved");

// Track event with data
EventTracker.Instance.TrackEventSafely("enemy_defeated", new Dictionary<string, object>
{
    ["enemy_type"] = "Goblin",
    ["damage_dealt"] = 150,
    ["weapon_used"] = "Sword"
});
```

## 📊 Available Event Types

### Game Progression Events
```csharp
// Level started
EventTracker.Instance.TrackLevelStarted(5, "hard");

// Level completed
EventTracker.Instance.TrackLevelCompleted(5, 1500, 120.5f);
```

### Combat Events
```csharp
// Enemy defeated
EventTracker.Instance.TrackEnemyDefeated("Goblin", 150, "Sword");

// Player damaged
EventTracker.Instance.TrackPlayerDamaged(25f, 75f, "Goblin Attack");
```

### Resource Collection Events
```csharp
// Resource collected
EventTracker.Instance.TrackResourceCollected("Coins", 50, "Enemy Drop");
```

### Ability Usage Events
```csharp
// Ability used
EventTracker.Instance.TrackAbilityUsed("Fireball", "Projectile", 0.8f);
```

### Feature Usage Events
```csharp
// Feature started
EventTracker.Instance.TrackFeatureUsageStart("Shop");

// Feature ended (with usage time)
EventTracker.Instance.TrackFeatureUsageEnd("Shop");
```

### Performance & Error Events
```csharp
// Performance metric
EventTracker.Instance.TrackPerformance("fps", 60f, "fps");

// Error occurred
EventTracker.Instance.TrackError("Combat", "Invalid weapon damage", stackTrace);
```

## 🔧 Integration Examples

### In Player Controller
```csharp
public class PlayerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            // Track collectible pickup
            EventTracker.Instance.TrackResourceCollected(
                other.name, 
                1, 
                "level_pickup"
            );
        }
    }
    
    public void TakeDamage(float damage, string source)
    {
        // Track player damage
        EventTracker.Instance.TrackPlayerDamaged(
            damage, 
            currentHealth, 
            source
        );
    }
}
```

### In Monster/Enemy System
```csharp
public class Monster : MonoBehaviour
{
    public void OnDeath()
    {
        // Track enemy defeat
        EventTracker.Instance.TrackEnemyDefeated(
            monsterType, 
            lastDamageDealt, 
            lastWeaponUsed
        );
    }
}
```

### In Level Manager
```csharp
public class LevelManager : MonoBehaviour
{
    public void StartLevel(int levelNumber)
    {
        EventTracker.Instance.TrackLevelStarted(levelNumber);
    }
    
    public void CompleteLevel(int levelNumber, int score, float timeSpent)
    {
        EventTracker.Instance.TrackLevelCompleted(levelNumber, score, timeSpent);
    }
}
```

## 🎮 Testing Events

### Using EventTrackerTest (Recommended)
1. Add the `EventTrackerTest` component to any GameObject
2. Enable `testOnStart` to run tests automatically
3. Use context menu items to test different event types
4. Check console for test results and event tracking confirmation

### Using Context Menu (Editor Only)
1. Add the `EventTrackingExamples` component to any GameObject
2. Right-click the component in the Inspector
3. Use the context menu items to test different events

### Manual Testing
```csharp
// Test from any script
if (EventTracker.Instance != null)
{
    EventTracker.Instance.TrackEventSafely("test_event", new Dictionary<string, object>
    {
        ["test_value"] = 42,
        ["timestamp"] = System.DateTime.Now.ToString()
    });
}
```

### Event Batching
Events are automatically batched and sent by NovaSDK. No manual flushing is required.

## 📈 Event Data Structure

### Standard Properties
All events automatically include:
- `timestamp`: When the event occurred
- `user_id`: Nova user identifier
- `platform`: Device platform (iOS, Android, etc.)
- `app_version`: Game version

### Custom Properties
Add game-specific data:
```csharp
var eventData = new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["difficulty"] = "hard",
    ["weapon"] = "Sword",
    ["enemy_count"] = 10
};

EventTracker.Instance.TrackEventSafely("custom_event", eventData);
```

## 🔍 Monitoring & Debugging

### Console Messages
Watch for these messages in Unity Console:
```
✅ EventTracker created and added to scene
✅ Event tracking initialized successfully
✅ Event tracked: [event_name]
```

### Error Handling
Events are automatically validated:
- SDK initialization check
- User creation check
- Exception handling with detailed logging

### Verbose Logging
Enable verbose logging in NovaSettings to see detailed event tracking information. The EventTracker will log all successful events to the console.

## 🚨 Best Practices

### 1. Event Naming
- Use descriptive, consistent names
- Use snake_case format: `level_completed`, `enemy_defeated`
- Avoid vague names like `event1`, `test`

### 2. Data Consistency
- Always include the same properties for similar events
- Use appropriate data types (int, float, string, bool)
- Avoid including large objects or complex data

### 3. Event Frequency
- Don't track events every frame
- Batch related events when possible
- Consider performance impact for high-frequency events

### 4. Error Handling
- Always check if EventTracker.Instance exists
- Handle cases where NovaSDK isn't initialized
- Use try-catch blocks for critical events

## 🔗 Related Files

- `EventTracker.cs` - Main event tracking system
- `NovaManager.cs` - NovaSDK integration and initialization
- `EventTrackingExamples.cs` - Usage examples and testing
- `EventTrackerTest.cs` - Testing and debugging script
- `NovaConfig.cs` - Game configuration system

## 📚 Next Steps

1. **Test the system** using the example methods
2. **Integrate events** into your existing game systems
3. **Monitor events** in your Nova dashboard
4. **Customize events** based on your specific needs
5. **Add new event types** as your game evolves

## 🆘 Troubleshooting

### EventTracker.Instance is null
- Ensure NovaManager is in the scene
- Check that NovaSDK initialization completed successfully
- Verify EventTracker was created during initialization

### Events not appearing in Nova dashboard
- Check internet connection
- Verify NovaSDK configuration
- Look for error messages in console
- Ensure events are being flushed (automatic on app close)

### Performance issues
- Reduce event frequency
- Simplify event data
- Use event batching for high-frequency events
