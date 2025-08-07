# 🛠️ Nova SDK Troubleshooting Guide

## 🚨 **Current Issue: Experience Not Found (404 Errors)**

### **Error Pattern:**
```
ERROR - Error committing async database transaction: 500: 404: Experience 'game_balance_config' not found
ERROR - Error committing async database transaction: 500: 404: Experience 'player_progression_config' not found
ERROR - Error committing async database transaction: 500: 404: Experience 'combat_config' not found
```

### **Root Cause:**
The Nova backend is looking for **NovaContext objects** that haven't been created or pushed to the backend yet.

---

## ✅ **Step-by-Step Solution**

### **STEP 1: Create NovaContext Prefabs First**

You **MUST** create the NovaContext prefabs before the NovaManager can fetch their values.

#### **Quick Manual Creation:**

**1. Create GameBalanceConfig Prefab:**
```
1. GameObject → Create Empty → Name: "GameBalanceConfig"
2. Add Component → Nova Context
3. Set Object Name: "game_balance_config"
4. Add 5 properties:
   - monster_spawn_rate_multiplier (Number, 1.0)
   - monster_health_multiplier (Number, 1.0)
   - monster_damage_multiplier (Number, 1.0)
   - exp_gem_drop_rate (Number, 1.0)
   - coin_drop_rate (Number, 1.0)
5. Drag to Project → Create Prefab
6. Delete from scene
```

**2. Create PlayerProgressionConfig Prefab:**
```
1. GameObject → Create Empty → Name: "PlayerProgressionConfig"
2. Add Component → Nova Context
3. Set Object Name: "player_progression_config"
4. Add 4 properties:
   - starting_health_multiplier (Number, 1.0)
   - starting_movement_speed (Number, 5.0)
   - exp_to_level_multiplier (Number, 1.0)
   - ability_drop_rarity_weights (JSON, {"Common": 50, "Uncommon": 25, "Rare": 15, "Legendary": 9, "Exotic": 1})
5. Drag to Project → Create Prefab
6. Delete from scene
```

**3. Create CombatConfig Prefab:**
```
1. GameObject → Create Empty → Name: "CombatConfig"
2. Add Component → Nova Context
3. Set Object Name: "combat_config"
4. Add 4 properties:
   - player_damage_multiplier (Number, 1.0)
   - knockback_strength (Number, 1.0)
   - armor_effectiveness (Number, 1.0)
   - healing_effectiveness (Number, 1.0)
5. Drag to Project → Create Prefab
6. Delete from scene
```

### **STEP 2: Create NovaExperience Asset**

```
1. Right-click Project → Create → Nova → Experience
2. Name: "VampireSurvivalExperience"
3. Experience Name: "vampire_survival_experience"
4. Description: "Core gameplay configuration"
5. Leave GameObjects list empty (handled in code)
```

### **STEP 3: Push Schema to Backend**

```
1. Unity Menu → Nova → Push Schema to Backend
2. Wait for completion
3. Check Console for success messages
```

### **STEP 4: Setup NovaManager**

```
1. Create GameObject → Name: "NovaManager"
2. Add NovaManager component
3. Assign references:
   - Vampire Survival Experience: VampireSurvivalExperience asset
   - Game Balance Config Prefab: GameBalanceConfig prefab
   - Player Progression Config Prefab: PlayerProgressionConfig prefab
   - Combat Config Prefab: CombatConfig prefab
```

### **STEP 5: Test Integration**

```
1. Press Play
2. Check Console for messages:
   ✅ Nova context prefabs instantiated
   ✅ Nova SDK initialized
   ✅ Nova user created successfully
   ✅ Nova experience fetched successfully
   ✅ Loading Nova configuration values...
   ✅ Nova configuration loaded successfully
```

---

## 🔍 **Diagnostic Console Messages**

### **Expected Success Flow:**
```
Nova context prefabs instantiated
Nova SDK initialized
Nova user created successfully: [user-id] with properties: country=US, tier=free
Nova experience fetched successfully
Loading Nova configuration values...
Loading game balance configuration...
Game balance configuration loaded successfully
Loading player progression configuration...
Player progression configuration loaded successfully
Loading combat configuration...
Combat configuration loaded successfully
✅ Nova configuration loaded successfully
📊 Values - Spawn Rate: 1, Health: 1, Movement: 5
```

### **Expected Fallback Flow (if Nova fails):**
```
❌ Error loading Nova configuration: [error details]
🔄 Falling back to default configuration...
🔄 Loading default configuration values...
✅ Default configuration loaded successfully
📊 Default values - Spawn Rate: 1, Health: 1, Movement: 5
```

---

## 🚫 **Common Mistakes**

### **Mistake 1: Wrong Object Names**
❌ **Wrong:** `GameBalance` or `game-balance-config`  
✅ **Correct:** `game_balance_config`

### **Mistake 2: Missing Properties**
❌ **Wrong:** Missing required properties in NovaContext  
✅ **Correct:** All properties exactly as specified

### **Mistake 3: Schema Not Pushed**
❌ **Wrong:** Using NovaManager before pushing schema  
✅ **Correct:** Push schema first, then test

### **Mistake 4: Empty Experience**
❌ **Wrong:** NovaExperience with no GameObjects  
✅ **Correct:** Experience references prefabs in code

---

## 🔧 **Quick Debug Checklist**

### **Before Running Game:**
- [ ] GameBalanceConfig prefab exists with 5 properties
- [ ] PlayerProgressionConfig prefab exists with 4 properties  
- [ ] CombatConfig prefab exists with 4 properties
- [ ] VampireSurvivalExperience asset exists
- [ ] Schema pushed to backend successfully
- [ ] NovaManager in scene with all references assigned

### **During Game Run:**
- [ ] Check console for "Nova context prefabs instantiated"
- [ ] Check console for "Nova SDK initialized"
- [ ] Check console for "Nova user created successfully"
- [ ] Check console for "Nova experience fetched successfully"
- [ ] Check console for "Nova configuration loaded successfully"

### **If Still Getting Errors:**
1. **Verify NovaSettings** - Check Organization ID and App ID
2. **Check Network** - Ensure internet connection
3. **Restart Unity** - Sometimes helps with compilation issues
4. **Check Nova Dashboard** - Verify objects appear in backend

---

## 🎯 **Expected Final Result**

When everything works correctly, you should see:

**In Console:**
```
✅ Nova configuration loaded successfully
📊 Values - Spawn Rate: 1, Health: 1, Movement: 5
```

**In Game:**
- Monster spawning respects Nova multipliers
- Player health uses Nova multipliers
- Player movement speed uses Nova values
- All systems work with default values if Nova fails

**In Nova Dashboard:**
- `game_balance_config` object with 5 properties
- `player_progression_config` object with 4 properties
- `combat_config` object with 4 properties

---

## 📞 **Still Need Help?**

If you're still getting errors after following these steps:

1. **Copy the exact error messages** from Console
2. **Verify each step** was completed correctly
3. **Check NovaSettings** configuration
4. **Test with internet disconnected** (should use defaults)
5. **Review this troubleshooting guide** step by step

The key is that **Nova needs the objects to exist in the backend before it can fetch their values**. The prefabs and schema push are essential first steps! 🎯

