using IRSI.Aloha.Sales.Cli.Application.Services;

namespace IRSI.Aloha.Sales.Cli.Infrastructure.Services;

public class Environment : IEnvironment
{
    public string? GetEnvironmentVariable(string variable) => System.Environment.GetEnvironmentVariable(variable);
    public string CurrentDirectory => System.Environment.CurrentDirectory;
}