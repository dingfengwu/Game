
using Game.Base.Data;

namespace Game.Base.Domain.Menus
{
    /// <summary>
    /// 菜单类
    /// </summary>
    public class SysMenus:BaseEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
