# Configuration

This guide covers how to configure Nova SDK settings and API connections.

## ⚙️ NovaSettings Asset

The **NovaSettings** asset is the central configuration file for the Nova SDK. It contains all the necessary settings to connect your Unity project to the Nova backend.

### Creating NovaSettings

1. **Create the Asset**
   ```
   Right-click in Project window → Create → Nova → Settings
   ```

2. **Location**
   - Place in `Assets/Resources/` for automatic loading
   - Or place anywhere and reference manually

### Configuration Fields

| Field | Description | Required | Example |
|-------|-------------|----------|---------|
| **Organization ID** | Your Nova organization identifier | ✅ | `"my-company"` |
| **App ID** | Your application identifier | ✅ | `"my-game-v1"` |
| **API Base URL** | Nova backend API endpoint | ✅ | `"https://api.nova.com"` |
| **Verbose Logging** | Enable detailed debug logs | ❌ | `true` |

### Settings Validation

The SDK automatically validates your settings:

```csharp
// Check if settings are valid
if (NovaSettings.IsValid())
{
    Debug.Log("Settings are properly configured");
}
else
{
    Debug.LogError("Settings are missing required fields");
}
```

## 🔧 API Configuration

### Base URL

The **API Base URL** should be provided by Nova and typically follows this format:

```
https://api.nova.com
https://api-staging.nova.com  // For staging environment
https://api-dev.nova.com      // For development environment
```

### Environment Setup

For different environments, create separate NovaSettings assets:

```
Assets/
├── Resources/
│   ├── NovaSettings.asset          // Production
│   ├── NovaSettings-Staging.asset  // Staging
│   └── NovaSettings-Dev.asset      // Development
```

### Loading Settings Programmatically

```csharp
// Load specific settings asset
var settings = Resources.Load<NovaSettings>("NovaSettings-Staging");

// Initialize SDK with custom settings
NovaSDK.Instance.Initialize(settings);
```

## 🔐 Authentication

### Organization and App IDs

Your **Organization ID** and **App ID** are provided by Nova and are used to:

- Identify your application in the Nova backend
- Route API requests to the correct environment
- Manage access permissions and quotas

### Security Best Practices

1. **Never commit sensitive IDs** to version control
2. **Use environment variables** for production builds
3. **Rotate credentials** regularly
4. **Monitor API usage** for unusual activity

## 📊 Logging Configuration

### Verbose Logging

Enable **Verbose Logging** to get detailed information about:

- API requests and responses
- Cache operations
- Event tracking
- Error details

```csharp
// Check if verbose logging is enabled
if (NovaSDK.Instance.Settings.EnableVerboseLogging)
{
    Debug.Log("Verbose logging is enabled");
}
```

### Log Levels

The SDK provides different levels of logging:

- **Info**: General operational messages
- **Warning**: Non-critical issues
- **Error**: Critical problems that need attention

### Custom Logging

You can implement custom logging by extending the SDK:

```csharp
// Example: Custom logging implementation
public class CustomLogger : INovaLogger
{
    public void LogInfo(string message)
    {
        Debug.Log($"[Nova Info] {message}");
    }
    
    public void LogWarning(string message)
    {
        Debug.LogWarning($"[Nova Warning] {message}");
    }
    
    public void LogError(string message)
    {
        Debug.LogError($"[Nova Error] {message}");
    }
}
```

## 🌐 Network Configuration

### Timeout Settings

Configure network timeouts for different operations:

```csharp
// Default timeouts (in seconds)
var timeouts = new Dictionary<string, float>
{
    ["api_request"] = 30.0f,
    ["schema_push"] = 60.0f,
    ["event_batch"] = 10.0f
};
```

### Retry Logic

The SDK automatically retries failed requests with exponential backoff:

- **Initial delay**: 1 second
- **Maximum delay**: 30 seconds
- **Maximum retries**: 3 attempts

### Offline Handling

The SDK gracefully handles offline scenarios:

```csharp
// Check if SDK is online
if (NovaSDK.Instance.IsOnline)
{
    // Perform online operations
    await NovaSDK.Instance.FetchExperience(experience);
}
else
{
    // Use cached values
    var value = NovaSDK.Instance.GetValue<float>("PlayerConfig", "speed");
}
```

## 🔄 Cache Configuration

### Cache Settings

Configure how the SDK caches data:

```csharp
// Cache configuration
var cacheSettings = new CacheSettings
{
    MaxCacheSize = 100 * 1024 * 1024,  // 100MB
    CacheExpirationHours = 24,
    EnableCompression = true
};
```

### Cache Management

```csharp
// Clear all cached data
NovaSDK.Instance.ClearCache();

// Get cache information
var cacheInfo = NovaSDK.Instance.GetCacheInfo();
Debug.Log($"Cache size: {cacheInfo["size"]} bytes");
Debug.Log($"Cache entries: {cacheInfo["entries"]}");
```

## 🚀 Performance Configuration

### Event Batching

Configure event batching for better performance:

```csharp
// Event batching settings
var batchSettings = new EventBatchSettings
{
    BatchSize = 50,
    BatchTimeoutSeconds = 30,
    EnableBatching = true
};
```

### Background Processing

Configure background operations:

```csharp
// Background processing settings
var backgroundSettings = new BackgroundSettings
{
    EnableBackgroundSync = true,
    SyncIntervalMinutes = 15,
    EnableBackgroundEvents = true
};
```

## 🔧 Advanced Configuration

### Custom API Headers

Add custom headers to API requests:

```csharp
// Add custom headers
var customHeaders = new Dictionary<string, string>
{
    ["X-Custom-Header"] = "custom-value",
    ["User-Agent"] = "MyGame/1.0"
};

// Apply headers to SDK
NovaSDK.Instance.SetCustomHeaders(customHeaders);
```

### Proxy Configuration

Configure proxy settings for corporate networks:

```csharp
// Proxy configuration
var proxySettings = new ProxySettings
{
    ProxyUrl = "http://proxy.company.com:8080",
    Username = "username",
    Password = "password"
};
```

## 📱 Platform-Specific Configuration

### Mobile Platforms

For mobile platforms, consider these settings:

```csharp
// Mobile-optimized settings
var mobileSettings = new MobileSettings
{
    EnableBatteryOptimization = true,
    ReduceNetworkUsage = true,
    CacheAggressively = true
};
```

### WebGL Platform

For WebGL builds:

```csharp
// WebGL-specific settings
var webglSettings = new WebGLSettings
{
    UseLocalStorage = true,
    EnableCrossOriginRequests = true,
    RequestTimeoutSeconds = 60
};
```

## 🔍 Configuration Validation

### Validation Checklist

Before deploying, verify your configuration:

- [ ] **Organization ID** is correct and active
- [ ] **App ID** matches your application
- [ ] **API Base URL** is accessible from your environment
- [ ] **Network connectivity** is working
- [ ] **SSL certificates** are valid (for HTTPS)
- [ ] **Firewall rules** allow API access
- [ ] **Rate limits** are appropriate for your usage

### Testing Configuration

```csharp
// Test configuration connectivity
public async Task TestConfiguration()
{
    try
    {
        NovaSDK.Instance.Initialize();
        
        // Test API connectivity
        var success = await NovaSDK.Instance.CreateUser("test_user");
        
        if (success)
        {
            Debug.Log("Configuration test successful!");
        }
        else
        {
            Debug.LogError("Configuration test failed");
        }
    }
    catch (Exception ex)
    {
        Debug.LogError($"Configuration test error: {ex.Message}");
    }
}
```

## 🔗 Related Documentation

- **[Getting Started](./getting-started.md)** - Initial setup guide
- **[Runtime API](./runtime-api.md)** - API reference
- **[Troubleshooting](./troubleshooting.md)** - Common configuration issues
- **[Examples](./examples.md)** - Configuration examples 