using UnityEngine;

namespace Vampire
{
    /// <summary>
    /// Example script showing how to use EventTracker in your game systems
    /// Copy these methods to your actual game scripts
    /// </summary>
    public class EventTrackingExamples : MonoBehaviour
    {
        [Header("Example Values")]
        public int currentLevel = 1;
        public int playerScore = 0;
        public float playerHealth = 100f;
        
        // Example: Track level progression
        public void OnLevelStarted(int levelNumber)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackLevelStarted(levelNumber, "normal");
            }
        }
        
        public void OnLevelCompleted(int levelNumber, int finalScore, float timeSpent)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackLevelCompleted(levelNumber, finalScore, timeSpent);
            }
        }
        
        // Example: Track combat events
        public void OnEnemyDefeated(string enemyType, int damageDealt, string weaponUsed)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackEnemyDefeated(enemyType, damageDealt, weaponUsed);
            }
        }
        
        public void OnPlayerDamaged(float damageAmount, string damageSource)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackPlayerDamaged(damageAmount, playerHealth, damageSource);
            }
        }
        
        // Example: Track resource collection
        public void OnResourceCollected(string resourceType, int amount, string source)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackResourceCollected(resourceType, amount, source);
            }
        }
        
        // Example: Track ability usage
        public void OnAbilityUsed(string abilityName, string abilityType, float effectiveness)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackAbilityUsed(abilityName, abilityType, effectiveness);
            }
        }
        
        // Example: Track feature usage
        public void OnFeatureStarted(string featureName)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackFeatureUsageStart(featureName);
            }
        }
        
        public void OnFeatureEnded(string featureName)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackFeatureUsageEnd(featureName);
            }
        }
        
        // Example: Track performance metrics
        public void OnPerformanceMetric(string metric, float value, string unit)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackPerformance(metric, value, unit);
            }
        }
        
        // Example: Track errors
        public void OnErrorOccurred(string errorType, string errorMessage, string stackTrace = null)
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackError(errorType, errorMessage, stackTrace);
            }
        }
        
        // Example: Button click handlers for testing
        [ContextMenu("Test Level Started")]
        public void TestLevelStarted()
        {
            OnLevelStarted(currentLevel);
        }
        
        [ContextMenu("Test Level Completed")]
        public void TestLevelCompleted()
        {
            OnLevelCompleted(currentLevel, playerScore, 120.5f);
        }
        
        [ContextMenu("Test Enemy Defeated")]
        public void TestEnemyDefeated()
        {
            OnEnemyDefeated("Goblin", 150, "Sword");
        }
        
        [ContextMenu("Test Player Damaged")]
        public void TestPlayerDamaged()
        {
            OnPlayerDamaged(25f, "Goblin Attack");
        }
        
        [ContextMenu("Test Resource Collected")]
        public void TestResourceCollected()
        {
            OnResourceCollected("Coins", 50, "Enemy Drop");
        }
        
        [ContextMenu("Test Ability Used")]
        public void TestAbilityUsed()
        {
            OnAbilityUsed("Fireball", "Projectile", 0.8f);
        }
    }
}
