using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data.Extensions
{
    public class StringDefaultLengthConvention: IPropertyAddedConvention
    {
        public InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder)
        {
            if (propertyBuilder.Metadata.ClrType == typeof(string))
                propertyBuilder.HasMaxLength(50, ConfigurationSource.Convention);
            return propertyBuilder;
        }
    }
}
