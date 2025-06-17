using System;

namespace Plugins.AudioManager.ranged_float
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }

        public float Max { get; }

    
    }
}