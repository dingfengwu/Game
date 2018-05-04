using FluentValidation.Attributes;
using Game.Facade.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Models.Matches
{
    [Validator(typeof(MatchPostModel))]
    public class MatchPostModel: BaseGameEntityModel
    {
        public int GameId { get; set; }

        public string MatchName { get; set; }

        public DateTime MatchTimeLocal { get; set; }

        public int  MasterTeamId { get; set; }

        public int SlaveTeamId { get; set; }

        public decimal MasterTeamRate { get; set; }

        public decimal SlaverTeamRate { get; set; }

        public decimal MasterTeamScore { get; set; }

        public decimal SlaverTeamScore { get; set; }

        public bool Enabled { get; set; }

        public string LiveUrl { get; set; }

        public string Memo { get; set; }
    }
}
