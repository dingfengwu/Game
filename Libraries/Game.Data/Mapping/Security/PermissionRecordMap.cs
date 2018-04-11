using Game.Base.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Mapping.Security
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class PermissionRecordMap : GameEntityTypeConfiguration<PermissionRecord>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.ToTable("PermissionRecord");
            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Name).IsRequired();
            builder.Property(pr => pr.SystemName).IsRequired().HasMaxLength(255);
            builder.Property(pr => pr.Category).IsRequired().HasMaxLength(255);
        }
    }
}