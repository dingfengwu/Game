using Game.Base.Domain.Seo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Seo
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class UrlRecordMap : GameEntityTypeConfiguration<UrlRecord>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<UrlRecord> builder)
        {
            builder.ToTable("UrlRecord");
            builder.HasKey(lp => lp.Id);

            builder.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            builder.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
        }
    }
}