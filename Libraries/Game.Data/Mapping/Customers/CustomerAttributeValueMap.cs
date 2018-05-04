using Game.Base.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Customers
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CustomerAttributeValueMap : GameEntityTypeConfiguration<CustomerAttributeValue>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<CustomerAttributeValue> builder)
        {
            builder.ToTable("CustomerAttributeValue");
            builder.HasKey(cav => cav.Id);
            builder.Property(cav => cav.Name).IsRequired().HasMaxLength(400);
        }
    }
}