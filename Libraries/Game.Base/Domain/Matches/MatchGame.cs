using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Matches
{
    public class MatchGame:BaseEntity
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public DateTime CreateTimeUtc { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? UpdateTimeUtc { get; set; }

        public int? UpdateUserId { get; set; }

        public bool Enabled { get; set; }

        public int SortIndex { get; set; }
    }
}
