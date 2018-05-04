using System;
using System.Collections.Generic;
using System.Text;
using Game.Base.Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Mapping.Matches
{
    public class MatchMap : GameEntityTypeConfiguration<Match>
    {
        protected override void PostInitialize(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable(nameof(Match));
            builder.HasKey(p => p.Id);


            builder.HasOne(p => p.Game)
                .WithMany()
                .HasForeignKey(p => p.GameId);

            builder.HasOne(p => p.MasterTeam)
                .WithMany()
                .HasForeignKey(p => p.MasterTeamId);

            builder.HasOne(p => p.SlaverTeam)
                .WithMany()
                .HasForeignKey(p => p.SlaveTeamId);
        }
    }
}
