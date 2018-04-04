using Game.Base.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Customers
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CustomerPasswordMap : GameEntityTypeConfiguration<CustomerPassword>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<CustomerPassword> builder)
        {
            builder.ToTable("CustomerPassword");
            builder.HasKey(password => password.Id);

            builder.HasOne(password => password.Customer)
                .WithMany()
                .HasForeignKey(password => password.CustomerId);

            builder.Ignore(password => password.PasswordFormat);
        }
    }
}