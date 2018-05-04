using Game.Base.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Data.Extensions;

namespace Game.Data.Mapping.Orders
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class OrderItemMap : GameEntityTypeConfiguration<OrderItem>
    {
        protected override void PostInitialize(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(orderItem => orderItem.Id);
            builder.Property(p => p.UnitPrice).HasPrecision(18, 4);


            builder.HasOne(orderItem => orderItem.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId);

            builder.HasOne(orderItem => orderItem.Match)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.MatchId);

            builder.HasOne(orderItem => orderItem.Team)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.TeamId);
        }
    }
}