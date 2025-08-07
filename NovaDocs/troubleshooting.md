# Troubleshooting

This guide helps you resolve common issues when using the Nova Unity SDK.

## 🚨 Common Error Messages

### "NovaSettings asset is missing or invalid"

**Problem**: The SDK can't find or load the NovaSettings configuration.

**Solutions**:
1. **Check file location**: Ensure `NovaSettings` is in a `Resources` folder
2. **Verify asset name**: The asset must be named exactly `NovaSettings`
3. **Check configuration**: All required fields must be filled:
   - Organization ID
   - App ID
   - API Base URL

```csharp
// Debug: Check if settings are loaded
if (NovaSDK.Instance.Settings == null)
{
    Debug.LogError("NovaSettings not found in Resources folder");
}
```

### "Failed to create user"

**Problem**: User creation is failing.

**Solutions**:
1. **Check internet connection**: Ensure your device has internet access
2. **Verify credentials**: Check your Organization ID and App ID are correct
3. **Check API endpoint**: Verify the API Base URL is accessible
4. **Enable verbose logging**: Set `EnableVerboseLogging = true` in NovaSettings

```csharp
// Debug: Check user creation with error handling
try
{
    bool success = await NovaSDK.Instance.CreateUser("test_user");
    if (!success)
    {
        Debug.LogError("User creation returned false");
    }
}
catch (Exception ex)
{
    Debug.LogError($"User creation failed: {ex.Message}");
}
```

### "Experience not found"

**Problem**: The backend can't find the experience you're trying to fetch.

**Solutions**:
1. **Sync schema first**: Use `Nova → Sync Schema to Backend` in Unity
2. **Check experience name**: Verify the experience name matches exactly
3. **Verify in dashboard**: Check that the experience appears in Nova dashboard
4. **Check organization/app**: Ensure you're using the correct org/app IDs

```csharp
// Debug: Check experience name
var experience = Resources.Load<NovaExperience>("VampireSurvivalExperience");
Debug.Log($"Experience name: {experience.ExperienceName}");
```

### "Value not found in cache"

**Problem**: `GetValue()` returns default values (0, false, null).

**Solutions**:
1. **Call FetchExperience first**: Always fetch before getting values
2. **Check object name**: Verify the object name matches exactly
3. **Check property name**: Verify the property name matches exactly
4. **Check cache**: Use `GetCacheInfo()` to see what's cached

```csharp
// Debug: Check cache contents
var cacheInfo = NovaSDK.Instance.GetCacheInfo();
Debug.Log($"Cached objects: {cacheInfo["Cached_Object_Count"]}");

// Debug: Check specific value
var feature = NovaSDK.Instance.Cache.GetObject("game_balance_config");
if (feature != null)
{
    Debug.Log($"Feature config: {string.Join(", ", feature.config.Keys)}");
}
```

### "Cannot convert value to type"

**Problem**: Type conversion is failing when getting values.

**Solutions**:
1. **Check property type**: Ensure the property type matches your C# type
2. **Use correct type**: Use `float` for numbers, `bool` for booleans, `string` for text
3. **Handle conversion errors**: Wrap in try-catch

```csharp
// Debug: Safe value retrieval
try
{
    float value = NovaSDK.Instance.GetValue<float>("object_name", "property_name");
    Debug.Log($"Retrieved value: {value}");
}
catch (Exception ex)
{
    Debug.LogError($"Value conversion failed: {ex.Message}");
    // Use fallback value
    float fallbackValue = 1.0f;
}
```

## 🔧 Debugging Techniques

### Enable Verbose Logging

```csharp
// In NovaSettings asset
EnableVerboseLogging = true
```

This will show detailed API requests and responses in the console.

### Check SDK Status

```csharp
// Check initialization
Debug.Log($"SDK Initialized: {NovaSDK.Instance.IsInitialized}");

// Check user
Debug.Log($"User ID: {NovaSDK.Instance.NovaUserId}");

// Check cache
var cacheInfo = NovaSDK.Instance.GetCacheInfo();
Debug.Log($"Cache info: {string.Join(", ", cacheInfo.Select(kv => $"{kv.Key}={kv.Value}"))}");
```

### Test API Connectivity

```csharp
// Test basic connectivity
async void TestConnection()
{
    try
    {
        bool success = await NovaSDK.Instance.CreateUser("test_user");
        Debug.Log($"Connection test: {(success ? "PASSED" : "FAILED")}");
    }
    catch (Exception ex)
    {
        Debug.LogError($"Connection test failed: {ex.Message}");
    }
}
```

## 🐛 Common Implementation Issues

### Wrong Initialization Order

**Problem**: Calling SDK methods before initialization.

**Solution**: Always follow this order:
1. `NovaSDK.Instance.Initialize()`
2. `NovaSDK.Instance.CreateUser()`
3. `NovaSDK.Instance.FetchExperience()`
4. `NovaSDK.Instance.GetValue()`

```csharp
// Correct order
async void Start()
{
    // 1. Initialize
    NovaSDK.Instance.Initialize();
    
    // 2. Create user
    await NovaSDK.Instance.CreateUser("user_id");
    
    // 3. Fetch experience
    await NovaSDK.Instance.FetchExperience(experience);
    
    // 4. Get values
    float value = NovaSDK.Instance.GetValue<float>("object", "property");
}
```

### Missing Await Keywords

**Problem**: Not awaiting async methods.

**Solution**: Always await async methods:

```csharp
// Wrong
NovaSDK.Instance.CreateUser("user_id");
NovaSDK.Instance.FetchExperience(experience);

// Correct
await NovaSDK.Instance.CreateUser("user_id");
await NovaSDK.Instance.FetchExperience(experience);
```

### Incorrect Object/Property Names

**Problem**: Names don't match between Unity and backend.

**Solution**: Use exact names:

```csharp
// Check your NovaContext Object Name
// Check your property names in NovaContext
// Use those exact names in GetValue()

// Example: If NovaContext Object Name is "game_balance_config"
// and property is "monster_spawn_rate_multiplier"
float value = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
```

## 🔍 Advanced Debugging

### Network Debugging

```csharp
// Check network requests in Unity Console
// Look for [Nova API] log messages when verbose logging is enabled
```

### Cache Debugging

```csharp
// Clear cache and retry
NovaSDK.Instance.ClearCache();
await NovaSDK.Instance.FetchExperience(experience);
```

### Schema Debugging

```csharp
// Check if schema is synced
// Use Nova → Sync Schema to Backend
// Check console for sync results
```

## 📞 Getting Help

If you're still experiencing issues:

1. **Enable verbose logging** and check console output
2. **Check this troubleshooting guide** for your specific error
3. **Verify your setup** using the getting started guide
4. **Contact support** with:
   - Error message
   - Console logs
   - Your NovaSettings configuration (without sensitive data)
   - Steps to reproduce the issue

## 🔗 Related Documentation

- **[Getting Started](./getting-started.md)** - Initial setup guide
- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[Examples](./examples.md)** - Working code examples 