using UnityEngine;

namespace DashAttack.Utilities
{
    public static class FloatExtensions
    {
        private const float DefaultFloatComparisonThreshold = .0001f;

        public static bool IsCloseTo(this float f, float number, float threshold = DefaultFloatComparisonThreshold)
            => Mathf.Abs(f - number) <= DefaultFloatComparisonThreshold;

        public static bool IsCloseToZero(this float f, float threshold = DefaultFloatComparisonThreshold)
            => f.IsCloseTo(0, threshold);
    }
}
