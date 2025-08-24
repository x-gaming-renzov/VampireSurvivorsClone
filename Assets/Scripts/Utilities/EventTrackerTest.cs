using UnityEngine;
using System.Collections.Generic;

namespace Vampire
{
    /// <summary>
    /// Simple test script for EventTracker functionality
    /// Add this to any GameObject to test event tracking
    /// </summary>
    public class EventTrackerTest : MonoBehaviour
    {
        [Header("Test Configuration")]
        public bool testOnStart = false;
        public float testInterval = 5f;
        
        private float lastTestTime = 0f;
        
        void Start()
        {
            if (testOnStart)
            {
                Invoke("RunBasicTests", 2f); // Wait for EventTracker to initialize
            }
        }
        
        void Update()
        {
            if (testOnStart && Time.time - lastTestTime > testInterval)
            {
                lastTestTime = Time.time;
                RunBasicTests();
            }
        }
        
        [ContextMenu("Test Basic Events")]
        public void RunBasicTests()
        {
            if (EventTracker.Instance == null)
            {
                Debug.LogWarning("EventTracker not available yet. Make sure NovaManager has initialized.");
                return;
            }
            
            Debug.Log("🧪 Running EventTracker tests...");
            
            // Test basic event tracking
            EventTracker.Instance.TrackEventSafely("test_basic_event");
            
            // Test event with data
            EventTracker.Instance.TrackEventSafely("test_event_with_data", new Dictionary<string, object>
            {
                ["test_value"] = 42,
                ["test_string"] = "Hello World",
                ["test_bool"] = true
            });
            
            // Test game events
            EventTracker.Instance.TrackLevelStarted(1, "easy");
            EventTracker.Instance.TrackEnemyDefeated("Test Goblin", 100, "Test Sword");
            EventTracker.Instance.TrackResourceCollected("Test Coins", 25, "test_source");
            
            Debug.Log("✅ EventTracker tests completed!");
        }
        
        [ContextMenu("Test Session Events")]
        public void TestSessionEvents()
        {
            if (EventTracker.Instance == null)
            {
                Debug.LogWarning("EventTracker not available yet.");
                return;
            }
            
            Debug.Log("🧪 Testing session events...");
            
            // Test feature usage tracking
            EventTracker.Instance.TrackFeatureUsageStart("Test Feature");
            
            // Simulate some time passing
            Invoke("EndTestFeature", 2f);
        }
        
        private void EndTestFeature()
        {
            if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackFeatureUsageEnd("Test Feature");
                Debug.Log("✅ Feature usage test completed!");
            }
        }
        
        [ContextMenu("Test Performance Events")]
        public void TestPerformanceEvents()
        {
            if (EventTracker.Instance == null)
            {
                Debug.LogWarning("EventTracker not available yet.");
                return;
            }
            
            Debug.Log("🧪 Testing performance events...");
            
            EventTracker.Instance.TrackPerformance("fps", 60f, "fps");
            EventTracker.Instance.TrackPerformance("memory_usage", 512f, "MB");
            EventTracker.Instance.TrackPerformance("loading_time", 2.5f, "seconds");
            
            Debug.Log("✅ Performance events test completed!");
        }
        
        [ContextMenu("Test Error Events")]
        public void TestErrorEvents()
        {
            if (EventTracker.Instance == null)
            {
                Debug.LogWarning("EventTracker not available yet.");
                return;
            }
            
            Debug.Log("🧪 Testing error events...");
            
            EventTracker.Instance.TrackError("Test Error", "This is a test error message", "Test stack trace");
            
            Debug.Log("✅ Error events test completed!");
        }
        
        [ContextMenu("Run All Tests")]
        public void RunAllTests()
        {
            RunBasicTests();
            TestSessionEvents();
            TestPerformanceEvents();
            TestErrorEvents();
            
            Debug.Log("🎉 All EventTracker tests completed!");
        }
    }
}
