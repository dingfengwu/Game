using Game.Base.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Localization
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class LanguageMap : GameEntityTypeConfiguration<Language>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Language");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
            builder.Property(l => l.LanguageCulture).IsRequired().HasMaxLength(20);
            builder.Property(l => l.UniqueSeoCode).HasMaxLength(2);
            builder.Property(l => l.FlagImageFileName).HasMaxLength(50);
        }
    }
}