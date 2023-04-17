using System.ComponentModel;
using System.Data;
using IRSI.Aloha.Sales.Cli.Application.Services;
using IRSI.Aloha.Sales.Cli.Data;
using IRSI.Aloha.Sales.Cli.Entities;
using IRSI.Aloha.Sales.Cli.Options;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace IRSI.Aloha.Sales.Cli.Commands.Sql;

public class SqlSalesCommandSettings : CommandSettings
{
    [CommandOption("--businessDate <BUSINESS_DATE>")]
    [Description("Business date to sync")]
    public string? BusinessDate { get; set; }
}

public class SqlSalesCommand : AsyncCommand<SqlSalesCommandSettings>
{
    private readonly IAnsiConsole _ansiConsole;
    private readonly StoreOptions _storeOptions;
    private readonly IAlohaDatedFolderReader _alohaDatedFolderReader;
    private readonly AlohaSalesDbContext _context;

    public SqlSalesCommand(IAnsiConsole ansiConsole, IOptions<StoreOptions> storeOptions,
        IAlohaDatedFolderReader alohaDatedFolderReader, AlohaSalesDbContext context)
    {
        _ansiConsole = ansiConsole;
        _alohaDatedFolderReader = alohaDatedFolderReader;
        _context = context;
        _storeOptions = storeOptions.Value;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SqlSalesCommandSettings settings)
    {
        var businessDate = settings.BusinessDate == null
            ? DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
            : DateOnly.ParseExact(settings.BusinessDate, "yyyyMMdd");

        await _ansiConsole.Status()
            .Spinner(Spinner.Known.Balloon)
            .StartAsync("Waiting...", async _ =>
            {
                var data = await _alohaDatedFolderReader.GetBusinessDateData(businessDate);

                _ansiConsole.MarkupLine($"[bold]Processing {businessDate}...[/]");
                _ansiConsole.MarkupLine($"[bold]Reading Sales...[/]");
                var sales = data[AlohaFiles.GNDSALE]
                    .Rows.Cast<DataRow>()
                    .Where(salesRow => Convert.ToInt32(salesRow["TYPE"]) == 1)
                    .Sum(salesRow => (decimal)salesRow["AMOUNT"]);
                _ansiConsole.MarkupLine($"[bold]Reading Net Sales...[/]");
                var netSales = data[AlohaFiles.GNDSALE]
                    .Rows.Cast<DataRow>()
                    .Where(salesRow => Convert.ToInt32(salesRow["TYPE"]) == 52)
                    .Sum(salesRow => (decimal)salesRow["AMOUNT"]);
                _ansiConsole.MarkupLine($"[bold]Reading Gross Sales...[/]");
                var grossSales = data[AlohaFiles.GNDSALE]
                    .Rows.Cast<DataRow>()
                    .Where(salesRow => Convert.ToInt32(salesRow["TYPE"]) == 53)
                    .Sum(salesRow => (decimal)salesRow["AMOUNT"]);

                _ansiConsole.MarkupLine($"[bold]Removing old data...[/]");
                var businessDateKey =
                    $"{_storeOptions.ConceptId}-{_storeOptions.StoreId}-{businessDate.ToString("yyyyMMdd")}";
                _context.BusinessDateSales.RemoveRange(
                    _context.BusinessDateSales.Where(dk => dk.BusinessDateKey == businessDateKey));
                await _context.SaveChangesAsync();

                _ansiConsole.MarkupLine($"[bold]Syncing with DB...[/]");
                var businessDateSales = new BusinessDateSales
                {
                    BusinessDateKey = businessDateKey,
                    ConceptId = _storeOptions.ConceptId,
                    StoreId = _storeOptions.StoreId,
                    BusinessDate = businessDate.ToDateTime(new()),
                    Sales = sales,
                    NetSales = netSales,
                    GrossSales = grossSales
                };

                await _context.BusinessDateSales.AddAsync(businessDateSales);
                await _context.SaveChangesAsync();
            });

        return 0;
    }
}