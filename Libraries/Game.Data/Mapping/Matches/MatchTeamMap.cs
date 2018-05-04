using Game.Base.Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data.Mapping.Matches
{
    public class MatchTeamMap : GameEntityTypeConfiguration<MatchTeam>
    {
        protected override void PostInitialize(EntityTypeBuilder<MatchTeam> builder)
        {
            builder.ToTable(nameof(MatchTeam));
            builder.HasKey(p => p.Id);
        }
    }
}
