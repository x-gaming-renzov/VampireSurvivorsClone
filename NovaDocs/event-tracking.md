# Event Tracking

Event tracking is a crucial part of Nova's experimentation and analytics system. This guide covers how to track user behavior and game events to measure experiment performance and understand user engagement.

## 🎯 Why Track Events?

Event tracking enables you to:

- **Measure experiment performance** - Compare metrics between A/B test variants
- **Understand user behavior** - Identify patterns and optimization opportunities
- **Optimize game balance** - Fine-tune parameters based on real data
- **Monitor feature usage** - Track adoption and engagement of new features
- **Debug issues** - Identify problems through user behavior patterns

## 📊 Basic Event Tracking

### Simple Events

Track basic events without additional data:

```csharp
// Track a simple event
NovaSDK.Instance.TrackEvent("level_completed");
NovaSDK.Instance.TrackEvent("purchase_made");
NovaSDK.Instance.TrackEvent("tutorial_started");
```

### Events with Data

Track events with contextual information:

```csharp
// Track event with data
var eventData = new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["time_spent"] = 120.5f
};
NovaSDK.Instance.TrackEvent("level_completed", eventData);
```

## 🎮 Game Event Categories

### Progression Events

Track player advancement through your game:

```csharp
// Level progression
NovaSDK.Instance.TrackEvent("level_started", new Dictionary<string, object>
{
    ["level"] = 5,
    ["difficulty"] = "normal",
    ["attempt_number"] = 1
});

NovaSDK.Instance.TrackEvent("level_completed", new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["stars"] = 3,
    ["time_spent"] = 120.5f,
    ["attempts"] = 2
});

// Achievement events
NovaSDK.Instance.TrackEvent("achievement_unlocked", new Dictionary<string, object>
{
    ["achievement_id"] = "first_win",
    ["achievement_name"] = "First Victory",
    ["points"] = 100
});
```

### Monetization Events

Track revenue-related activities:

```csharp
// Purchase events
NovaSDK.Instance.TrackEvent("purchase_attempted", new Dictionary<string, object>
{
    ["item_id"] = "coin_pack_100",
    ["price"] = 0.99f,
    ["currency"] = "USD",
    ["platform"] = "ios"
});

NovaSDK.Instance.TrackEvent("purchase_completed", new Dictionary<string, object>
{
    ["item_id"] = "coin_pack_100",
    ["price"] = 0.99f,
    ["currency"] = "USD",
    ["coins_received"] = 100,
    ["transaction_id"] = "txn_123456"
});

// Ad events
NovaSDK.Instance.TrackEvent("ad_watched", new Dictionary<string, object>
{
    ["ad_type"] = "rewarded",
    ["reward_type"] = "coins",
    ["reward_amount"] = 50,
    ["placement"] = "level_complete"
});
```

### Engagement Events

Track user interaction and engagement:

```csharp
// Session events
NovaSDK.Instance.TrackEvent("app_opened");
NovaSDK.Instance.TrackEvent("app_closed", new Dictionary<string, object>
{
    ["session_duration"] = 1800.0f,
    ["screens_visited"] = 5
});

// Feature usage
NovaSDK.Instance.TrackEvent("feature_used", new Dictionary<string, object>
{
    ["feature"] = "shop",
    ["action"] = "browse_items",
    ["time_spent"] = 45.2f
});

// Social events
NovaSDK.Instance.TrackEvent("friend_invited", new Dictionary<string, object>
{
    ["invitation_method"] = "facebook",
    ["friends_count"] = 15
});
```

### Gameplay Events

Track specific gameplay actions:

```csharp
// Combat events
NovaSDK.Instance.TrackEvent("enemy_defeated", new Dictionary<string, object>
{
    ["enemy_type"] = "goblin",
    ["enemy_level"] = 3,
    ["damage_dealt"] = 150,
    ["weapon_used"] = "sword"
});

// Resource events
NovaSDK.Instance.TrackEvent("resource_collected", new Dictionary<string, object>
{
    ["resource_type"] = "coins",
    ["amount"] = 25,
    ["source"] = "level_completion"
});

// Power-up events
NovaSDK.Instance.TrackEvent("powerup_used", new Dictionary<string, object>
{
    ["powerup_type"] = "shield",
    ["duration"] = 10.0f,
    ["effectiveness"] = 0.8f
});
```

## 📈 Experiment Tracking

### A/B Test Events

Track events specifically for experiment analysis:

```csharp
// Track experiment exposure
NovaSDK.Instance.TrackEvent("experiment_exposed", new Dictionary<string, object>
{
    ["experiment_id"] = "button_color_test",
    ["variant"] = "blue",
    ["user_id"] = NovaSDK.Instance.NovaUserId
});

// Track conversion events
NovaSDK.Instance.TrackEvent("conversion", new Dictionary<string, object>
{
    ["experiment_id"] = "button_color_test",
    ["conversion_type"] = "purchase",
    ["value"] = 0.99f
});
```

### Feature Flag Events

Track feature flag usage:

```csharp
// Track feature flag exposure
NovaSDK.Instance.TrackEvent("feature_flag_exposed", new Dictionary<string, object>
{
    ["flag_name"] = "new_ui_enabled",
    ["enabled"] = true,
    ["user_id"] = NovaSDK.Instance.NovaUserId
});
```

## 🔄 Event Batching

### Automatic Batching

Events are automatically batched for better performance:

```csharp
// Multiple events are queued and sent in batches
for (int i = 0; i < 100; i++)
{
    NovaSDK.Instance.TrackEvent("enemy_defeated", new Dictionary<string, object>
    {
        ["enemy_type"] = "goblin",
        ["level"] = i
    });
}

// Events are automatically sent in batches
// No need to manually flush for normal usage
```

### Manual Flushing

Force send queued events:

```csharp
// Manually flush the event queue
NovaSDK.Instance.FlushEventQueue();

// Useful before app close or important moments
void OnApplicationPause(bool pauseStatus)
{
    if (pauseStatus)
    {
        NovaSDK.Instance.FlushEventQueue();
    }
}
```

## 📊 Event Data Best Practices

### Data Types

Use appropriate data types for your event properties:

```csharp
// Good: Use appropriate types
var eventData = new Dictionary<string, object>
{
    ["level"] = 5,                    // int
    ["score"] = 1500.5f,              // float
    ["completed"] = true,              // bool
    ["player_name"] = "Player123",     // string
    ["items"] = new string[] { "sword", "shield" }  // array
};

// Avoid: Don't use complex objects directly
// var badData = new Dictionary<string, object>
// {
//     ["player"] = playerObject,  // Don't do this
//     ["transform"] = transform   // Don't do this
// };
```

### Naming Conventions

Follow consistent naming patterns:

```csharp
// Use snake_case for event names and properties
NovaSDK.Instance.TrackEvent("level_completed");  // Good
NovaSDK.Instance.TrackEvent("LevelCompleted");   // Avoid

// Use descriptive, specific names
NovaSDK.Instance.TrackEvent("purchase_completed");  // Good
NovaSDK.Instance.TrackEvent("buy");                 // Too vague
```

### Data Consistency

Ensure consistent data structure:

```csharp
// Always include the same properties for similar events
NovaSDK.Instance.TrackEvent("level_completed", new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["time_spent"] = 120.5f,
    ["attempts"] = 2
});

// Use the same structure for all level_completed events
// Don't mix different properties for the same event type
```

## 🚀 Performance Optimization

### Event Frequency

Consider the frequency of your events:

```csharp
// High-frequency events (track every frame/update)
// Consider batching or sampling
void Update()
{
    // Don't track every frame
    // NovaSDK.Instance.TrackEvent("player_position"); // Bad
    
    // Instead, track periodically
    if (Time.frameCount % 60 == 0) // Every 60 frames
    {
        NovaSDK.Instance.TrackEvent("player_position", new Dictionary<string, object>
        {
            ["x"] = transform.position.x,
            ["y"] = transform.position.y
        });
    }
}
```

### Data Size

Keep event data compact:

```csharp
// Good: Compact data
NovaSDK.Instance.TrackEvent("item_purchased", new Dictionary<string, object>
{
    ["item_id"] = "sword_01",
    ["price"] = 100
});

// Avoid: Large data
// NovaSDK.Instance.TrackEvent("item_purchased", new Dictionary<string, object>
// {
//     ["item_id"] = "sword_01",
//     ["price"] = 100,
//     ["description"] = "A very long description...", // Don't include large text
//     ["full_item_data"] = largeObject // Don't include large objects
// });
```

## 🔍 Debugging Events

### Enable Verbose Logging

```csharp
// In NovaSettings, enable Verbose Logging
// This will show event tracking details in the console
```

### Event Validation

```csharp
// Validate event data before sending
public void TrackValidatedEvent(string eventName, Dictionary<string, object> eventData)
{
    // Check for required fields
    if (string.IsNullOrEmpty(eventName))
    {
        Debug.LogError("Event name cannot be null or empty");
        return;
    }
    
    // Validate data types
    foreach (var kvp in eventData)
    {
        if (kvp.Value == null)
        {
            Debug.LogWarning($"Event property '{kvp.Key}' is null");
        }
    }
    
    // Track the event
    NovaSDK.Instance.TrackEvent(eventName, eventData);
}
```

### Event Monitoring

```csharp
// Monitor event tracking performance
public class EventTracker : MonoBehaviour
{
    private int eventsTracked = 0;
    private float lastReportTime = 0f;
    
    public void TrackEventWithMonitoring(string eventName, Dictionary<string, object> eventData = null)
    {
        var startTime = Time.time;
        
        NovaSDK.Instance.TrackEvent(eventName, eventData);
        
        eventsTracked++;
        
        // Report every 100 events
        if (eventsTracked % 100 == 0)
        {
            var currentTime = Time.time;
            var timeSinceLastReport = currentTime - lastReportTime;
            Debug.Log($"Tracked {eventsTracked} events in {timeSinceLastReport:F2} seconds");
            lastReportTime = currentTime;
        }
    }
}
```

## 🔗 Related Documentation

- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[User Management](./user-management.md)** - Managing user profiles
- **[Examples](./examples.md)** - Event tracking examples
- **[Troubleshooting](./troubleshooting.md)** - Common event tracking issues 