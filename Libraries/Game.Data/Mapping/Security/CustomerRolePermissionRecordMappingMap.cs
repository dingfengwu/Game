using Domain.Domain.Security;
using Game.Base.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Mapping.Security
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CustomerRolePermissionRecordMappingMap : GameEntityTypeConfiguration<CustomerRolePermissionRecordMapping>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CustomerRolePermissionRecordMapping> builder)
        {
            builder.ToTable("CustomerRole_PermissionRecord_Mapping");
            builder.HasKey(pr => new { pr.CustomerRoleId, pr.PermissionRecordId });

            builder.HasOne(t => t.CustomerRole).WithMany(t=>t.CustomerRolePermissionRecordMapping).HasForeignKey(t => t.CustomerRoleId).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.PermissionRecord).WithMany(t=>t.CustomerRolePermissionRecordMapping).HasForeignKey(t => t.PermissionRecordId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}