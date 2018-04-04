using System;
using Game.Base.Caching;
using Game.Base.Domain.Menus;
using Game.Base.Events;
using Game.Services.Events;

namespace Game.Web.Events
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer:
        IConsumer<EntityUpdated<SysMenus>>,
        IConsumer<EntityDeleted<SysMenus>>,
        IConsumer<EntityInserted<SysMenus>>
    {
        #region 字段

        public const string MERCHANT_API_CONFIGURATION = "Game.Menus-{0}";
        private readonly ICacheManager _cacheManager;

        #endregion

        #region 构造函数

        public ModelCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region 事件处理

        public void HandleEvent(EntityUpdated<SysMenus> eventMessage)
        {
            if (eventMessage.Entity == null)
            {
                _cacheManager.RemoveByPattern(MERCHANT_API_CONFIGURATION.Replace("{0}", ""));
            }
            else
            {
                _cacheManager.RemoveByPattern(string.Format(MERCHANT_API_CONFIGURATION, eventMessage.Entity.Id));
            }
        }

        public void HandleEvent(EntityDeleted<SysMenus> eventMessage)
        {
            if (eventMessage.Entity == null)
            {
                _cacheManager.RemoveByPattern(MERCHANT_API_CONFIGURATION.Replace("{0}", ""));
            }
            else
            {
                _cacheManager.RemoveByPattern(string.Format(MERCHANT_API_CONFIGURATION, eventMessage.Entity.Id));
            }
        }

        public void HandleEvent(EntityInserted<SysMenus> eventMessage)
        {
            
        }
        #endregion
    }
}
