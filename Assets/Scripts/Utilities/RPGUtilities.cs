using UnityEngine;

namespace RPGSystem.Utilities
{
    /// <summary>
    /// Utility class for common RPG calculations
    /// </summary>
    public static class RPGUtilities
    {
        /// <summary>
        /// Calculate damage with armor reduction
        /// </summary>
        public static float CalculateDamageWithArmor(float baseDamage, float armor)
        {
            float reduction = armor / (armor + 100f);
            return baseDamage * (1f - reduction);
        }
        
        /// <summary>
        /// Calculate if an attack is a critical hit
        /// </summary>
        public static bool IsCriticalHit(float critChance)
        {
            return Random.value <= critChance;
        }
        
        /// <summary>
        /// Calculate experience needed for a specific level
        /// </summary>
        public static int GetExperienceForLevel(int level)
        {
            return level * 100;
        }
        
        /// <summary>
        /// Calculate total experience needed to reach a level
        /// </summary>
        public static int GetTotalExperienceForLevel(int level)
        {
            int total = 0;
            for (int i = 1; i < level; i++)
            {
                total += GetExperienceForLevel(i);
            }
            return total;
        }
        
        /// <summary>
        /// Linear interpolation with clamping
        /// </summary>
        public static float Lerp(float a, float b, float t)
        {
            t = Mathf.Clamp01(t);
            return a + (b - a) * t;
        }
        
        /// <summary>
        /// Calculate distance between two positions (2D, ignoring Y)
        /// </summary>
        public static float Distance2D(Vector3 a, Vector3 b)
        {
            Vector2 a2D = new Vector2(a.x, a.z);
            Vector2 b2D = new Vector2(b.x, b.z);
            return Vector2.Distance(a2D, b2D);
        }
        
        /// <summary>
        /// Get random position within radius
        /// </summary>
        public static Vector3 GetRandomPositionInRadius(Vector3 center, float radius)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            return center + new Vector3(randomCircle.x, 0, randomCircle.y);
        }
        
        /// <summary>
        /// Clamp value between min and max
        /// </summary>
        public static float ClampValue(float value, float min, float max)
        {
            return Mathf.Max(min, Mathf.Min(max, value));
        }
        
        /// <summary>
        /// Calculate percentage
        /// </summary>
        public static float GetPercentage(float current, float max)
        {
            if (max == 0) return 0;
            return (current / max) * 100f;
        }
        
        /// <summary>
        /// Format time in seconds to MM:SS format
        /// </summary>
        public static string FormatTime(float seconds)
        {
            int minutes = Mathf.FloorToInt(seconds / 60f);
            int secs = Mathf.FloorToInt(seconds % 60f);
            return string.Format("{0:00}:{1:00}", minutes, secs);
        }
        
        /// <summary>
        /// Check if position is within range of target
        /// </summary>
        public static bool IsInRange(Vector3 position, Vector3 target, float range)
        {
            return Vector3.Distance(position, target) <= range;
        }
    }
}
