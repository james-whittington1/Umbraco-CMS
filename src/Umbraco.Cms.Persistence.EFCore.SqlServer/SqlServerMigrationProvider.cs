using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Persistence.EFCore.Migrations;
using Umbraco.Extensions;

namespace Umbraco.Cms.Persistence.EFCore.SqlServer;

public class SqlServerMigrationProvider : IMigrationProvider
{
    private readonly IDbContextFactory<UmbracoDbContext> _dbContextFactory;

    public SqlServerMigrationProvider(IDbContextFactory<UmbracoDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory;

    public string ProviderName => "Microsoft.Data.SqlClient";

    public async Task MigrateAsync(EFCoreMigration migration)
    {
        UmbracoDbContext context = await _dbContextFactory.CreateDbContextAsync();
        await context.MigrateDatabaseAsync(GetMigrationType(migration));
    }

    public async Task MigrateAllAsync()
    {
        UmbracoDbContext context = await _dbContextFactory.CreateDbContextAsync();
        await context.Database.MigrateAsync();
    }

    private static Type GetMigrationType(EFCoreMigration migration) =>
        migration switch
        {
            EFCoreMigration.InitialCreate => typeof(Migrations.InitialCreate),
            _ => throw new ArgumentOutOfRangeException(nameof(migration), $@"Not expected migration value: {migration}")
        };
}
