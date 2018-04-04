using Game.Base.Domain.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Services.Menus
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public interface ISysMenusService
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        IList<SysMenus> QueryMenus();
    }
}
