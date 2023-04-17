using System.Data;

namespace IRSI.Aloha.Sales.Cli.Application.Services;

public interface IAlohaDatedFolderReader
{
    Task<Dictionary<AlohaFiles, DataTable>> GetBusinessDateData(DateOnly businessDate);
}

public enum AlohaFiles
{
    GNDLINE,
    GNDSALE
}