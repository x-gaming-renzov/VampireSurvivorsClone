using UnityEngine;

namespace Vampire
{
    public class NovaPrefabHelper : MonoBehaviour
    {
        [Header("Manual Prefab Creation Guide")]
        [TextArea(15, 25)]
        public string prefabCreationGuide = @"
🎯 MANUAL NOVA PREFAB CREATION GUIDE

Since the automatic prefab creator was deleted, you need to create the NovaContext prefabs manually:

STEP 1: Create GameBalanceConfig Prefab
1. Create Empty GameObject in scene
2. Name it 'GameBalanceConfig'
3. Add Component > Nova Context
4. Set Object Name to: 'game_balance_config'
5. Add these 5 properties:

Property 1:
- Property Name: monster_spawn_rate_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for monster spawn rates

Property 2:
- Property Name: monster_health_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for monster health

Property 3:
- Property Name: monster_damage_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for monster damage

Property 4:
- Property Name: exp_gem_drop_rate
- Type: Number
- Default Value: 1.0
- Description: Multiplier for exp gem drops

Property 5:
- Property Name: coin_drop_rate
- Type: Number
- Default Value: 1.0
- Description: Multiplier for coin drops

6. Drag to Project window to create prefab
7. Delete from scene

STEP 2: Create PlayerProgressionConfig Prefab
1. Create Empty GameObject in scene
2. Name it 'PlayerProgressionConfig'
3. Add Component > Nova Context
4. Set Object Name to: 'player_progression_config'
5. Add these 4 properties:

Property 1:
- Property Name: starting_health_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for starting player health

Property 2:
- Property Name: starting_movement_speed
- Type: Number
- Default Value: 5.0
- Description: Base player movement speed

Property 3:
- Property Name: exp_to_level_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for exp required to level up

Property 4:
- Property Name: ability_drop_rarity_weights
- Type: JSON
- Default Value: {""Common"": 50, ""Uncommon"": 25, ""Rare"": 15, ""Legendary"": 9, ""Exotic"": 1}
- Description: JSON object for ability rarity weights

6. Drag to Project window to create prefab
7. Delete from scene

STEP 3: Create CombatConfig Prefab
1. Create Empty GameObject in scene
2. Name it 'CombatConfig'
3. Add Component > Nova Context
4. Set Object Name to: 'combat_config'
5. Add these 4 properties:

Property 1:
- Property Name: player_damage_multiplier
- Type: Number
- Default Value: 1.0
- Description: Multiplier for player damage

Property 2:
- Property Name: knockback_strength
- Type: Number
- Default Value: 1.0
- Description: Multiplier for knockback force

Property 3:
- Property Name: armor_effectiveness
- Type: Number
- Default Value: 1.0
- Description: Multiplier for armor damage reduction

Property 4:
- Property Name: healing_effectiveness
- Type: Number
- Default Value: 1.0
- Description: Multiplier for healing amount

6. Drag to Project window to create prefab
7. Delete from scene

STEP 4: Create NovaExperience Asset
1. Right-click in Project window
2. Create > Nova > Experience
3. Name it 'VampireSurvivalExperience'
4. Set Experience Name to: 'vampire_survival_experience'
5. Set Description to: 'Core gameplay configuration'

STEP 5: Push Schema
1. Go to Nova > Push Schema to Backend
2. Wait for completion
3. Check console for success

STEP 6: Setup NovaManager
1. Create empty GameObject named 'NovaManager'
2. Add NovaManager component
3. Assign all prefab references
4. Test the integration

⚠️ IMPORTANT: Make sure to push the schema AFTER creating the prefabs!
";

        [Header("Current Status")]
        public bool gameBalancePrefabCreated = false;
        public bool playerProgressionPrefabCreated = false;
        public bool combatPrefabCreated = false;
        public bool experienceCreated = false;
        public bool schemaPushed = false;

        void Start()
        {
            Debug.Log("Nova Prefab Helper loaded. Check the prefabCreationGuide field for detailed instructions.");
        }

        [ContextMenu("Check Prefab Status")]
        public void CheckPrefabStatus()
        {
            Debug.Log("=== NOVA PREFAB STATUS ===");
            Debug.Log($"GameBalance Prefab: {(gameBalancePrefabCreated ? "✅" : "❌")}");
            Debug.Log($"PlayerProgression Prefab: {(playerProgressionPrefabCreated ? "✅" : "❌")}");
            Debug.Log($"Combat Prefab: {(combatPrefabCreated ? "✅" : "❌")}");
            Debug.Log($"Experience Created: {(experienceCreated ? "✅" : "❌")}");
            Debug.Log($"Schema Pushed: {(schemaPushed ? "✅" : "❌")}");
            
            if (gameBalancePrefabCreated && playerProgressionPrefabCreated && combatPrefabCreated && experienceCreated && schemaPushed)
            {
                Debug.Log("🎉 All Nova prefabs and schema are ready!");
            }
            else
            {
                Debug.Log("⚠️ Some steps still need to be completed. Follow the prefabCreationGuide.");
            }
        }
    }
} 