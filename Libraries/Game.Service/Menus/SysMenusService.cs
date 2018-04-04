using Game.Base.Data;
using Game.Base.Domain.Menus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game.Services.Menus
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public class SysMenusService:ISysMenusService
    {
        #region 字段

        readonly IRepository<SysMenus> _menuRep;

        #endregion

        #region 构造函数

        public SysMenusService(IRepository<SysMenus> menuRep)
        {
            _menuRep = menuRep;
        }

        #endregion
        
        #region 方法

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public IList<SysMenus> QueryMenus()
        {
            return _menuRep.TableNoTracking.ToList();
        }

        #endregion
    }
}
