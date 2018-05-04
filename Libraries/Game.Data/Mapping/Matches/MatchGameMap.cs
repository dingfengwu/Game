using Game.Base.Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data.Mapping.Matches
{
    public class MatchGameMap : GameEntityTypeConfiguration<MatchGame>
    {
        protected override void PostInitialize(EntityTypeBuilder<MatchGame> builder)
        {
            builder.ToTable(nameof(MatchGame));
            builder.HasKey(p => p.Id);

            builder.Property(p => p.SortIndex).HasDefaultValue(0);
        }
    }
}
