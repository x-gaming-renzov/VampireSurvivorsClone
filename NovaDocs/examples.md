# Examples

This guide provides comprehensive examples of how to use the Nova Unity SDK in real-world scenarios. Each example demonstrates best practices and common use cases.

## 🎮 Game Configuration Example

### Complete Game Setup

This example shows a complete game configuration setup with player settings, UI configuration, and game balance.

#### 1. Create NovaContext Components

```csharp
// PlayerConfig GameObject with NovaContext
public class PlayerConfig : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        
        // Configure the context
        context.ObjectName = "player_config";
        
        // Add properties
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "player_speed",
            Type = PropertyType.Number,
            DefaultValue = "5.0",
            Description = "Player movement speed in units per second"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "jump_height",
            Type = PropertyType.Number,
            DefaultValue = "3.0",
            Description = "Player jump height in units"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "show_tutorial",
            Type = PropertyType.Boolean,
            DefaultValue = "true",
            Description = "Show tutorial for new players"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "starting_coins",
            Type = PropertyType.Number,
            DefaultValue = "100",
            Description = "Number of coins given to new players"
        });
    }
}

// UISettings GameObject with NovaContext
public class UISettings : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        
        context.ObjectName = "ui_settings";
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "ui_scale",
            Type = PropertyType.Number,
            DefaultValue = "1.0",
            Description = "UI scale factor"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "show_ads",
            Type = PropertyType.Boolean,
            DefaultValue = "true",
            Description = "Show advertisements"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "theme_color",
            Type = PropertyType.Text,
            DefaultValue = "#4A90E2",
            Description = "UI theme color in hex format"
        });
    }
}

// GameBalance GameObject with NovaContext
public class GameBalance : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        
        context.ObjectName = "game_balance";
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "enemy_health",
            Type = PropertyType.Number,
            DefaultValue = "100",
            Description = "Base enemy health"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "coin_rewards",
            Type = PropertyType.Number,
            DefaultValue = "10",
            Description = "Coins rewarded for defeating enemies"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "difficulty_multiplier",
            Type = PropertyType.Number,
            DefaultValue = "1.0",
            Description = "Global difficulty multiplier"
        });
    }
}
```

#### 2. Create NovaExperience Asset

```csharp
// MainGameExperience asset configuration
// Experience Name: "MainGameExperience"
// Description: "Core gameplay configuration including player settings, UI, and game balance"
// GameObjects: [PlayerConfig, UISettings, GameBalance]
```

#### 3. Game Manager Implementation

```csharp
public class GameManager : MonoBehaviour
{
    [Header("Nova Configuration")]
    public NovaExperience mainGameExperience;
    
    [Header("Game References")]
    public PlayerController playerController;
    public UIManager uiManager;
    public EnemySpawner enemySpawner;
    
    private bool novaInitialized = false;
    
    void Start()
    {
        InitializeNova();
    }
    
    async void InitializeNova()
    {
        try
        {
            // Initialize Nova SDK
            NovaSDK.Instance.Initialize();
            
            // Create user
            string userId = GetOrCreateUserId();
            bool userCreated = await NovaSDK.Instance.CreateUser(userId);
            
            if (userCreated)
            {
                Debug.Log("Nova user created successfully");
                
                // Fetch experience
                await NovaSDK.Instance.FetchExperience(mainGameExperience);
                
                // Load configuration
                LoadGameConfiguration();
                
                novaInitialized = true;
            }
            else
            {
                Debug.LogError("Failed to create Nova user");
                LoadDefaultConfiguration();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Nova initialization error: {ex.Message}");
            LoadDefaultConfiguration();
        }
    }
    
    void LoadGameConfiguration()
    {
        // Load player configuration
        float playerSpeed = NovaSDK.Instance.GetValue<float>("player_config", "player_speed");
        float jumpHeight = NovaSDK.Instance.GetValue<float>("player_config", "jump_height");
        bool showTutorial = NovaSDK.Instance.GetValue<bool>("player_config", "show_tutorial");
        int startingCoins = NovaSDK.Instance.GetValue<int>("player_config", "starting_coins");
        
        // Apply to player
        if (playerController != null)
        {
            playerController.SetSpeed(playerSpeed);
            playerController.SetJumpHeight(jumpHeight);
            playerController.SetCoins(startingCoins);
        }
        
        // Load UI configuration
        float uiScale = NovaSDK.Instance.GetValue<float>("ui_settings", "ui_scale");
        bool showAds = NovaSDK.Instance.GetValue<bool>("ui_settings", "show_ads");
        string themeColor = NovaSDK.Instance.GetValue<string>("ui_settings", "theme_color");
        
        // Apply to UI
        if (uiManager != null)
        {
            uiManager.SetUIScale(uiScale);
            uiManager.SetAdsVisibility(showAds);
            uiManager.SetThemeColor(themeColor);
        }
        
        // Load game balance
        float enemyHealth = NovaSDK.Instance.GetValue<float>("game_balance", "enemy_health");
        int coinRewards = NovaSDK.Instance.GetValue<int>("game_balance", "coin_rewards");
        float difficultyMultiplier = NovaSDK.Instance.GetValue<float>("game_balance", "difficulty_multiplier");
        
        // Apply to game systems
        if (enemySpawner != null)
        {
            enemySpawner.SetEnemyHealth(enemyHealth);
            enemySpawner.SetCoinRewards(coinRewards);
            enemySpawner.SetDifficultyMultiplier(difficultyMultiplier);
        }
        
        // Show tutorial if enabled
        if (showTutorial)
        {
            ShowTutorial();
        }
        
        Debug.Log("Game configuration loaded successfully");
    }
    
    void LoadDefaultConfiguration()
    {
        Debug.Log("Loading default configuration");
        // Implement fallback configuration
    }
    
    string GetOrCreateUserId()
    {
        string userId = PlayerPrefs.GetString("nova_user_id", "");
        if (string.IsNullOrEmpty(userId))
        {
            userId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("nova_user_id", userId);
            PlayerPrefs.Save();
        }
        return userId;
    }
    
    void ShowTutorial()
    {
        // Implement tutorial logic
        Debug.Log("Showing tutorial");
    }
}
```

## 🛒 Shop System Example

### Dynamic Shop Configuration

This example demonstrates how to create a configurable shop system with dynamic pricing and inventory.

#### 1. Shop Configuration

```csharp
// ShopConfig GameObject with NovaContext
public class ShopConfig : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        context.ObjectName = "shop_config";
        
        // Item prices
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "sword_price",
            Type = PropertyType.Number,
            DefaultValue = "100",
            Description = "Price of basic sword"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "shield_price",
            Type = PropertyType.Number,
            DefaultValue = "75",
            Description = "Price of basic shield"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "potion_price",
            Type = PropertyType.Number,
            DefaultValue = "25",
            Description = "Price of health potion"
        });
        
        // Shop settings
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "discount_percentage",
            Type = PropertyType.Number,
            DefaultValue = "0",
            Description = "Global discount percentage"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "featured_item",
            Type = PropertyType.Text,
            DefaultValue = "sword",
            Description = "Featured item for promotion"
        });
        
        // Shop availability
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "shop_enabled",
            Type = PropertyType.Boolean,
            DefaultValue = "true",
            Description = "Enable shop access"
        });
    }
}

// CurrencyConfig GameObject with NovaContext
public class CurrencyConfig : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        context.ObjectName = "currency_config";
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "coin_to_gem_rate",
            Type = PropertyType.Number,
            DefaultValue = "100",
            Description = "Coins needed to buy 1 gem"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "daily_bonus_coins",
            Type = PropertyType.Number,
            DefaultValue = "50",
            Description = "Daily login bonus coins"
        });
    }
}
```

#### 2. Shop Manager Implementation

```csharp
public class ShopManager : MonoBehaviour
{
    [Header("Nova Configuration")]
    public NovaExperience shopExperience;
    
    [Header("Shop UI")]
    public ShopUI shopUI;
    public InventoryManager inventoryManager;
    
    private Dictionary<string, float> itemPrices = new Dictionary<string, float>();
    private float discountPercentage = 0f;
    private string featuredItem = "";
    private bool shopEnabled = true;
    
    void Start()
    {
        LoadShopConfiguration();
    }
    
    async void LoadShopConfiguration()
    {
        try
        {
            // Fetch shop experience
            await NovaSDK.Instance.FetchExperience(shopExperience);
            
            // Load shop configuration
            LoadShopSettings();
            
            // Update shop UI
            UpdateShopUI();
            
            Debug.Log("Shop configuration loaded successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load shop configuration: {ex.Message}");
            LoadDefaultShopConfiguration();
        }
    }
    
    void LoadShopSettings()
    {
        // Load item prices
        itemPrices["sword"] = NovaSDK.Instance.GetValue<float>("shop_config", "sword_price");
        itemPrices["shield"] = NovaSDK.Instance.GetValue<float>("shop_config", "shield_price");
        itemPrices["potion"] = NovaSDK.Instance.GetValue<float>("shop_config", "potion_price");
        
        // Load shop settings
        discountPercentage = NovaSDK.Instance.GetValue<float>("shop_config", "discount_percentage");
        featuredItem = NovaSDK.Instance.GetValue<string>("shop_config", "featured_item");
        shopEnabled = NovaSDK.Instance.GetValue<bool>("shop_config", "shop_enabled");
        
        // Load currency settings
        float coinToGemRate = NovaSDK.Instance.GetValue<float>("currency_config", "coin_to_gem_rate");
        int dailyBonusCoins = NovaSDK.Instance.GetValue<int>("currency_config", "daily_bonus_coins");
        
        // Apply currency settings
        CurrencyManager.Instance.SetCoinToGemRate(coinToGemRate);
        CurrencyManager.Instance.SetDailyBonusCoins(dailyBonusCoins);
    }
    
    void UpdateShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetShopEnabled(shopEnabled);
            shopUI.SetDiscountPercentage(discountPercentage);
            shopUI.SetFeaturedItem(featuredItem);
            shopUI.UpdatePrices(itemPrices);
        }
    }
    
    public float GetItemPrice(string itemId)
    {
        if (itemPrices.ContainsKey(itemId))
        {
            float basePrice = itemPrices[itemId];
            float discountedPrice = basePrice * (1f - discountPercentage / 100f);
            return discountedPrice;
        }
        return 0f;
    }
    
    public bool IsShopEnabled()
    {
        return shopEnabled;
    }
    
    public string GetFeaturedItem()
    {
        return featuredItem;
    }
    
    public void PurchaseItem(string itemId)
    {
        if (!shopEnabled)
        {
            Debug.LogWarning("Shop is disabled");
            return;
        }
        
        float price = GetItemPrice(itemId);
        if (CurrencyManager.Instance.HasEnoughCoins(price))
        {
            CurrencyManager.Instance.SpendCoins(price);
            inventoryManager.AddItem(itemId);
            
            // Track purchase event
            NovaSDK.Instance.TrackEvent("item_purchased", new Dictionary<string, object>
            {
                ["item_id"] = itemId,
                ["price"] = price,
                ["discount_applied"] = discountPercentage > 0
            });
            
            Debug.Log($"Purchased {itemId} for {price} coins");
        }
        else
        {
            Debug.LogWarning("Not enough coins");
        }
    }
    
    void LoadDefaultShopConfiguration()
    {
        // Set default prices
        itemPrices["sword"] = 100f;
        itemPrices["shield"] = 75f;
        itemPrices["potion"] = 25f;
        
        discountPercentage = 0f;
        featuredItem = "sword";
        shopEnabled = true;
        
        UpdateShopUI();
    }
}
```

## 🎯 A/B Testing Example

### Button Color Experiment

This example shows how to implement A/B testing for UI elements.

#### 1. Experiment Configuration

```csharp
// UIExperimentConfig GameObject with NovaContext
public class UIExperimentConfig : MonoBehaviour
{
    private NovaContext context;
    
    void Awake()
    {
        context = GetComponent<NovaContext>();
        context.ObjectName = "ui_experiment_config";
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "button_color",
            Type = PropertyType.Text,
            DefaultValue = "blue",
            Description = "Primary button color (blue, red, green)"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "button_size",
            Type = PropertyType.Number,
            DefaultValue = "1.0",
            Description = "Button size multiplier"
        });
        
        context.Properties.Add(new PropertyDefinition
        {
            PropertyName = "button_text",
            Type = PropertyType.Text,
            DefaultValue = "Play Now",
            Description = "Primary button text"
        });
    }
}
```

#### 2. Experiment Manager

```csharp
public class ExperimentManager : MonoBehaviour
{
    [Header("Nova Configuration")]
    public NovaExperience experimentExperience;
    
    [Header("UI References")]
    public Button primaryButton;
    public Text buttonText;
    
    private string buttonColor = "blue";
    private float buttonSize = 1.0f;
    private string buttonTextContent = "Play Now";
    
    void Start()
    {
        LoadExperimentConfiguration();
    }
    
    async void LoadExperimentConfiguration()
    {
        try
        {
            // Fetch experiment experience
            await NovaSDK.Instance.FetchExperience(experimentExperience);
            
            // Load experiment settings
            LoadExperimentSettings();
            
            // Apply experiment to UI
            ApplyExperimentToUI();
            
            // Track experiment exposure
            TrackExperimentExposure();
            
            Debug.Log("Experiment configuration loaded successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load experiment configuration: {ex.Message}");
            ApplyDefaultConfiguration();
        }
    }
    
    void LoadExperimentSettings()
    {
        buttonColor = NovaSDK.Instance.GetValue<string>("ui_experiment_config", "button_color");
        buttonSize = NovaSDK.Instance.GetValue<float>("ui_experiment_config", "button_size");
        buttonTextContent = NovaSDK.Instance.GetValue<string>("ui_experiment_config", "button_text");
    }
    
    void ApplyExperimentToUI()
    {
        if (primaryButton != null)
        {
            // Apply button color
            Color color = GetColorFromString(buttonColor);
            primaryButton.GetComponent<Image>().color = color;
            
            // Apply button size
            primaryButton.transform.localScale = Vector3.one * buttonSize;
        }
        
        if (buttonText != null)
        {
            buttonText.text = buttonTextContent;
        }
    }
    
    Color GetColorFromString(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red":
                return Color.red;
            case "green":
                return Color.green;
            case "blue":
            default:
                return Color.blue;
        }
    }
    
    void TrackExperimentExposure()
    {
        NovaSDK.Instance.TrackEvent("experiment_exposed", new Dictionary<string, object>
        {
            ["experiment_id"] = "button_color_test",
            ["variant"] = buttonColor,
            ["user_id"] = NovaSDK.Instance.NovaUserId
        });
    }
    
    public void OnButtonClicked()
    {
        // Track button click
        NovaSDK.Instance.TrackEvent("button_clicked", new Dictionary<string, object>
        {
            ["experiment_id"] = "button_color_test",
            ["variant"] = buttonColor,
            ["button_text"] = buttonTextContent,
            ["user_id"] = NovaSDK.Instance.NovaUserId
        });
        
        // Handle button click
        Debug.Log($"Button clicked: {buttonTextContent}");
    }
    
    void ApplyDefaultConfiguration()
    {
        buttonColor = "blue";
        buttonSize = 1.0f;
        buttonTextContent = "Play Now";
        
        ApplyExperimentToUI();
    }
}
```

## 📊 Analytics Example

### Comprehensive Event Tracking

This example demonstrates comprehensive event tracking for game analytics.

#### 1. Analytics Manager

```csharp
public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager instance;
    public static AnalyticsManager Instance => instance;
    
    private float sessionStartTime;
    private int levelAttempts = 0;
    private Dictionary<string, float> featureUsageTime = new Dictionary<string, float>();
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
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
    
    void StartSession()
    {
        sessionStartTime = Time.time;
        
        NovaSDK.Instance.TrackEvent("session_started", new Dictionary<string, object>
        {
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["platform"] = Application.platform.ToString(),
            ["app_version"] = Application.version
        });
    }
    
    void EndSession()
    {
        float sessionDuration = Time.time - sessionStartTime;
        
        NovaSDK.Instance.TrackEvent("session_ended", new Dictionary<string, object>
        {
            ["duration"] = sessionDuration,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
        
        // Flush events before ending
        NovaSDK.Instance.FlushEventQueue();
    }
    
    // Game progression events
    public void TrackLevelStarted(int level, string difficulty)
    {
        levelAttempts++;
        
        NovaSDK.Instance.TrackEvent("level_started", new Dictionary<string, object>
        {
            ["level"] = level,
            ["difficulty"] = difficulty,
            ["attempt_number"] = levelAttempts
        });
    }
    
    public void TrackLevelCompleted(int level, int score, int stars, float timeSpent)
    {
        NovaSDK.Instance.TrackEvent("level_completed", new Dictionary<string, object>
        {
            ["level"] = level,
            ["score"] = score,
            ["stars"] = stars,
            ["time_spent"] = timeSpent,
            ["attempts"] = levelAttempts
        });
        
        levelAttempts = 0; // Reset for next level
    }
    
    public void TrackLevelFailed(int level, string reason)
    {
        NovaSDK.Instance.TrackEvent("level_failed", new Dictionary<string, object>
        {
            ["level"] = level,
            ["reason"] = reason,
            ["attempts"] = levelAttempts
        });
    }
    
    // Monetization events
    public void TrackPurchaseAttempted(string itemId, float price, string currency)
    {
        NovaSDK.Instance.TrackEvent("purchase_attempted", new Dictionary<string, object>
        {
            ["item_id"] = itemId,
            ["price"] = price,
            ["currency"] = currency,
            ["platform"] = Application.platform.ToString()
        });
    }
    
    public void TrackPurchaseCompleted(string itemId, float price, string currency, string transactionId)
    {
        NovaSDK.Instance.TrackEvent("purchase_completed", new Dictionary<string, object>
        {
            ["item_id"] = itemId,
            ["price"] = price,
            ["currency"] = currency,
            ["transaction_id"] = transactionId,
            ["platform"] = Application.platform.ToString()
        });
    }
    
    // Feature usage events
    public void TrackFeatureUsageStart(string featureName)
    {
        featureUsageTime[featureName] = Time.time;
        
        NovaSDK.Instance.TrackEvent("feature_usage_started", new Dictionary<string, object>
        {
            ["feature"] = featureName,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
    
    public void TrackFeatureUsageEnd(string featureName)
    {
        if (featureUsageTime.ContainsKey(featureName))
        {
            float usageTime = Time.time - featureUsageTime[featureName];
            featureUsageTime.Remove(featureName);
            
            NovaSDK.Instance.TrackEvent("feature_usage_ended", new Dictionary<string, object>
            {
                ["feature"] = featureName,
                ["usage_time"] = usageTime,
                ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
    
    // Error tracking
    public void TrackError(string errorType, string errorMessage, string stackTrace = null)
    {
        var eventData = new Dictionary<string, object>
        {
            ["error_type"] = errorType,
            ["error_message"] = errorMessage,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        if (!string.IsNullOrEmpty(stackTrace))
        {
            eventData["stack_trace"] = stackTrace;
        }
        
        NovaSDK.Instance.TrackEvent("error_occurred", eventData);
    }
    
    // Performance tracking
    public void TrackPerformance(string metric, float value, string unit = "")
    {
        NovaSDK.Instance.TrackEvent("performance_metric", new Dictionary<string, object>
        {
            ["metric"] = metric,
            ["value"] = value,
            ["unit"] = unit,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
```

## 🔗 Related Documentation

- **[Getting Started](./getting-started.md)** - Initial setup guide
- **[Runtime API](./runtime-api.md)** - Complete API reference
- **[NovaContext Components](./novacontext.md)** - Creating configurable objects
- **[Event Tracking](./event-tracking.md)** - Detailed event tracking guide
- **[Troubleshooting](./troubleshooting.md)** - Common issues and solutions 