using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nova.SDK;

namespace Vampire
{
    public class EventTracker : MonoBehaviour
    {
        private static EventTracker instance;
        public static EventTracker Instance => instance;
        
        private static bool sessionStarted = false;
        private float sessionStartTime;
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
        
        void OnApplicationQuit()
        {
            EndSession();
        }
        
        // Session Management
        private void StartSession()
        {
            if (sessionStarted)
            {
                Debug.Log("🔄 Session already started, skipping...");
                return;
            }
            
            sessionStarted = true;
            sessionStartTime = Time.time;
            TrackEventSafely("session_started", new Dictionary<string, object>
            {
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ["platform"] = Application.platform.ToString(),
                ["app_version"] = Application.version
            });
        }
        
        private void EndSession()
        {
            float sessionDuration = Time.time - sessionStartTime;
            TrackEventSafely("session_ended", new Dictionary<string, object>
            {
                ["duration"] = sessionDuration,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            
            // Note: Event batching is handled automatically by NovaSDK
            // No manual flushing needed
        }
        
        // Core Event Tracking Method
        public async void TrackEventSafely(string eventName, Dictionary<string, object> eventData = null)
        {
            try
            {
                // Check if SDK is ready
                if (!NovaSDK.Instance.IsInitialized)
                {
                    Debug.LogWarning($"Nova SDK not initialized. Event '{eventName}' not tracked.");
                    return;
                }
                
                // Check if user exists
                if (string.IsNullOrEmpty(NovaSDK.Instance.NovaUserId))
                {
                    Debug.LogWarning($"Nova user not created. Event '{eventName}' not tracked.");
                    return;
                }
                
                // Track the event
                await NovaSDK.Instance.TrackEvent(eventName, eventData);
                
                // Log successful event tracking
                Debug.Log($"✅ Event tracked: {eventName}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Failed to track event '{eventName}': {ex.Message}");
            }
        }
        
        // Game Progression Events
        public void TrackLevelStarted(int level, string difficulty = "normal")
        {
            TrackEventSafely("level_started", new Dictionary<string, object>
            {
                ["level"] = level,
                ["difficulty"] = difficulty,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        public void TrackLevelCompleted(int level, int score, float timeSpent)
        {
            TrackEventSafely("level_completed", new Dictionary<string, object>
            {
                ["level"] = level,
                ["score"] = score,
                ["time_spent"] = timeSpent,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        // Combat Events
        public void TrackEnemyDefeated(string enemyType, int damageDealt, string weaponUsed = "")
        {
            TrackEventSafely("enemy_defeated", new Dictionary<string, object>
            {
                ["enemy_type"] = enemyType,
                ["damage_dealt"] = damageDealt,
                ["weapon_used"] = weaponUsed,
                ["player_level"] = GetPlayerLevel(),
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        public void TrackPlayerDamaged(float damageAmount, float currentHealth, string damageSource = "")
        {
            TrackEventSafely("player_damaged", new Dictionary<string, object>
            {
                ["damage_amount"] = damageAmount,
                ["player_health"] = currentHealth,
                ["damage_source"] = damageSource,
                ["player_level"] = GetPlayerLevel(),
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        // Resource Collection Events
        public void TrackResourceCollected(string resourceType, int amount, string source = "")
        {
            TrackEventSafely("resource_collected", new Dictionary<string, object>
            {
                ["resource_type"] = resourceType,
                ["amount"] = amount,
                ["source"] = source,
                ["player_level"] = GetPlayerLevel(),
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        // Ability Usage Events
        public void TrackAbilityUsed(string abilityName, string abilityType, float effectiveness = 1.0f)
        {
            TrackEventSafely("ability_used", new Dictionary<string, object>
            {
                ["ability_name"] = abilityName,
                ["ability_type"] = abilityType,
                ["effectiveness"] = effectiveness,
                ["player_level"] = GetPlayerLevel(),
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        // Feature Usage Tracking
        public void TrackFeatureUsageStart(string featureName)
        {
            featureUsageTime[featureName] = Time.time;
            TrackEventSafely("feature_usage_started", new Dictionary<string, object>
            {
                ["feature"] = featureName,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        public void TrackFeatureUsageEnd(string featureName)
        {
            if (featureUsageTime.ContainsKey(featureName))
            {
                float usageTime = Time.time - featureUsageTime[featureName];
                featureUsageTime.Remove(featureName);
                
                TrackEventSafely("feature_usage_ended", new Dictionary<string, object>
                {
                    ["feature"] = featureName,
                    ["usage_time"] = usageTime,
                    ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
        }
        
        // Performance Tracking
        public void TrackPerformance(string metric, float value, string unit = "")
        {
            TrackEventSafely("performance_metric", new Dictionary<string, object>
            {
                ["metric"] = metric,
                ["value"] = value,
                ["unit"] = unit,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        
        // Error Tracking
        public void TrackError(string errorType, string errorMessage, string stackTrace = null)
        {
            var eventData = new Dictionary<string, object>
            {
                ["error_type"] = errorType,
                ["error_message"] = errorMessage,
                ["timestamp"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            
            if (!string.IsNullOrEmpty(stackTrace))
            {
                eventData["stack_trace"] = stackTrace;
            }
            
            TrackEventSafely("error_occurred", eventData);
        }
        
        // Helper method to get player level (implement based on your game)
        private int GetPlayerLevel()
        {
            // Replace with your actual player level system
            return 1; // Default value
        }
    }
}
