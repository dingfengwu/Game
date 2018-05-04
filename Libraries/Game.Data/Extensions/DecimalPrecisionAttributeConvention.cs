using Game.Data.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Game.Data.Extensions
{
    //attribute方式设置decimal精度
    public class DecimalPrecisionAttributeConvention : PropertyAttributeConvention<DecimalPrecisionAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, DecimalPrecisionAttribute attribute, MemberInfo clrMember)
        {
            if (propertyBuilder.Metadata.ClrType == typeof(decimal))
                propertyBuilder.HasPrecision(attribute.Precision, attribute.Scale);
            return propertyBuilder;
        }
    }

    public static class DecimalPrecisionConventionExtension
    {
        /// <summary>
        /// decimal类型设置精度
        /// </summary>
        /// <param name="propertyBuilder"></param>
        /// <param name="precision">精度</param>
        /// <param name="scale">小数位数</param>
        public static PropertyBuilder<TProperty> HasPrecision<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, int precision = 18, int scale = 4)
        {
            //fluntapi方式设置精度  
            ((IInfrastructure<InternalPropertyBuilder>)propertyBuilder).Instance.HasPrecision(precision, scale);

            return propertyBuilder;
        }

        public static InternalPropertyBuilder HasPrecision(this InternalPropertyBuilder propertyBuilder, int precision, int scale)
        {
            propertyBuilder.Relational(ConfigurationSource.Explicit).HasColumnType($"decimal({precision},{scale})");

            return propertyBuilder;
        }
    }
}
