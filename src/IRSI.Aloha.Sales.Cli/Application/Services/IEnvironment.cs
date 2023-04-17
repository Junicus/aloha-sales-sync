namespace IRSI.Aloha.Sales.Cli.Application.Services;

public interface IEnvironment
{
    string? GetEnvironmentVariable(string variable);
    string CurrentDirectory { get; }
}