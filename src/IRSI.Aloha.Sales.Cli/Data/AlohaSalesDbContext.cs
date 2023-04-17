using System.Reflection;
using IRSI.Aloha.Sales.Cli.Entities;
using Microsoft.EntityFrameworkCore;

namespace IRSI.Aloha.Sales.Cli.Data;

public class AlohaSalesDbContext : DbContext
{
    public DbSet<BusinessDateSales> BusinessDateSales => Set<BusinessDateSales>();

    public AlohaSalesDbContext(DbContextOptions<AlohaSalesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}