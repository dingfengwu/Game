using Game.Base.Domain.Directory;
using Game.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Directory
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CurrencyMap : GameEntityTypeConfiguration<Currency>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency");
            builder.HasKey(c =>c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.CurrencyCode).IsRequired().HasMaxLength(5);
            builder.Property(c => c.DisplayLocale).HasMaxLength(50);
            builder.Property(c => c.CustomFormatting).HasMaxLength(50);
            builder.Property(c => c.Rate).HasPrecision(18, 4);

            builder.Ignore(c => c.RoundingType);
        }
    }
}