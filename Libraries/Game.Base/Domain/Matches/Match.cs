using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Matches
{
    public class Match:BaseEntity
    {
        public int GameId { get; set; }

        public string MatchName { get; set; }

        public DateTime MatchTimeUtc { get; set; }

        public int MasterTeamId { get; set; }

        public int SlaveTeamId { get; set; }

        public decimal MasterTeamRate { get; set; }

        public decimal SlaverTeamRate { get; set; }

        public decimal MasterTeamScore { get; set; }

        public decimal SlaverTeamScore { get; set; }

        public string LiveUrl { get; set; }

        public string Memo { get; set; }

        /// <summary>
        /// 是否已经允许竞猜
        /// </summary>
        public bool Enabled { get; set; }

        public DateTime CreateTimeUtc { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? UpdateTimeUtc { get; set; }

        public int? UpdateUserId { get; set; }

        public virtual MatchGame Game { get; set; }

        public virtual MatchTeam MasterTeam { get; set; }

        public virtual MatchTeam SlaverTeam { get; set; }
    }
}
