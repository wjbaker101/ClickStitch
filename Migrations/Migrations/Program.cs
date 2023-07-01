using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Migrations.Migrations;

Run();

static void Run()
{
    using var serviceProvider = CreateServices();
    using var scope = serviceProvider.CreateScope();

    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

static ServiceProvider CreateServices()
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(runner => runner
            .AddPostgres()
            .WithGlobalConnectionString(ConnectionStringBuilder.Build(new ConnectionStringBuilder.ConnectionStringParameters
            {
                Host = "localhost",
                Port = 5432,
                Database = "clickstitch",
                Username = "postgres",
                Password = "password"
            }))
            .ConfigureGlobalProcessorOptions(options =>
            {
                options.ProviderSwitches = "Force Quote=false";
            })
            .ScanIn(typeof(_Migrations).Assembly)
            .For.Migrations())
        .AddLogging(x => x.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}