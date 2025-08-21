using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Nova.SDK;

namespace Vampire
{
    public class NovaManager : MonoBehaviour
    {
        [Header("Nova Configuration")]
        public NovaExperience vampireSurvivalExperience;
        
        [Header("NovaContext Prefabs")]
        public GameObject gameBalanceConfigPrefab;
        public GameObject playerProgressionConfigPrefab;
        public GameObject combatConfigPrefab;
        
        private bool novaInitialized = false;
        private GameObject gameBalanceInstance;
        private GameObject playerProgressionInstance;
        private GameObject combatInstance;
        
        void Start()
        {
            // Check if all required components are assigned
            if (!AreNovaComponentsValid())
            {
                Debug.LogError("❌ Required Nova components are not properly assigned in the inspector");
                LoadDefaultConfiguration();
                return;
            }
            
            InstantiateNovaContexts();
            
            // Check if contexts were instantiated successfully
            if (gameBalanceInstance == null || playerProgressionInstance == null || combatInstance == null)
            {
                Debug.LogError("❌ Nova contexts not properly instantiated, cannot initialize Nova");
                LoadDefaultConfiguration();
                return;
            }
            
            InitializeNova();
        }
        
        private void InstantiateNovaContexts()
        {
            // Check if prefabs are assigned
            if (!AreNovaComponentsValid())
            {
                Debug.LogError("❌ Cannot instantiate Nova contexts: required components are not assigned");
                return;
            }
            
            // Create parent object for organization
            GameObject novaParent = new GameObject("Nova Contexts");
            
            // Instantiate all prefabs
            gameBalanceInstance = Instantiate(gameBalanceConfigPrefab, novaParent.transform);
            playerProgressionInstance = Instantiate(playerProgressionConfigPrefab, novaParent.transform);
            combatInstance = Instantiate(combatConfigPrefab, novaParent.transform);
            
            // Verify instances were created
            if (gameBalanceInstance == null || playerProgressionInstance == null || combatInstance == null)
            {
                Debug.LogError("❌ Failed to instantiate one or more Nova context prefabs");
                return;
            }
            
            Debug.Log("Nova context prefabs instantiated successfully");
        }
        
        async void InitializeNova()
        {
            try
            {
                // Check if all required components are assigned
                if (!AreNovaComponentsValid())
                {
                    Debug.LogError("❌ Required Nova components are not properly assigned in the inspector");
                    LoadDefaultConfiguration();
                    return;
                }
                
                // Initialize Nova SDK
                NovaSDK.Instance.Initialize();
                Debug.Log("Nova SDK initialized");
                
                // Create user
                string userId = GetOrCreateUserId();

                // Create user properties
                var userProperties = new Dictionary<string, object>
                {
                    ["country"] = "US",
                    ["tier"] = "free"
                };

                bool userCreated = await NovaSDK.Instance.CreateUser(userId, userProperties);
                
                if (userCreated)
                {
                    Debug.Log("Nova user created successfully: " + userId);
                    
                    // Fetch experience
                    await NovaSDK.Instance.FetchExperience(vampireSurvivalExperience);
                    Debug.Log("Nova experience fetched successfully");
                    
                    // Wait a moment for the experience to be fully loaded
                    await System.Threading.Tasks.Task.Delay(500);
                    
                    // Wait longer for the experience to be fully loaded
                    Debug.Log($"🔍 Waiting for experience to fully load...");
                    await System.Threading.Tasks.Task.Delay(1000);
                    
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
            catch (System.Exception ex)
            {
                Debug.LogError($"Nova initialization error: {ex.Message}");
                LoadDefaultConfiguration();
            }
        }
        
        private string GetOrCreateUserId()
        {
            string userId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("NovaUserId", userId);
            PlayerPrefs.Save();
            return userId;
        }
        
        /// <summary>
        /// Safely checks if a Unity Object is null or has been destroyed
        /// </summary>
        /// <param name="obj">The Unity Object to check</param>
        /// <returns>True if the object is valid, false if null or destroyed</returns>
        private bool IsValidUnityObject(Object obj)
        {
            return obj != null && !Object.ReferenceEquals(obj, null);
        }
        
        /// <summary>
        /// Checks if all required Nova components are properly set up
        /// </summary>
        /// <returns>True if all components are valid, false otherwise</returns>
        private bool AreNovaComponentsValid()
        {
            return vampireSurvivalExperience != null &&
                   gameBalanceConfigPrefab != null &&
                   playerProgressionConfigPrefab != null &&
                   combatConfigPrefab != null;
        }
        
        private void LoadGameConfiguration()
        {
            try
            {
                Debug.Log("Loading Nova configuration values...");
                
                // Check if NovaSDK is initialized
                if (NovaSDK.Instance == null)
                {
                    Debug.LogError("❌ NovaSDK.Instance is null, cannot load configuration");
                    return;
                }
                
                if (!NovaSDK.Instance.IsInitialized)
                {
                    Debug.LogError("❌ NovaSDK is not initialized, cannot load configuration");
                    return;
                }
                
                Debug.Log($"🔍 NovaSDK Initialized: {NovaSDK.Instance.IsInitialized}");
                
                // Check if experience asset is valid
                if (vampireSurvivalExperience == null)
                {
                    Debug.LogError("❌ vampireSurvivalExperience is null, cannot load configuration");
                    return;
                }
                
                // Check if instantiated objects are valid and not destroyed
                if (!IsValidUnityObject(gameBalanceInstance) || !IsValidUnityObject(playerProgressionInstance) || !IsValidUnityObject(combatInstance))
                {
                    Debug.LogError("❌ One or more Nova context instances are null or destroyed, cannot load configuration");
                    return;
                }
                
                // Debug: Check if experience is loaded
                Debug.Log($"🔍 Experience Asset: {vampireSurvivalExperience.name}");
                
                // Debug: Check instantiated NovaContext components
                if (IsValidUnityObject(gameBalanceInstance))
                {
                    Debug.Log($"🔍 GameBalance Instance: {gameBalanceInstance.name}");
                }
                else
                {
                    Debug.LogWarning("🔍 GameBalance Instance is null or destroyed");
                }
                
                if (IsValidUnityObject(playerProgressionInstance))
                {
                    Debug.Log($"🔍 PlayerProgression Instance: {playerProgressionInstance.name}");
                }
                else
                {
                    Debug.LogWarning("🔍 PlayerProgression Instance is null or destroyed");
                }
                
                if (IsValidUnityObject(combatInstance))
                {
                    Debug.Log($"🔍 Combat Instance: {combatInstance.name}");
                }
                else
                {
                    Debug.LogWarning("🔍 Combat Instance is null or destroyed");
                }
                
                var gameBalanceContext = gameBalanceInstance?.GetComponent<Nova.SDK.NovaContext>();
                var playerProgressionContext = playerProgressionInstance?.GetComponent<Nova.SDK.NovaContext>();
                var combatContext = combatInstance?.GetComponent<Nova.SDK.NovaContext>();
                
                if (IsValidUnityObject(gameBalanceContext))
                {
                    Debug.Log($"🔍 GameBalance Context: {gameBalanceContext.ObjectName}");
                }
                else
                {
                    Debug.LogWarning("🔍 GameBalance Context is null or destroyed");
                }
                
                if (IsValidUnityObject(playerProgressionContext))
                {
                    Debug.Log($"🔍 PlayerProgression Context: {playerProgressionContext.ObjectName}");
                }
                else
                {
                    Debug.LogWarning("🔍 PlayerProgression Context is null or destroyed");
                }
                
                if (IsValidUnityObject(combatContext))
                {
                    Debug.Log($"🔍 Combat Context: {combatContext.ObjectName}");
                }
                else
                {
                    Debug.LogWarning("🔍 Combat Context is null or destroyed");
                }
                
                // Try getting values with individual try-catch blocks
                Debug.Log("Loading game balance configuration...");
                
                float spawnRate = 0;
                try
                {
                    spawnRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_spawn_rate_multiplier");
                    Debug.Log($"🔍 GetValue('game_balance_config', 'monster_spawn_rate_multiplier') = {spawnRate}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting spawn rate: {ex.Message}");
                }
                
                float healthMultiplier = 0;
                try
                {
                    healthMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_health_multiplier");
                    Debug.Log($"🔍 GetValue('game_balance_config', 'monster_health_multiplier') = {healthMultiplier}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting health multiplier: {ex.Message}");
                }
                
                float damageMultiplier = 0;
                try
                {
                    damageMultiplier = NovaSDK.Instance.GetValue<float>("game_balance_config", "monster_damage_multiplier");
                    Debug.Log($"🔍 GetValue('game_balance_config', 'monster_damage_multiplier') = {damageMultiplier}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting damage multiplier: {ex.Message}");
                }
                
                float expGemDropRate = 0;
                try
                {
                    expGemDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "exp_gem_drop_rate");
                    Debug.Log($"🔍 GetValue('game_balance_config', 'exp_gem_drop_rate') = {expGemDropRate}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting exp gem drop rate: {ex.Message}");
                }
                
                float coinDropRate = 0;
                try
                {
                    coinDropRate = NovaSDK.Instance.GetValue<float>("game_balance_config", "coin_drop_rate");
                    Debug.Log($"🔍 GetValue('game_balance_config', 'coin_drop_rate') = {coinDropRate}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting coin drop rate: {ex.Message}");
                }
                
                NovaConfig.GameBalance.SpawnRateMultiplier = spawnRate;
                NovaConfig.GameBalance.HealthMultiplier = healthMultiplier;
                NovaConfig.GameBalance.DamageMultiplier = damageMultiplier;
                NovaConfig.GameBalance.ExpGemDropRate = expGemDropRate;
                NovaConfig.GameBalance.CoinDropRate = coinDropRate;
                
                Debug.Log($"🔍 Raw Nova values - Spawn: {spawnRate}, Health: {healthMultiplier}, Damage: {damageMultiplier}, Exp: {expGemDropRate}, Coin: {coinDropRate}");
                Debug.Log("Game balance configuration loaded successfully");
                
                // Load player progression values
                Debug.Log("Loading player progression configuration...");
                
                float healthMult = 0;
                try
                {
                    healthMult = NovaSDK.Instance.GetValue<float>("player_progression_config", "starting_health_multiplier");
                    Debug.Log($"🔍 GetValue('player_progression_config', 'starting_health_multiplier') = {healthMult}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting health multiplier: {ex.Message}");
                }
                
                float movementSpeed = 0;
                try
                {
                    movementSpeed = NovaSDK.Instance.GetValue<float>("player_progression_config", "starting_movement_speed");
                    Debug.Log($"🔍 GetValue('player_progression_config', 'starting_movement_speed') = {movementSpeed}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting movement speed: {ex.Message}");
                }
                
                float expToLevel = 0;
                try
                {
                    expToLevel = NovaSDK.Instance.GetValue<float>("player_progression_config", "exp_to_level_multiplier");
                    Debug.Log($"🔍 GetValue('player_progression_config', 'exp_to_level_multiplier') = {expToLevel}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting exp to level multiplier: {ex.Message}");
                }
                
                NovaConfig.PlayerProgression.HealthMultiplier = healthMult;
                NovaConfig.PlayerProgression.MovementSpeed = movementSpeed;
                NovaConfig.PlayerProgression.ExpToLevelMultiplier = expToLevel;
                
                Debug.Log($"🔍 Raw Nova values - Health: {healthMult}, Movement: {movementSpeed}, ExpToLevel: {expToLevel}");
                Debug.Log("Player progression configuration loaded successfully");
                
                // Load combat values
                Debug.Log("Loading combat configuration...");
                
                float playerDamage = 0;
                try
                {
                    playerDamage = NovaSDK.Instance.GetValue<float>("combat_config", "player_damage_multiplier");
                    Debug.Log($"🔍 GetValue('combat_config', 'player_damage_multiplier') = {playerDamage}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting player damage multiplier: {ex.Message}");
                }
                
                float knockback = 0;
                try
                {
                    knockback = NovaSDK.Instance.GetValue<float>("combat_config", "knockback_strength");
                    Debug.Log($"🔍 GetValue('combat_config', 'knockback_strength') = {knockback}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting knockback strength: {ex.Message}");
                }
                
                float armor = 0;
                try
                {
                    armor = NovaSDK.Instance.GetValue<float>("combat_config", "armor_effectiveness");
                    Debug.Log($"🔍 GetValue('combat_config', 'armor_effectiveness') = {armor}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting armor effectiveness: {ex.Message}");
                }
                
                float healing = 0;
                try
                {
                    healing = NovaSDK.Instance.GetValue<float>("combat_config", "healing_effectiveness");
                    Debug.Log($"🔍 GetValue('combat_config', 'healing_effectiveness') = {healing}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error getting healing effectiveness: {ex.Message}");
                }
                
                NovaConfig.Combat.PlayerDamageMultiplier = playerDamage;
                NovaConfig.Combat.KnockbackStrength = knockback;
                NovaConfig.Combat.ArmorEffectiveness = armor;
                NovaConfig.Combat.HealingEffectiveness = healing;
                
                Debug.Log($"🔍 Raw Nova values - Damage: {playerDamage}, Knockback: {knockback}, Armor: {armor}, Healing: {healing}");
                Debug.Log("Combat configuration loaded successfully");
                
                Debug.Log("✅ Nova configuration loaded successfully");
                Debug.Log($"📊 Final Values - Spawn Rate: {NovaConfig.GameBalance.SpawnRateMultiplier}, Health: {NovaConfig.GameBalance.HealthMultiplier}, Movement: {NovaConfig.PlayerProgression.MovementSpeed}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Error loading Nova configuration: {ex.Message}");
                Debug.LogError($"❌ Exception details: {ex}");
                Debug.LogWarning("🔄 Falling back to default configuration...");
                LoadDefaultConfiguration();
            }
        }
        
        private void LoadDefaultConfiguration()
        {
            Debug.Log("🔄 Loading default configuration values...");
            
            NovaConfig.GameBalance.SpawnRateMultiplier = 1.0f;
            NovaConfig.GameBalance.HealthMultiplier = 1.0f;
            NovaConfig.GameBalance.DamageMultiplier = 1.0f;
            NovaConfig.GameBalance.ExpGemDropRate = 1.0f;
            NovaConfig.GameBalance.CoinDropRate = 1.0f;
            
            NovaConfig.PlayerProgression.HealthMultiplier = 1.0f;
            NovaConfig.PlayerProgression.MovementSpeed = 5.0f;
            NovaConfig.PlayerProgression.ExpToLevelMultiplier = 1.0f;
            
            NovaConfig.Combat.PlayerDamageMultiplier = 1.0f;
            NovaConfig.Combat.KnockbackStrength = 1.0f;
            NovaConfig.Combat.ArmorEffectiveness = 1.0f;
            NovaConfig.Combat.HealingEffectiveness = 1.0f;
            
            Debug.Log("✅ Default configuration loaded successfully");
            Debug.Log($"📊 Default values - Spawn Rate: {NovaConfig.GameBalance.SpawnRateMultiplier}, Health: {NovaConfig.GameBalance.HealthMultiplier}, Movement: {NovaConfig.PlayerProgression.MovementSpeed}");
        }
        
        /// <summary>
        /// Safely destroys Nova context instances
        /// </summary>
        private void CleanupNovaContexts()
        {
            if (IsValidUnityObject(gameBalanceInstance))
            {
                if (Application.isPlaying)
                {
                    Destroy(gameBalanceInstance);
                }
                else
                {
                    DestroyImmediate(gameBalanceInstance);
                }
                gameBalanceInstance = null;
            }
            
            if (IsValidUnityObject(playerProgressionInstance))
            {
                if (Application.isPlaying)
                {
                    Destroy(playerProgressionInstance);
                }
                else
                {
                    DestroyImmediate(playerProgressionInstance);
                }
                playerProgressionInstance = null;
            }
            
            if (IsValidUnityObject(combatInstance))
            {
                if (Application.isPlaying)
                {
                    Destroy(combatInstance);
                }
                else
                {
                    DestroyImmediate(combatInstance);
                }
                combatInstance = null;
            }
        }
        
        void OnDestroy()
        {
            CleanupNovaContexts();
        }
        
        void OnApplicationQuit()
        {
            CleanupNovaContexts();
        }
        
        /// <summary>
        /// Editor-only method to validate Nova setup
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void ValidateNovaSetup()
        {
            if (!AreNovaComponentsValid())
            {
                Debug.LogWarning("⚠️ Nova components validation failed. Please check the inspector assignments.");
            }
            else
            {
                Debug.Log("✅ Nova components validation passed.");
            }
        }
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            ValidateNovaSetup();
        }
        #endif
    }
} 