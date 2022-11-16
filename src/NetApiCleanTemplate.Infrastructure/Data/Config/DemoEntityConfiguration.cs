using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetApiCleanTemplate.Core.Entities.DemoEntity;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Constants;

namespace NetApiCleanTemplate.Infrastructure.Data.Config;

public class DemoEntityConfiguration : IEntityTypeConfiguration<DemoEntity>
{
    public void Configure(EntityTypeBuilder<DemoEntity> builder)
    {
        builder.Property(x => x.DemoString)
            .IsRequired()
            .HasMaxLength(DemoConstants.MaxDemoStringLength);
    }
}

