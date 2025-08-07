using UnityEngine;

namespace Vampire
{
    public class NovaSetupGuide : MonoBehaviour
    {
        [Header("Setup Instructions")]
        [TextArea(10, 20)]
        public string setupInstructions = @"
🎯 NOVA SDK SETUP GUIDE FOR VAMPIRE SURVIVAL GAME

✅ COMPLETED STEPS:
1. NovaConfig.cs - Created static configuration class
2. NovaManager.cs - Created manager for Nova integration
3. MonsterSpawnTable.cs - Modified to use Nova spawn rate multiplier
4. Character.cs - Modified to use Nova health and movement speed
5. Monster.cs - Modified to use Nova health multiplier
6. NovaPrefabCreator.cs - Created utility to generate prefabs

🔄 NEXT STEPS TO COMPLETE:

STEP 1: Create NovaContext Prefabs
1. Create an empty GameObject in your scene
2. Add NovaPrefabCreator component to it
3. Right-click the component and select 'Create Nova Prefabs'
4. This will create 3 prefabs in Assets/Prefabs/Nova/:
   - GameBalanceConfig.prefab
   - PlayerProgressionConfig.prefab
   - CombatConfig.prefab

STEP 2: Create NovaExperience Asset
1. Right-click in Project window
2. Create > Nova > Experience
3. Name it 'VampireSurvivalExperience'
4. Set Experience Name to 'vampire_survival_experience'
5. Set Description to 'Core gameplay configuration'

STEP 3: Setup NovaManager in Scene
1. Create empty GameObject named 'NovaManager'
2. Add NovaManager component
3. Assign references:
   - Vampire Survival Experience: Drag your experience asset
   - Game Balance Config Prefab: Drag GameBalanceConfig prefab
   - Player Progression Config Prefab: Drag PlayerProgressionConfig prefab
   - Combat Config Prefab: Drag CombatConfig prefab

STEP 4: Push Schema to Backend
1. Go to Unity menu: Nova > Push Schema to Backend
2. Wait for completion and check console for success

STEP 5: Test Integration
1. Press Play in Unity
2. Check console for Nova initialization messages
3. Verify configuration is loaded successfully

STEP 6: Fix NovaConfig Reference Issue
The NovaConfig class needs to be accessible. Make sure:
1. NovaConfig.cs is in the Vampire namespace
2. All scripts using NovaConfig are in the same namespace
3. Compile the project to resolve references

🎉 CONGRATULATIONS!
Your vampire survival game now has real-time configuration capabilities!

TROUBLESHOOTING:
- If you get NovaConfig errors, make sure all scripts are compiled
- If prefabs don't create, check Unity Console for errors
- If Nova doesn't initialize, verify NovaSettings is configured
- If values don't change, check Nova dashboard configuration

NEXT FEATURES TO ADD:
- Event tracking for player behavior
- A/B testing for different difficulty curves
- Personalization based on player skill
- More configurable game systems
";

        [Header("Current Status")]
        public bool novaConfigCreated = true;
        public bool novaManagerCreated = true;
        public bool scriptsModified = true;
        public bool prefabsCreated = false;
        public bool experienceCreated = false;
        public bool schemaPushed = false;
        public bool integrationTested = false;

        void Start()
        {
            Debug.Log("Nova Setup Guide loaded. Check the setupInstructions field for detailed steps.");
        }

        [ContextMenu("Check Setup Status")]
        public void CheckSetupStatus()
        {
            Debug.Log("=== NOVA SETUP STATUS ===");
            Debug.Log($"NovaConfig Created: {(novaConfigCreated ? "✅" : "❌")}");
            Debug.Log($"NovaManager Created: {(novaManagerCreated ? "✅" : "❌")}");
            Debug.Log($"Scripts Modified: {(scriptsModified ? "✅" : "❌")}");
            Debug.Log($"Prefabs Created: {(prefabsCreated ? "✅" : "❌")}");
            Debug.Log($"Experience Created: {(experienceCreated ? "✅" : "❌")}");
            Debug.Log($"Schema Pushed: {(schemaPushed ? "✅" : "❌")}");
            Debug.Log($"Integration Tested: {(integrationTested ? "✅" : "❌")}");
            
            if (novaConfigCreated && novaManagerCreated && scriptsModified && prefabsCreated && experienceCreated && schemaPushed && integrationTested)
            {
                Debug.Log("🎉 NOVA INTEGRATION COMPLETE!");
            }
            else
            {
                Debug.Log("⚠️ Some steps still need to be completed. Check the setupInstructions for details.");
            }
        }
    }
} 