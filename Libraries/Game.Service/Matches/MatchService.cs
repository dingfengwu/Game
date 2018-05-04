using Game.Base.Caching;
using Game.Base.Data;
using Game.Base.Domain.Matches;
using Game.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Game.Services.Helpers;

namespace Game.Services.Matches
{
    public class MatchService:IMatchService
    {
        IRepository<Match> _matchRepository;
        IEventPublisher _eventPublisher;
        IRepository<MatchGame> _gameRepository;
        IRepository<MatchTeam> _teamRepository;
        IStaticCacheManager _cacheManager;
        IDateTimeHelper _dateTimeHelper;

        public MatchService(IRepository<Match> matchRepository,
            IRepository<MatchGame> gameRepository,
            IRepository<MatchTeam> teamRepository,
            IEventPublisher eventPublisher,
            IStaticCacheManager cacheManager,
            IDateTimeHelper dateTimeHelper
            )
        {
            this._matchRepository = matchRepository;
            this._eventPublisher = eventPublisher;
            this._gameRepository = gameRepository;
            this._teamRepository = teamRepository;
            this._cacheManager = cacheManager;
            this._dateTimeHelper = dateTimeHelper;
        }

        /// <summary>
        /// 新增比赛
        /// </summary>
        /// <param name="model"></param>
        public void InsertMatch(Match model)
        {
            _matchRepository.Insert(model);

            _eventPublisher.EntityInserted(model);
        }

        /// <summary>
        /// 修改比赛
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMatch(Match model)
        {
            _matchRepository.Update(model);

            _eventPublisher.EntityUpdated(model);
        }

        /// <summary>
        /// 获取可见的游戏
        /// </summary>
        /// <returns></returns>
        public List<MatchGame> GetAvailableGames()
        {
            return _cacheManager.Get(ModelCacheEventConsumer.AVAILABLE_MATCH_GAME_LIST, () =>
            {
                return _gameRepository.TableNoTracking.Where(P => P.Enabled).ToList();
            });
        }

        /// <summary>
        /// 获取可见的团队
        /// </summary>
        /// <returns></returns>
        public List<MatchTeam> GetAvailableTeams()
        {
            return _cacheManager.Get(ModelCacheEventConsumer.AVAILABLE_MATCH_TEAM_LIST, () =>
            {
                return _teamRepository.TableNoTracking.Where(P => P.Enabled).ToList();
            });
        }

        /// <summary>
        /// 获取所有可用的比赛
        /// </summary>
        /// <returns></returns>
        public List<MatchCacheModel> GetAvailableMatch()
        {
            return _cacheManager.Get(ModelCacheEventConsumer.AVAILABLE_MATCH_LIST, () =>
            {
                return _matchRepository.TableNoTracking.Where(P => P.Enabled).Select(p => new MatchCacheModel
                {
                    MatchId=p.Id,
                    MatchName=p.MatchName,
                    MatchTimeLocal=_dateTimeHelper.ConvertToUserTime(p.MatchTimeUtc, DateTimeKind.Utc),
                    LiveUrl=p.LiveUrl,
                    CreateTimeLocal= _dateTimeHelper.ConvertToUserTime(p.CreateTimeUtc, DateTimeKind.Utc),
                    UpdateTimeLocal= p.UpdateTimeUtc.HasValue?_dateTimeHelper.ConvertToUserTime(p.UpdateTimeUtc.Value,DateTimeKind.Utc):new Nullable<DateTime>(),
                    Enabled=p.Enabled,
                    CreateUserId=p.CreateUserId,
                    UpdateUserId=p.UpdateUserId,
                    MatchState=p.GetMatchState(),
                    GameId=p.GameId,
                    GameIcon=p.Game.Icon,
                    MasterTeamId=p.MasterTeamId,
                    MasterTeam=p.MasterTeam.Name,
                    MasterTeamIcon=p.MasterTeam.Icon,
                    MasterTeamRate=p.MasterTeamRate,
                    MasterTeamScore=p.MasterTeamScore,
                    SlaveTeamId=p.SlaveTeamId,
                    SlaveTeam=p.SlaverTeam.Name,
                    SlaveTeamIcon=p.SlaverTeam.Icon,
                    SlaveTeamRate=p.SlaverTeamRate,
                    SlaveTeamScore=p.SlaverTeamScore,
                    MatchMemo=p.Memo
                }).ToList();
            });
        }

        /// <summary>
        /// 创建缓存模型
        /// </summary>
        /// <param name="match">比赛</param>
        /// <returns></returns>
        public MatchCacheModel CreateMatchCacheModel(Match match)
        {
            var game = GetAvailableGames()?.Where(p => p.Id == match.GameId).FirstOrDefault();
            var masterTeam = GetAvailableTeams()?.Where(p => p.Id == match.MasterTeamId).FirstOrDefault();
            var slaverTeam = GetAvailableTeams()?.Where(p => p.Id == match.SlaveTeamId).FirstOrDefault();
            return new MatchCacheModel
            {
                MatchId = match.Id,
                MatchName = match.MatchName,
                MatchState =match.GetMatchState(),
                MatchTimeLocal = _dateTimeHelper.ConvertToUserTime(match.MatchTimeUtc, DateTimeKind.Utc),
                GameIcon = game?.Icon,
                MasterTeam = masterTeam?.Name,
                MasterTeamId = masterTeam == null ? 0 : masterTeam.Id,
                MasterTeamIcon = masterTeam?.Icon,
                MasterTeamRate = match.MasterTeamRate,
                MasterTeamScore = match.MasterTeamScore,
                SlaveTeam = slaverTeam?.Name,
                SlaveTeamId = slaverTeam == null ? 0 : slaverTeam.Id,
                SlaveTeamIcon = slaverTeam?.Icon,
                SlaveTeamRate = match.SlaverTeamRate,
                SlaveTeamScore = match.SlaverTeamScore,

                MatchMemo=match.Memo,
                LiveUrl=match.LiveUrl,
                GameId=match.GameId,
                Enabled=match.Enabled,
                UpdateTimeLocal= match.UpdateTimeUtc.HasValue? _dateTimeHelper.ConvertToUserTime(match.UpdateTimeUtc.Value, DateTimeKind.Utc):new DateTime?(),
                UpdateUserId=match.UpdateUserId,
                CreateTimeLocal= _dateTimeHelper.ConvertToUserTime(match.CreateTimeUtc, DateTimeKind.Utc),
                CreateUserId=match.CreateUserId
            };
        }

        /// <summary>
        /// 获取比赛
        /// </summary>
        /// <param name="id"></param>
        public Match GetMatch(int id)
        {
            var match = GetAvailableMatch().FirstOrDefault(p => p.MatchId == id);
            if (match != null)
                return match.ToEntity();
            
            return _matchRepository.TableNoTracking.FirstOrDefault(p => p.Id == id);
        }
    }
}
