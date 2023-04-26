using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
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
            .WithGlobalConnectionString("Server=localhost;Port=5432;Database=cross_stitch_viewer;User Id=postgres;Password=password;")
            .ConfigureGlobalProcessorOptions(options =>
            {
                options.ProviderSwitches = "Force Quote=false";
            })
            .ScanIn(typeof(_Migrations).Assembly)
            .For.Migrations())
        .AddLogging(x => x.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}