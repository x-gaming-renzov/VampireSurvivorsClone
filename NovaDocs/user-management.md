# User Management

User management is a core feature of Nova that enables personalization and user-specific configurations. This guide covers creating users, managing profiles, and using user data for personalization.

## 👥 User Concepts

### User Identification

Users in Nova are identified by a unique string ID that you provide. This ID should be:

- **Unique**: Each user should have a different ID
- **Consistent**: The same user should always use the same ID
- **Persistent**: Should survive app reinstalls and device changes
- **Secure**: Should not contain sensitive information

### User Profiles

User profiles contain custom data that can be used for:

- **Personalization**: Tailor experiences based on user attributes
- **Segmentation**: Group users for A/B testing
- **Analytics**: Understand user demographics and behavior
- **Targeting**: Deliver specific content to user segments

## 🚀 Creating Users

### Basic User Creation

```csharp
// Create a user with just an ID
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

### User Creation with Profile

```csharp
// Create user profile data
var userProfile = new Dictionary<string, object>
{
    ["level"] = 5,
    ["country"] = "US",
    ["premium"] = true,
    ["play_time_hours"] = 25.5f,
    ["device_type"] = "mobile",
    ["language"] = "en"
};

// Create user with profile
bool success = await NovaSDK.Instance.CreateUser("player_123", userProfile);
```

### User ID Generation

Generate consistent user IDs:

```csharp
public class UserManager : MonoBehaviour
{
    private const string USER_ID_KEY = "nova_user_id";
    
    public string GetOrCreateUserId()
    {
        // Try to get existing user ID
        string userId = PlayerPrefs.GetString(USER_ID_KEY, "");
        
        if (string.IsNullOrEmpty(userId))
        {
            // Generate new user ID
            userId = GenerateUserId();
            PlayerPrefs.SetString(USER_ID_KEY, userId);
            PlayerPrefs.Save();
        }
        
        return userId;
    }
    
    private string GenerateUserId()
    {
        // Generate a unique user ID
        // You can use various strategies:
        
        // Option 1: UUID/GUID
        string userId = System.Guid.NewGuid().ToString();
        
        // Option 2: Device ID + timestamp
        // string deviceId = SystemInfo.deviceUniqueIdentifier;
        // string timestamp = DateTime.Now.Ticks.ToString();
        // string userId = $"{deviceId}_{timestamp}";
        
        // Option 3: Random string
        // string userId = "user_" + UnityEngine.Random.Range(100000, 999999);
        
        return userId;
    }
}
```

## 📊 User Profile Management

### Updating User Profiles

```csharp
// Update user profile with new data
var updatedProfile = new Dictionary<string, object>
{
    ["level"] = 10,
    ["coins"] = 1500,
    ["last_login"] = DateTime.Now.ToString("yyyy-MM-dd"),
    ["total_play_time"] = 50.5f,
    ["achievements"] = new string[] { "first_win", "level_10" }
};

bool success = await NovaSDK.Instance.UpdateUserProfile(updatedProfile);
```

### Incremental Profile Updates

```csharp
// Update specific profile fields
public async Task UpdateUserLevel(int newLevel)
{
    var profileUpdate = new Dictionary<string, object>
    {
        ["level"] = newLevel,
        ["last_level_up"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };
    
    await NovaSDK.Instance.UpdateUserProfile(profileUpdate);
}

public async Task UpdateUserCoins(int coinsEarned)
{
    var profileUpdate = new Dictionary<string, object>
    {
        ["coins"] = coinsEarned,
        ["last_coin_earned"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };
    
    await NovaSDK.Instance.UpdateUserProfile(profileUpdate);
}
```

### Profile Validation

```csharp
// Validate profile data before sending
public Dictionary<string, object> ValidateProfile(Dictionary<string, object> profile)
{
    var validatedProfile = new Dictionary<string, object>();
    
    foreach (var kvp in profile)
    {
        // Validate key names
        if (string.IsNullOrEmpty(kvp.Key))
        {
            Debug.LogWarning("Profile key cannot be null or empty");
            continue;
        }
        
        // Validate values
        if (kvp.Value != null)
        {
            // Ensure values are serializable
            if (IsSerializable(kvp.Value))
            {
                validatedProfile[kvp.Key] = kvp.Value;
            }
            else
            {
                Debug.LogWarning($"Profile value for '{kvp.Key}' is not serializable");
            }
        }
    }
    
    return validatedProfile;
}

private bool IsSerializable(object value)
{
    return value is string || 
           value is int || 
           value is float || 
           value is double || 
           value is bool ||
           value is string[] ||
           value is int[] ||
           value is float[];
}
```

## 🎯 User Personalization

### Profile-Based Personalization

```csharp
// Use user profile for personalization
public class PersonalizationManager : MonoBehaviour
{
    public async Task PersonalizeExperience()
    {
        // Get user profile data
        var userProfile = await GetUserProfile();
        
        // Personalize based on user level
        if (userProfile.ContainsKey("level"))
        {
            int level = Convert.ToInt32(userProfile["level"]);
            PersonalizeForLevel(level);
        }
        
        // Personalize based on country
        if (userProfile.ContainsKey("country"))
        {
            string country = userProfile["country"].ToString();
            PersonalizeForCountry(country);
        }
        
        // Personalize based on premium status
        if (userProfile.ContainsKey("premium"))
        {
            bool isPremium = Convert.ToBoolean(userProfile["premium"]);
            PersonalizeForPremium(isPremium);
        }
    }
    
    private void PersonalizeForLevel(int level)
    {
        if (level < 5)
        {
            // New player experience
            ShowTutorial();
            SetEasyDifficulty();
        }
        else if (level < 20)
        {
            // Intermediate player
            ShowTips();
            SetNormalDifficulty();
        }
        else
        {
            // Advanced player
            HideTutorial();
            SetHardDifficulty();
        }
    }
    
    private void PersonalizeForCountry(string country)
    {
        switch (country.ToUpper())
        {
            case "US":
                SetCurrency("USD");
                SetLanguage("en");
                break;
            case "JP":
                SetCurrency("JPY");
                SetLanguage("ja");
                break;
            default:
                SetCurrency("EUR");
                SetLanguage("en");
                break;
        }
    }
    
    private void PersonalizeForPremium(bool isPremium)
    {
        if (isPremium)
        {
            RemoveAds();
            EnablePremiumFeatures();
            SetPremiumUI();
        }
        else
        {
            ShowAds();
            DisablePremiumFeatures();
            SetStandardUI();
        }
    }
}
```

### Dynamic Configuration

```csharp
// Use user profile to get personalized configurations
public class DynamicConfigManager : MonoBehaviour
{
    public async Task LoadPersonalizedConfig()
    {
        var userProfile = await GetUserProfile();
        
        // Get personalized configuration based on user profile
        string configKey = GetConfigKeyForUser(userProfile);
        var personalizedConfig = await GetPersonalizedConfig(configKey);
        
        ApplyConfiguration(personalizedConfig);
    }
    
    private string GetConfigKeyForUser(Dictionary<string, object> profile)
    {
        // Determine configuration key based on user profile
        if (profile.ContainsKey("level"))
        {
            int level = Convert.ToInt32(profile["level"]);
            if (level < 10) return "beginner_config";
            if (level < 50) return "intermediate_config";
            return "advanced_config";
        }
        
        return "default_config";
    }
}
```

## 🔄 User Session Management

### Session Tracking

```csharp
public class SessionManager : MonoBehaviour
{
    private float sessionStartTime;
    private bool sessionActive = false;
    
    void Start()
    {
        StartSession();
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            EndSession();
        }
        else
        {
            StartSession();
        }
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            EndSession();
        }
        else
        {
            StartSession();
        }
    }
    
    private void StartSession()
    {
        if (!sessionActive)
        {
            sessionStartTime = Time.time;
            sessionActive = true;
            
            // Track session start
            NovaSDK.Instance.TrackEvent("session_started", new Dictionary<string, object>
            {
                ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
    
    private void EndSession()
    {
        if (sessionActive)
        {
            float sessionDuration = Time.time - sessionStartTime;
            sessionActive = false;
            
            // Track session end
            NovaSDK.Instance.TrackEvent("session_ended", new Dictionary<string, object>
            {
                ["duration"] = sessionDuration,
                ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            
            // Update user profile with session data
            UpdateSessionData(sessionDuration);
        }
    }
    
    private async void UpdateSessionData(float sessionDuration)
    {
        var profileUpdate = new Dictionary<string, object>
        {
            ["last_session_duration"] = sessionDuration,
            ["last_login"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        await NovaSDK.Instance.UpdateUserProfile(profileUpdate);
    }
}
```

## 🔐 User Privacy and Security

### Sensitive Data Handling

```csharp
// Never include sensitive data in user profiles
public class SecureUserManager : MonoBehaviour
{
    // Good: Safe profile data
    public Dictionary<string, object> CreateSafeProfile()
    {
        return new Dictionary<string, object>
        {
            ["level"] = 5,
            ["country"] = "US",
            ["premium"] = true,
            ["play_time_hours"] = 25.5f
        };
    }
    
    // Bad: Don't include sensitive data
    // public Dictionary<string, object> CreateUnsafeProfile()
    // {
    //     return new Dictionary<string, object>
    //     {
    //         ["email"] = "user@example.com",        // Don't include
    //         ["password"] = "secret123",            // Don't include
    //         ["credit_card"] = "1234-5678-9012",    // Don't include
    //         ["phone_number"] = "+1234567890"       // Don't include
    //     };
    // }
}
```

### Data Anonymization

```csharp
// Anonymize user data for privacy
public class PrivacyManager : MonoBehaviour
{
    public Dictionary<string, object> AnonymizeProfile(Dictionary<string, object> profile)
    {
        var anonymized = new Dictionary<string, object>();
        
        foreach (var kvp in profile)
        {
            switch (kvp.Key)
            {
                case "email":
                    // Hash or remove email
                    anonymized["email_hash"] = HashString(kvp.Value.ToString());
                    break;
                    
                case "device_id":
                    // Hash device ID
                    anonymized["device_hash"] = HashString(kvp.Value.ToString());
                    break;
                    
                default:
                    // Keep other data as-is
                    anonymized[kvp.Key] = kvp.Value;
                    break;
            }
        }
        
        return anonymized;
    }
    
    private string HashString(string input)
    {
        // Simple hash function (use proper hashing in production)
        return System.Security.Cryptography.SHA256.Create()
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(input))
            .ToString();
    }
}
```

## 📊 User Analytics

### User Segmentation

```csharp
// Segment users based on profile data
public class UserSegmentation : MonoBehaviour
{
    public string GetUserSegment(Dictionary<string, object> profile)
    {
        // Segment by level
        if (profile.ContainsKey("level"))
        {
            int level = Convert.ToInt32(profile["level"]);
            if (level < 5) return "newbie";
            if (level < 20) return "casual";
            if (level < 50) return "regular";
            return "hardcore";
        }
        
        // Segment by play time
        if (profile.ContainsKey("play_time_hours"))
        {
            float playTime = Convert.ToSingle(profile["play_time_hours"]);
            if (playTime < 1) return "very_casual";
            if (playTime < 10) return "casual";
            if (playTime < 50) return "regular";
            return "dedicated";
        }
        
        return "unknown";
    }
    
    public async Task TrackUserSegment()
    {
        var profile = await GetUserProfile();
        string segment = GetUserSegment(profile);
        
        NovaSDK.Instance.TrackEvent("user_segmented", new Dictionary<string, object>
        {
            ["segment"] = segment,
            ["user_id"] = NovaSDK.Instance.NovaUserId
        });
    }
}
```

## 🔗 Related Documentation

- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[Event Tracking](./event-tracking.md)** - Tracking user behavior
- **[Examples](./examples.md)** - User management examples
- **[Troubleshooting](./troubleshooting.md)** - Common user management issues 