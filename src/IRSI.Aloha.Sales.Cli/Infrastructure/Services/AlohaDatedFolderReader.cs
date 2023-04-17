using System.Data;
using System.IO.Abstractions;
using System.Text;
using IRSI.Aloha.Sales.Cli.Application.Services;
using NDbfReader;

namespace IRSI.Aloha.Sales.Cli.Infrastructure.Services;

public class AlohaDatedFolderReader : IAlohaDatedFolderReader
{
    private const string IBERDIR = "IBERDIR";
    private readonly Dictionary<DateOnly, Dictionary<AlohaFiles, DataTable>> _data = new();
    private readonly IFileSystem _fileSystem;
    private readonly IEnvironment _environment;
    
    public AlohaDatedFolderReader(IFileSystem fileSystem, IEnvironment environment)
    {
        _fileSystem = fileSystem;
        _environment = environment;
    }

    public async Task<Dictionary<AlohaFiles, DataTable>> GetBusinessDateData(DateOnly businessDate)
    {
        if (_data.TryGetValue(businessDate, out var folderData)) return folderData;

        var basePath = _environment.GetEnvironmentVariable(IBERDIR) ?? "C:\\Bootdrv\\AlohaTS";
        var data = await ReadFolder(_fileSystem.Path.Combine(basePath, businessDate.ToString("yyyyMMdd")));
        _data[businessDate] = data;
        return data;
    }

    private async Task<Dictionary<AlohaFiles, DataTable>> ReadFolder(string folder)
    {
        var dataTables = new Dictionary<AlohaFiles, DataTable>
        {
            [AlohaFiles.GNDLINE] = await LoadData(folder, "gndline.dbf"),
            [AlohaFiles.GNDSALE] = await LoadData(folder, "gndsale.dbf")
        };

        return dataTables;
    }

    private async Task<DataTable> LoadData(string folder, string filename)
    {
        using var table = await Table.OpenAsync(_fileSystem.File.OpenRead(_fileSystem.Path.Combine(folder, filename)));
        return await table.AsDataTableAsync(Encoding.Default);
    }
}