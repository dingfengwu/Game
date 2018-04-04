using Game.Base.Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Logging
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class ActivityLogTypeMap : GameEntityTypeConfiguration<ActivityLogType>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<ActivityLogType> builder)
        {
            builder.ToTable("ActivityLogType");
            builder.HasKey(alt => alt.Id);

            builder.Property(alt => alt.SystemKeyword).IsRequired().HasMaxLength(100);
            builder.Property(alt => alt.Name).IsRequired().HasMaxLength(200);
        }
    }
}
