using Game.Base.Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Logging
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class ActivityLogMap : GameEntityTypeConfiguration<ActivityLog>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable("ActivityLog");
            builder.HasKey(al => al.Id);
            builder.Property(al => al.Comment).IsRequired();
            builder.Property(al => al.IpAddress).HasMaxLength(200);

            builder.HasOne(al => al.ActivityLogType)
                .WithMany()
                .HasForeignKey(al => al.ActivityLogTypeId);

            builder.HasOne(al => al.Customer)
                .WithMany()
                .HasForeignKey(al => al.CustomerId);
        }
    }
}
