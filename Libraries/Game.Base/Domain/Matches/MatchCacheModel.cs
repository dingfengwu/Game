using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Matches
{
    public class MatchCacheModel
    {
        public int MatchId { get; set; }

        public int GameId { get; set; }
        
        public string GameIcon { get; set; }

        public string MatchName { get; set; }

        public DateTime MatchTimeLocal { get; set; }

        public int MasterTeamId { get; set; }

        public int SlaveTeamId { get; set; }

        public string MasterTeam { get; set; }

        public string SlaveTeam { get; set; }

        public bool Enabled { get; set; }

        public string LiveUrl { get; set; }

        public string MasterTeamIcon { get; set; }

        public string SlaveTeamIcon { get; set; }

        public decimal MasterTeamScore { get; set; }

        public decimal SlaveTeamScore { get; set; }

        public decimal MasterTeamRate { get; set; }

        public decimal SlaveTeamRate { get; set; }

        public MatchState MatchState { get; set; }

        public DateTime CreateTimeLocal { get; set; }
        
        public DateTime? UpdateTimeLocal { get; set; }

        public int CreateUserId { get; set; }

        public int? UpdateUserId { get; set; }

        public string MatchMemo { get; set; }

        public Match ToEntity()
        {
            return new Match
            {
                Id = MatchId,
                MatchName = MatchName,
                CreateTimeUtc = CreateTimeLocal.ToUniversalTime(),
                CreateUserId = CreateUserId,
                Enabled = Enabled,
                GameId = GameId,
                LiveUrl = LiveUrl,
                MasterTeamId = MasterTeamId,
                MasterTeamRate = MasterTeamRate,
                MasterTeamScore = MasterTeamScore,
                MatchTimeUtc = MatchTimeLocal.ToUniversalTime(),
                Memo = MatchMemo,
                SlaveTeamId = SlaveTeamId,
                SlaverTeamRate = SlaveTeamRate,
                SlaverTeamScore = SlaveTeamScore,
                UpdateUserId = UpdateUserId,
                UpdateTimeUtc = UpdateTimeLocal?.ToUniversalTime()
            };
        }
    }
}
