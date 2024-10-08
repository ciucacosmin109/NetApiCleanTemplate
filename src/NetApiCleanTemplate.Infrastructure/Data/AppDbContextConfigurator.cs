﻿using System;
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

public class AppDbContextConfigurator
{
    public static void Configure(DbContextOptionsBuilder builder, IConfiguration appConfiguration)
    {
        var connectionString = appConfiguration.GetConnectionString("DefaultConnection");
        Configure(builder, connectionString);
    }

    public static void Configure(DbContextOptionsBuilder builder, string connectionString)
    {
        /* // Oracle - 1st method
        //Demo: ODP.NET Core application that connects to Oracle Autonomous DB
        //Enter user id and password, such as ADMIN user	
        //Enter net service name for data source value
        var config = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
        var conString = connectionString; // config.GetSection("ConnectionStrings.Default").Value;
        OracleConnection con = new OracleConnection(conString);
        OracleCommand cmd = con.CreateCommand();

        //Enter directory where the tnsnames.ora and sqlnet.ora files are located
        OracleConfiguration.TnsAdmin = config.GetSection("DatabaseConfig").GetSection("TnsAdmin").Value;

        //Alternatively, connect descriptor and net service name entries can be placed in app itself
        //To use, uncomment below and enter the DB machine port, hostname/IP, service name, and distinguished name
        //Lastly, set the Data Source value to "autonomous"
        //OracleConfiguration.OracleDataSources.Add("autonomous", "(description=(address=(protocol=tcps)(port=<PORT>)(host=<HOSTNAME/IP>))(connect_data=(service_name=<SERVICE NAME>))(security=(ssl_server_cert_dn=<DISTINGUISHED NAME>)))");                       

        //Enter directory where wallet is stored locally
        OracleConfiguration.WalletLocation = config.GetSection("DatabaseConfig").GetSection("WalletLocation").Value;

        con.Open(); 
        //Console.WriteLine("Successfully connected to Oracle Autonomous Database");

        //Retrieve database version info
        cmd.CommandText = "SELECT BANNER FROM V$VERSION";
        OracleDataReader reader = cmd.ExecuteReader();
        reader.Read();
        //Console.WriteLine("Connected to " + reader.GetString(0));

        builder.UseOracle(con);
        */

        // Oracle - 2nd method
        //builder.UseOracle(connectionString);\

        // Sql server
        builder.UseSqlite(connectionString);
        builder.ReplaceService<IMigrator, TransactionWrappedMigrator>();
    }

}
