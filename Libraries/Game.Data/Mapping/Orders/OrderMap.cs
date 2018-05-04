using Game.Base.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Data.Extensions;

namespace Game.Data.Mapping.Orders
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class OrderMap : GameEntityTypeConfiguration<Order>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected override void PostInitialize(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.CurrencyRate).HasPrecision(18, 8);
            builder.Property(o => o.OrderTotal).HasPrecision(18, 4);
            builder.Property(o => o.CustomOrderNumber).IsRequired();

            builder.Ignore(o => o.OrderStatus);
            builder.Ignore(o => o.PaymentStatus);
            
            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId);
        }
    }
}