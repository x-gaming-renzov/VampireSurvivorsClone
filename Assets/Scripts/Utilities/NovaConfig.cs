namespace Vampire
{
    public static class NovaConfig
    {
        public static class GameBalance
        {
            public static float SpawnRateMultiplier { get; set; } = 1.0f;
            public static float HealthMultiplier { get; set; } = 1.0f;
            public static float DamageMultiplier { get; set; } = 1.0f;
            public static float ExpGemDropRate { get; set; } = 1.0f;
            public static float CoinDropRate { get; set; } = 1.0f;
        }
        
        public static class PlayerProgression
        {
            public static float HealthMultiplier { get; set; } = 1.0f;
            public static float MovementSpeed { get; set; } = 5.0f;
            public static float ExpToLevelMultiplier { get; set; } = 1.0f;
        }
        
        public static class Combat
        {
            public static float PlayerDamageMultiplier { get; set; } = 1.0f;
            public static float KnockbackStrength { get; set; } = 1.0f;
            public static float ArmorEffectiveness { get; set; } = 1.0f;
            public static float HealingEffectiveness { get; set; } = 1.0f;
        }
    }
} 