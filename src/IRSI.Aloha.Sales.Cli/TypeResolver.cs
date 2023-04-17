using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace IRSI.Aloha.Sales.Cli;

public class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IHost _host;

    public TypeResolver(IHost host)
    {
        _host = host;
    }

    public object? Resolve(Type? type)
    {
        return type != null ? _host.Services.GetService(type) : null;
    }

    public void Dispose()
    {
        _host.Dispose();
    }
}