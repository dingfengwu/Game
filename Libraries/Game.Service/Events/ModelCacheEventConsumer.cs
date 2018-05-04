using Game.Base.Caching;
using Game.Base.Domain.Configuration;
using Game.Base.Domain.Localization;
using Game.Base.Domain.Matches;
using Game.Base.Events;
using Game.Services.Events;
using Game.Services.Matches;
using System.Collections.Generic;


namespace Game.Services.Events
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        IConsumer<EntityInserted<Language>>,
        IConsumer<EntityUpdated<Language>>,
        IConsumer<EntityDeleted<Language>>,

        IConsumer<EntityInserted<Match>>,
        IConsumer<EntityUpdated<Match>>
    {
        #region Fields

        readonly IStaticCacheManager _cacheManager;
        readonly IMatchService _matchService;

        #endregion

        #region Ctor

        public ModelCacheEventConsumer(IStaticCacheManager cacheManager,
            IMatchService matchService)
        {
            this._cacheManager = cacheManager;
            this._matchService = matchService;
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


        /// <summary>
        /// 所有已经启用的游戏
        /// </summary>
        public const string AVAILABLE_MATCH_GAME_LIST = "Game.Match.Game.Enabled.List";

        /// <summary>
        /// 所有已经启用的团队
        /// </summary>
        public const string AVAILABLE_MATCH_TEAM_LIST = "Game.Match.Team.Enabled.List";

        /// <summary>
        /// 所有已经可以参加竞猜的比赛
        /// </summary>
        public const string AVAILABLE_MATCH_LIST = "Game.Match.Enabled.List";

        #endregion

        #region Methods

        //languages
        public void HandleEvent(EntityInserted<Language> eventMessage)
        {
            //TODO:
            //clear all localizable models
        }
        public void HandleEvent(EntityUpdated<Language> eventMessage)
        {
            //TODO:
            //clear all localizable models
        }
        public void HandleEvent(EntityDeleted<Language> eventMessage)
        {
            //TODO:
            //clear all localizable models
        }

        public void HandleEvent(EntityInserted<Match> eventMessage)
        {
            var match = eventMessage.Entity;

            var matches = _matchService.GetAvailableMatch() ?? new List<MatchCacheModel>();
            if (match != null)
            {
                var matchCacheModel = _matchService.CreateMatchCacheModel(match);
                matches.Add(matchCacheModel);
                _cacheManager.Set(AVAILABLE_MATCH_LIST, matches, 1 * 24 * 60);
            }
        }

        public void HandleEvent(EntityUpdated<Match> eventMessage)
        {
            var match = eventMessage.Entity;

            var matches = _matchService.GetAvailableMatch() ?? new List<MatchCacheModel>();
            if (match != null)
            {
                var matchCacheModel = _matchService.CreateMatchCacheModel(match);
                var index = matches.FindIndex(p => p.MatchId == match.Id);
                matches[index] = matchCacheModel;
                _cacheManager.Set(AVAILABLE_MATCH_LIST, matches, 1 * 24 * 60);
            }
        }

        #endregion
    }
}