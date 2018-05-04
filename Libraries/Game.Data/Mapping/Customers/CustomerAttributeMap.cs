using Game.Base.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Customers
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CustomerAttributeMap : GameEntityTypeConfiguration<CustomerAttribute>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<CustomerAttribute> builder)
        {
            builder.ToTable("CustomerAttribute");
            builder.HasKey(ca => ca.Id);
            builder.Property(ca => ca.Name).IsRequired().HasMaxLength(400);

            builder.Ignore(ca => ca.AttributeControlType);

            builder.HasMany(p => p.CustomerAttributeValues)
                .WithOne(p => p.CustomerAttribute)
                .HasForeignKey(p => p.CustomerAttributeId);
        }
    }
}