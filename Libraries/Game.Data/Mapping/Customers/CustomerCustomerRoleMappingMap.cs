using Game.Base.Domain.Common;
using Game.Base.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Customers
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CustomerCustomerRoleMappingMap : GameEntityTypeConfiguration<CustomerCustomerRoleMapping>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<CustomerCustomerRoleMapping> builder)
        {
            builder.ToTable("Customer_CustomerRole_Mapping");
            builder.HasKey(t => new { t.CustomerId, t.CustomerRoleId });

            builder.HasOne(t => t.Customer).WithMany(t=>t.CustomerCustomerRoleMapping).HasForeignKey(t => t.CustomerId).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.CustomerRole).WithMany(p => p.CustomerCustomerRoleMapping).HasForeignKey(t => t.CustomerRoleId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}