using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Infrastructure.Data;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "The built in migrator doesn't wrap the script in a transaction...")]
public class TransactionWrappedMigrator : Migrator
{
    public TransactionWrappedMigrator(
        IMigrationsAssembly migrationsAssembly,
        IHistoryRepository historyRepository,
        IDatabaseCreator databaseCreator,
        IMigrationsSqlGenerator migrationsSqlGenerator,
        IRawSqlCommandBuilder rawSqlCommandBuilder,
        IMigrationCommandExecutor migrationCommandExecutor,
        IRelationalConnection connection,
        ISqlGenerationHelper sqlGenerationHelper,
        ICurrentDbContext currentContext,
        IModelRuntimeInitializer modelRuntimeInitializer,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
        IRelationalCommandDiagnosticsLogger commandLogger,
        IDatabaseProvider databaseProvider
    )
        : base(
                migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator, 
                rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, 
                currentContext, modelRuntimeInitializer, logger, commandLogger, databaseProvider
        )
    {
    }

    private void PrintOptions(String fromMigration = null, String toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        Console.WriteLine(
            $"From migration: {fromMigration ?? "N\\A"} {Environment.NewLine}" +
            $"To migration: {toMigration ?? "N\\A"} {Environment.NewLine}" +
            $"Options: {Environment.NewLine}" +
            $" -> Default -> {(options & MigrationsSqlGenerationOptions.Default) != 0} {Environment.NewLine}" +
            $" -> Script -> {(options & MigrationsSqlGenerationOptions.Script) != 0} {Environment.NewLine}" +
            $" -> Idempotent -> {(options & MigrationsSqlGenerationOptions.Idempotent) != 0} {Environment.NewLine}" +
            $" -> NoTransactions -> {(options & MigrationsSqlGenerationOptions.NoTransactions) != 0} {Environment.NewLine}"
        );
    }

    public override String GenerateScript(String fromMigration = null, String toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        var shouldBeIdempotentScript = (
            options & MigrationsSqlGenerationOptions.Idempotent
        ) != MigrationsSqlGenerationOptions.Default;

        var shouldBeWithTransaction = (
            options & MigrationsSqlGenerationOptions.NoTransactions
        ) == MigrationsSqlGenerationOptions.Default;

        PrintOptions(fromMigration, toMigration, options);

        // dotnet ef database update
        if(options == MigrationsSqlGenerationOptions.Default)
        {
            return base.GenerateScript(fromMigration, toMigration, options); ;
        }

        // dotnet ef migrations script --idempotent -o update.sql
        if (shouldBeIdempotentScript && shouldBeWithTransaction)
        {
            var optionsWithoutTransactions = options | MigrationsSqlGenerationOptions.NoTransactions;
            var originalScript = base.GenerateScript(fromMigration, toMigration, optionsWithoutTransactions);

            var changedScript = originalScript
                .Replace(
                    "IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL",

                    $"DECLARE @XACT_STATE SMALLINT = XACT_STATE();{Environment.NewLine}" +
                    $"BEGIN TRY{Environment.NewLine}" +
                    $"IF @XACT_STATE = 1 AND OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL"
                )
                .Replace(
                    "IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] =",

                    $"DECLARE @XACT_STATE SMALLINT = XACT_STATE();{Environment.NewLine}" +
                    $"BEGIN TRY{Environment.NewLine}" +
                    $"IF @XACT_STATE = 1 AND NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] ="
                )
                .Replace(
                    $"END;{Environment.NewLine}GO",

                    $@"
                    END
                    END TRY
                    BEGIN CATCH
                        DECLARE @error INT=ERROR_NUMBER(), @errorMessage NVARCHAR(4000)=ERROR_MESSAGE(), @line INT=ERROR_LINE()
                        --RAISERROR('Error number=%d, Line=%d, Message=%s', 20, -1, @error, @line, @errorMessage) WITH LOG
                            
                        RAISERROR('Error number=%d, Line=%d, Message=%s', 16, 1, @error, @line, @errorMessage)
                        SET NOEXEC ON
                    END CATCH
                    GO
                    ".RemoveTheFirstIndentationLevel()
                );

            return $@"
                ---------------------------------------------------------------------------------------------------------------------------------
                -- Generated using NetApiCleanTemplate ({DateTime.Now})
                ---------------------------------------------------------------------------------------------------------------------------------

                SET XACT_ABORT ON;
                BEGIN TRANSACTION;

                ---------------------------------------------------------------------------------------------------------------------------------

                {changedScript}

                ---------------------------------------------------------------------------------------------------------------------------------

                IF XACT_STATE() = 1
                BEGIN
                    COMMIT TRANSACTION;
                    PRINT 'The transaction was commited.';
                END
                ELSE
                BEGIN
                    PRINT 'Catastrophic error. The transaction can NOT be commited.';
                END
                GO

                SET NOEXEC OFF;
                GO

                PRINT 'END.';
                GO
            ".RemoveTheFirstIndentationLevel();
        }

        // unknown case
        return base.GenerateScript(fromMigration, toMigration, options);
    }
}
