using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Base.Domain.Matches;

namespace Game.Web.Models.Matches
{
    public class MatchListItem
    {
        public int MatchId { get; set; }

        public string GameIcon { get; set; }

        public string MatchName { get; set; }

        public DateTime MatchTimeUtc { get; set; }

        public int MasterTeamId { get; set; }

        public int SlaveTeamId { get; set; }

        public string MasterTeam { get; set; }

        public string SlaveTeam { get; set; }

        public string MasterTeamIcon { get; set; }

        public string SlaveTeamIcon { get; set; }

        public int MasterTeamScore { get; set; }

        public int SlaveTeamScore { get; set; }

        public decimal MasterTeamRate { get; set; }

        public decimal SlaveTeamRate { get; set; }

        public MatchState MatchState { get; set; }
    }
}
