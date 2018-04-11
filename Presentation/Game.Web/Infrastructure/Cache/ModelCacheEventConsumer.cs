using Game.Base.Caching;
using Game.Base.Domain.Configuration;
using Game.Base.Domain.Localization;
using Game.Base.Events;
using Game.Services.Events;

namespace Game.Web.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        //languages
        IConsumer<EntityInserted<Language>>,
        IConsumer<EntityUpdated<Language>>,
        IConsumer<EntityDeleted<Language>>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ModelCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Cache keys
        /// <summary>
        /// Key for sitemap on the sitemap page
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string SITEMAP_PAGE_MODEL_KEY = "Game.pres.sitemap.page-{0}-{1}";
        /// <summary>
        /// Key for sitemap on the sitemap SEO page
        /// </summary>
        /// <remarks>
        /// {0} : sitemap identifier
        /// {1} : language id
        /// {2} : roles of the current user
        /// {3} : current store ID
        /// </remarks>
        public const string SITEMAP_SEO_MODEL_KEY = "Game.pres.sitemap.seo-{0}-{1}-{2}";
        public const string SITEMAP_PATTERN_KEY = "Game.pres.sitemap";

        /// <summary>
        /// Key for available languages
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        public const string AVAILABLE_LANGUAGES_MODEL_KEY = "Game.pres.languages.all";
        public const string AVAILABLE_LANGUAGES_PATTERN_KEY = "Game.pres.languages";

        /// <summary>
        /// Key for logo
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : current theme
        /// {2} : is connection SSL secured (included in a picture URL)
        /// </remarks>
        public const string STORE_LOGO_PATH = "Game.pres.logo-{0}-{1}";
        public const string STORE_LOGO_PATH_PATTERN_KEY = "Game.pres.logo";

        #endregion

        #region Methods

        //languages
        public void HandleEvent(EntityInserted<Language> eventMessage)
        {
            //clear all localizable models
        }
        public void HandleEvent(EntityUpdated<Language> eventMessage)
        {
            //clear all localizable models
        }
        public void HandleEvent(EntityDeleted<Language> eventMessage)
        {
            //clear all localizable models
        }
        
        #endregion
    }
}