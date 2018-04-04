using Game.Base.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Tasks
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class ScheduleTaskMap : GameEntityTypeConfiguration<ScheduleTask>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<ScheduleTask> builder)
        {
            builder.ToTable("ScheduleTask");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Type).IsRequired();
        }
    }
}