using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data.Extensions
{
    public class DecimalPrecisionAttribute: Attribute
    {
        public DecimalPrecisionAttribute()
        {

        }

        public DecimalPrecisionAttribute(int precision,int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        public int Precision { get; } = 18;

        public int Scale { get; } = 2;
    }
}
