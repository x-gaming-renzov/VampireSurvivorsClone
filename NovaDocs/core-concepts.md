# Core Concepts

This guide explains the fundamental concepts behind Nova's experimentation and personalization platform.

## 🎯 What is Nova?

Nova is a real-time experimentation and personalization platform that allows you to:

- **Update game parameters** without app store updates
- **Run A/B tests** to optimize user experience
- **Personalize content** based on user profiles
- **Track user behavior** and experiment performance
- **Manage configurations** through a web dashboard

## 🏗️ Architecture Overview

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Unity Game    │    │   Nova SDK      │    │   Nova Backend  │
│                 │    │                 │    │                 │
│ ┌─────────────┐ │    │ ┌─────────────┐ │    │ ┌─────────────┐ │
│ │ NovaContext │◄┼────┼►│ NovaSDK     │◄┼────┼►│ API Server  │ │
│ │ Components  │ │    │ │ Instance    │ │    │ │             │ │
│ └─────────────┘ │    │ └─────────────┘ │    │ └─────────────┘ │
│                 │    │                 │    │                 │
│ ┌─────────────┐ │    │ ┌─────────────┐ │    │ ┌─────────────┐ │
│ │NovaExperience│◄┼────┼►│ Cache &     │◄┼────┼►│ Dashboard   │ │
│ │ Assets      │ │    │ │ Event Queue │ │    │ │             │ │
│ └─────────────┘ │    │ └─────────────┘ │    │ └─────────────┘ │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 📋 Key Components

### 1. NovaContext Components

**NovaContext** components define configurable objects in your game. Each context represents a logical group of related properties.

```csharp
// Example: Player configuration context
Object Name: "PlayerConfig"
Properties:
  - player_speed (number, default: 5.0)
  - show_tutorial (boolean, default: true)
  - starting_coins (number, default: 100)
```

**Key Features:**
- **Object Name**: Unique identifier for the context
- **Properties**: Configurable parameters with types and default values
- **Runtime Access**: Values can be retrieved during gameplay
- **Real-time Updates**: Changes sync from backend automatically

### 2. NovaExperience Assets

**NovaExperience** assets group related NovaContext components into logical experiences. This allows you to:

- Organize configurations by feature or user journey
- Run experiments on specific experiences
- Manage different game modes or user segments

```csharp
// Example: Main game experience
Experience Name: "MainGameExperience"
Description: "Core gameplay configuration"
GameObjects:
  - PlayerConfig (NovaContext)
  - UISettings (NovaContext)
  - GameBalance (NovaContext)
```

### 3. NovaSDK Instance

The **NovaSDK** is the main entry point for all Nova functionality:

```csharp
// Singleton instance
NovaSDK.Instance.Initialize();
NovaSDK.Instance.CreateUser("user_id");
NovaSDK.Instance.GetValue<float>("PlayerConfig", "player_speed");
```

## 🔄 Data Flow

### 1. Configuration Flow

```
1. Create NovaContext components in Unity
2. Group into NovaExperience assets
3. Push schema to Nova backend
4. Configure values in Nova dashboard
5. Values sync to game at runtime
```

### 2. Runtime Flow

```
1. Game starts → NovaSDK.Initialize()
2. Create user → NovaSDK.CreateUser()
3. Fetch experience → NovaSDK.FetchExperience()
4. Get values → NovaSDK.GetValue<T>()
5. Track events → NovaSDK.TrackEvent()
```

### 3. Caching Strategy

- **Default Values**: Always available from NovaContext components
- **Remote Values**: Cached locally after first fetch
- **Offline Support**: Game works with cached values when offline
- **Background Sync**: Values update automatically when online

## 🎮 Property Types

Nova supports several property types for different use cases:

| Type | C# Type | Use Case | Example |
|------|---------|----------|---------|
| `number` | `float`, `int`, `double` | Numeric values | Player speed, prices, timers |
| `boolean` | `bool` | True/false flags | Feature toggles, tutorials |
| `text` | `string` | Text content | Messages, names, descriptions |
| `JSON` | `string` | Complex data | Arrays, objects, configurations |

## 👥 User Management

### User Identification

Users are identified by a unique string ID that you provide:

```csharp
// Create user with unique ID
await NovaSDK.Instance.CreateUser("player_123");
```

### User Profiles

You can attach custom data to users for personalization:

```csharp
var userProfile = new Dictionary<string, object>
{
    ["level"] = 5,
    ["country"] = "US",
    ["premium"] = true
};

await NovaSDK.Instance.CreateUser("player_123", userProfile);
```

## 🧪 Experimentation Concepts

### A/B Testing

Nova enables A/B testing by allowing you to:

1. **Define variants** of your configurations
2. **Assign users** to different variants
3. **Track metrics** to measure performance
4. **Analyze results** to determine winners

### Personalization

Personalize content based on:

- **User attributes** (level, country, preferences)
- **Behavior patterns** (play time, spending habits)
- **Contextual data** (time of day, device type)

## 📊 Event Tracking

Track user behavior to:

- **Measure experiment performance**
- **Understand user engagement**
- **Optimize game balance**
- **Identify issues and opportunities**

```csharp
// Track a simple event
NovaSDK.Instance.TrackEvent("level_completed");

// Track event with data
var eventData = new Dictionary<string, object>
{
    ["level"] = 5,
    ["score"] = 1500,
    ["time_spent"] = 120.5f
};
NovaSDK.Instance.TrackEvent("level_completed", eventData);
```

## 🔧 Configuration Management

### Schema Management

The **schema** defines your game's configurable structure:

- **Objects**: Your NovaContext components
- **Experiences**: How objects are grouped
- **Properties**: What can be configured
- **Types**: Data types for each property

### Version Control

- **Schema versions** are tracked automatically
- **Backward compatibility** is maintained
- **Rollback** to previous configurations
- **Environment separation** (dev, staging, production)

## 🚀 Best Practices

### 1. Naming Conventions

- **Object Names**: Use descriptive, lowercase names with underscores
- **Property Names**: Be specific and consistent
- **Experience Names**: Group related functionality

### 2. Property Design

- **Start with defaults**: Always provide sensible default values
- **Use appropriate types**: Choose the right data type for each property
- **Document properties**: Add descriptions for clarity

### 3. Performance

- **Cache values**: Don't call GetValue repeatedly for the same property
- **Batch events**: Use event batching for high-frequency events
- **Handle offline**: Design for offline-first operation

### 4. Testing

- **Test configurations**: Verify all property combinations work
- **Validate schemas**: Ensure schema pushes successfully
- **Monitor performance**: Watch for API rate limits and errors

## 🔗 Related Documentation

- **[NovaContext Components](./novacontext.md)** - Detailed guide to creating contexts
- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[Examples](./examples.md)** - Practical implementation examples
- **[Troubleshooting](./troubleshooting.md)** - Common issues and solutions 