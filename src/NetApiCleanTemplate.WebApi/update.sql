---------------------------------------------------------------------------------------------------------------------------------
-- Generated using NetApiCleanTemplate (11/21/2022 6:50:32 PM)
---------------------------------------------------------------------------------------------------------------------------------

SET XACT_ABORT ON;
BEGIN TRANSACTION;

---------------------------------------------------------------------------------------------------------------------------------

DECLARE @XACT_STATE SMALLINT = XACT_STATE();
BEGIN TRY
IF @XACT_STATE = 1 AND OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
END TRY
BEGIN CATCH
    DECLARE @error INT=ERROR_NUMBER(), @errorMessage NVARCHAR(4000)=ERROR_MESSAGE(), @line INT=ERROR_LINE()
    --RAISERROR('Error number=%d, Line=%d, Message=%s', 20, -1, @error, @line, @errorMessage) WITH LOG
        
    RAISERROR('Error number=%d, Line=%d, Message=%s', 16, 1, @error, @line, @errorMessage)
    SET NOEXEC ON
END CATCH
GO


DECLARE @XACT_STATE SMALLINT = XACT_STATE();
BEGIN TRY
IF @XACT_STATE = 1 AND NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220731175022_InitialMigration')
BEGIN
    CREATE TABLE [DemoEntities] (
        [Id] int NOT NULL IDENTITY,
        [DemoString] nvarchar(128) NOT NULL,
        [DemoParentId] int NULL,
        CONSTRAINT [PK_DemoEntities] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DemoEntities_DemoEntities_DemoParentId] FOREIGN KEY ([DemoParentId]) REFERENCES [DemoEntities] ([Id])
    );
END
END TRY
BEGIN CATCH
    DECLARE @error INT=ERROR_NUMBER(), @errorMessage NVARCHAR(4000)=ERROR_MESSAGE(), @line INT=ERROR_LINE()
    --RAISERROR('Error number=%d, Line=%d, Message=%s', 20, -1, @error, @line, @errorMessage) WITH LOG
        
    RAISERROR('Error number=%d, Line=%d, Message=%s', 16, 1, @error, @line, @errorMessage)
    SET NOEXEC ON
END CATCH
GO


DECLARE @XACT_STATE SMALLINT = XACT_STATE();
BEGIN TRY
IF @XACT_STATE = 1 AND NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220731175022_InitialMigration')
BEGIN
    CREATE INDEX [IX_DemoEntities_DemoParentId] ON [DemoEntities] ([DemoParentId]);
END
END TRY
BEGIN CATCH
    DECLARE @error INT=ERROR_NUMBER(), @errorMessage NVARCHAR(4000)=ERROR_MESSAGE(), @line INT=ERROR_LINE()
    --RAISERROR('Error number=%d, Line=%d, Message=%s', 20, -1, @error, @line, @errorMessage) WITH LOG
        
    RAISERROR('Error number=%d, Line=%d, Message=%s', 16, 1, @error, @line, @errorMessage)
    SET NOEXEC ON
END CATCH
GO


DECLARE @XACT_STATE SMALLINT = XACT_STATE();
BEGIN TRY
IF @XACT_STATE = 1 AND NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220731175022_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220731175022_InitialMigration', N'6.0.7');
END
END TRY
BEGIN CATCH
    DECLARE @error INT=ERROR_NUMBER(), @errorMessage NVARCHAR(4000)=ERROR_MESSAGE(), @line INT=ERROR_LINE()
    --RAISERROR('Error number=%d, Line=%d, Message=%s', 20, -1, @error, @line, @errorMessage) WITH LOG
        
    RAISERROR('Error number=%d, Line=%d, Message=%s', 16, 1, @error, @line, @errorMessage)
    SET NOEXEC ON
END CATCH
GO


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
            