using Game.Base.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Common
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class GenericAttributeMap : GameEntityTypeConfiguration<GenericAttribute>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable("GenericAttribute");
            builder.HasKey(ga => ga.Id);

            builder.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            builder.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            builder.Property(ga => ga.Value).IsRequired();
        }
    }
}