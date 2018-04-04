using Game.Base.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Localization
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class LocaleStringResourceMap : GameEntityTypeConfiguration<LocaleStringResource>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<LocaleStringResource> builder)
        {
            builder.ToTable("LocaleStringResource");
            builder.HasKey(lsr => lsr.Id);
            builder.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200);
            builder.Property(lsr => lsr.ResourceValue).IsRequired();

            builder.HasOne(lsr => lsr.Language)
                .WithMany()
                .HasForeignKey(lsr => lsr.LanguageId);
        }
    }
}