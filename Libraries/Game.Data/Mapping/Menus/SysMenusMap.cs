using Game.Base.Domain.Menus;
using Game.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Modules.Menus
{
    public class SysMenusMap:GameEntityTypeConfiguration<SysMenus>
    {
        protected override void PostInitialize(EntityTypeBuilder<SysMenus> builder)
        {
            builder.ToTable("SysMenus").HasKey(p => p.Id);
            builder.Property(p => p.MenuName).IsRequired();
        }
    }
}
