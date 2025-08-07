# 🧛‍♂️ NovaSDK Integration for Vampire Survival Game

## ✅ **COMPLETED CHANGES**

The following files have been created/modified to integrate NovaSDK:

### **New Files Created:**
- `Assets/Scripts/Utilities/NovaConfig.cs` - Static configuration class
- `Assets/Scripts/Utilities/NovaManager.cs` - NovaSDK manager
- `Assets/Scripts/Utilities/NovaPrefabCreator.cs` - Utility to create NovaContext prefabs
- `Assets/Scripts/Utilities/NovaSetupGuide.cs` - Setup instructions and status checker

### **Modified Files:**
- `Assets/Scripts/Monsters/MonsterSpawnTable.cs` - Added Nova spawn rate multiplier
- `Assets/Scripts/Character/Character.cs` - Added Nova health and movement speed multipliers
- `Assets/Scripts/Monsters/Monster.cs` - Added Nova health multiplier

---

## 🔄 **NEXT STEPS TO COMPLETE INTEGRATION**

### **Step 1: Create NovaContext Prefabs**

1. **In Unity, create an empty GameObject** in your scene
2. **Add the NovaPrefabCreator component** to it
3. **Right-click the NovaPrefabCreator component** and select **"Create Nova Prefabs"**
4. **Check the Console** for success messages
5. **Verify** that 3 prefabs were created in `Assets/Prefabs/Nova/`:
   - `GameBalanceConfig.prefab`
   - `PlayerProgressionConfig.prefab`
   - `CombatConfig.prefab`

### **Step 2: Create NovaExperience Asset**

1. **Right-click** in the Project window
2. Select **Create > Nova > Experience**
3. **Rename** it to `"VampireSurvivalExperience"`
4. **Select** the experience asset and configure:
   - **Experience Name**: `vampire_survival_experience`
   - **Description**: `Core gameplay configuration for vampire survival game`
   - **Leave GameObjects list empty** (handled in code)

### **Step 3: Setup NovaManager in Scene**

1. **In your game scene** (Level 1.unity), **create an empty GameObject**
2. **Name** it `"NovaManager"`
3. **Add the NovaManager component** to it
4. **Assign the references**:
   - **Vampire Survival Experience**: Drag your `VampireSurvivalExperience` asset
   - **Game Balance Config Prefab**: Drag `GameBalanceConfig` prefab
   - **Player Progression Config Prefab**: Drag `PlayerProgressionConfig` prefab
   - **Combat Config Prefab**: Drag `CombatConfig` prefab

### **Step 4: Push Schema to Backend**

1. **Go to Unity menu**: `Nova > Push Schema to Backend`
2. **Wait** for the process to complete
3. **Check Console** for success messages
4. **Verify** in your Nova dashboard that these objects appear:
   - `game_balance_config`
   - `player_progression_config`
   - `combat_config`

### **Step 5: Test the Integration**

1. **Press Play** in Unity
2. **Check Console** for these messages:
   ```
   Nova context prefabs instantiated
   Nova SDK initialized
   Nova user created successfully: [user-id]
   Nova experience fetched
   Nova configuration loaded successfully
   Spawn Rate: 1, Health: 1
   ```

3. **If you see errors**, check:
   - NovaSettings is configured with your Organization/App ID
   - All prefabs have NovaContext components
   - Object names match exactly

### **Step 6: Test Remote Configuration**

1. **Go to your Nova dashboard**
2. **Find** `game_balance_config`
3. **Change** `monster_spawn_rate_multiplier` to `2.0`
4. **Save** the changes
5. **Restart** your Unity game
6. **Check Console** for: `Spawn Rate: 2, Health: 1`
7. **Verify** monsters spawn twice as fast

---

## 🎯 **WHAT NOVA INTEGRATION ENABLES**

### **Real-time Configuration**
- **Monster Spawn Rates**: Adjust difficulty without app updates
- **Player Health**: Modify starting health for balance
- **Movement Speed**: Change player mobility
- **Monster Health**: Scale enemy toughness
- **Drop Rates**: Control reward frequency

### **A/B Testing Capabilities**
- Test different difficulty curves
- Compare reward structures
- Optimize player retention
- Measure feature effectiveness

### **Smart Game Balance**
- Adapt to player skill levels
- Personalize experience
- Optimize engagement
- Data-driven decisions

---

## 🔧 **TROUBLESHOOTING**

### **NovaConfig Reference Errors**
**Problem**: `The name 'NovaConfig' does not exist in the current context`
**Solution**: 
1. Make sure all scripts are compiled
2. Check that NovaConfig.cs is in the Vampire namespace
3. Restart Unity if needed

### **Prefab Creation Fails**
**Problem**: Prefabs don't create or show errors
**Solution**:
1. Check Unity Console for specific errors
2. Make sure NovaSDK is properly installed
3. Verify NovaSettings is configured

### **Nova Doesn't Initialize**
**Problem**: No Nova initialization messages
**Solution**:
1. Check NovaSettings configuration
2. Verify Organization ID and App ID
3. Check internet connection
4. Look for error messages in Console

### **Values Don't Change**
**Problem**: Nova configuration doesn't affect gameplay
**Solution**:
1. Verify schema was pushed successfully
2. Check Nova dashboard configuration
3. Ensure NovaManager is in the scene
4. Look for configuration loading messages

---

## 📊 **MONITORING & ANALYTICS**

### **Console Messages to Watch For**
```
✅ Nova context prefabs instantiated
✅ Nova SDK initialized  
✅ Nova user created successfully: [user-id]
✅ Nova experience fetched
✅ Nova configuration loaded successfully
✅ Spawn Rate: [value], Health: [value]
```

### **Error Messages to Watch For**
```
❌ Failed to create Nova user
❌ Nova initialization error: [details]
❌ Error loading Nova configuration: [details]
❌ Default configuration loaded
```

---

## 🚀 **NEXT FEATURES TO ADD**

### **Event Tracking**
```csharp
// Track player damage
NovaSDK.Instance.TrackEvent("player_damaged", new Dictionary<string, object>
{
    ["damage_amount"] = damage,
    ["player_health"] = currentHealth,
    ["player_level"] = currentLevel
});

// Track monster kills
NovaSDK.Instance.TrackEvent("monster_killed", new Dictionary<string, object>
{
    ["monster_type"] = monsterBlueprint.name,
    ["player_level"] = playerCharacter.CurrentLevel,
    ["game_time"] = Time.time
});
```

### **Additional NovaContext Components**
- **UI Configuration**: UI scaling, theme colors
- **Audio Configuration**: Volume levels, sound effects
- **Performance Configuration**: Graphics quality, frame rate
- **Economy Configuration**: Currency values, shop prices

### **Personalization Features**
- Adaptive difficulty based on player skill
- Personalized ability recommendations
- Custom reward structures
- Player preference tracking

---

## 🎉 **SUCCESS INDICATORS**

Your NovaSDK integration is complete when:

✅ **All prefabs created** in `Assets/Prefabs/Nova/`  
✅ **NovaExperience asset** configured  
✅ **NovaManager** in scene with all references  
✅ **Schema pushed** to backend successfully  
✅ **Console shows** successful initialization  
✅ **Remote configuration** changes take effect  
✅ **Game balance** responds to Nova values  

---

## 📞 **SUPPORT**

If you encounter issues:

1. **Check the Console** for specific error messages
2. **Verify NovaSettings** configuration
3. **Test with default values** (offline mode)
4. **Check Nova dashboard** for configuration
5. **Review this README** for troubleshooting steps

**Congratulations!** Your vampire survival game now has the power of real-time configuration and A/B testing! 🧛‍♂️✨ 