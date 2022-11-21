using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace NetApiCleanTemplate.Infrastructure.Data;
public class AppIdentityDbContextConfigurator
{
    public static void Configure(DbContextOptionsBuilder builder, IConfiguration appConfiguration)
    {
        var connectionString = appConfiguration.GetConnectionString("IdentityConnection");
        Configure(builder, connectionString);
    }

    public static void Configure(DbContextOptionsBuilder builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
        builder.ReplaceService<IMigrator, TransactionWrappedMigrator>();
    }

}
