using System.IO.Abstractions;
using IRSI.Aloha.Sales.Cli;
using IRSI.Aloha.Sales.Cli.Application.Services;
using IRSI.Aloha.Sales.Cli.Commands.Sql;
using IRSI.Aloha.Sales.Cli.Commands.Tasks;
using IRSI.Aloha.Sales.Cli.Data;
using IRSI.Aloha.Sales.Cli.Infrastructure.Services;
using IRSI.Aloha.Sales.Cli.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Spectre;
using Spectre.Console.Cli;
using Environment = IRSI.Aloha.Sales.Cli.Infrastructure.Services.Environment;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Spectre()
            .CreateBootstrapLogger();

        var builder = CreateHostBuilder(args);
        
        var registrar = new TypeRegistrar(builder);

        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.SetApplicationName("IRSI.Aloha.Sales.Cli");

            config.AddBranch("sql", sql =>
            {
                sql.SetDescription("Sync to sql");
                sql.AddCommand<SqlSalesCommand>("sales");
            });

            config.AddBranch("tasks", tasks =>
            {
                tasks.AddCommand<TasksAddSchedule>("add-schedule");
            });
        });

        return await app.RunAsync(args);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services.Configure<StoreOptions>(context.Configuration.GetRequiredSection(StoreOptions.SectionName));
            services.Configure<TaskOptions>(context.Configuration.GetRequiredSection(TaskOptions.SectionName));

            services.AddDbContext<AlohaSalesDbContext>(options =>
                options.UseSqlServer(context.Configuration.GetConnectionString("AlohaSalesConnectionString")));

            services.AddSingleton<IAlohaDatedFolderReader, AlohaDatedFolderReader>();
            services.AddSingleton<IEnvironment, Environment>();
            services.AddTransient<IFileSystem, FileSystem>();
        })
        .UseSerilog((context, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Spectre();
        });
}