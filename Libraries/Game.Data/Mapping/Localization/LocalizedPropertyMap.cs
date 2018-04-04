using Game.Base.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Localization
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class LocalizedPropertyMap : GameEntityTypeConfiguration<LocalizedProperty>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<LocalizedProperty> builder)
        {
            builder.ToTable("LocalizedProperty");
            builder.HasKey(lp => lp.Id);

            builder.Property(lp => lp.LocaleKeyGroup).IsRequired().HasMaxLength(400);
            builder.Property(lp => lp.LocaleKey).IsRequired().HasMaxLength(400);
            builder.Property(lp => lp.LocaleValue).IsRequired();
            
            builder.HasOne(lp => lp.Language)
                .WithMany()
                .HasForeignKey(lp => lp.LanguageId);
        }
    }
}