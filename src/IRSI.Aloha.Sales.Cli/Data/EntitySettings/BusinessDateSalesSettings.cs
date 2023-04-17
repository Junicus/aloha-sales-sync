using IRSI.Aloha.Sales.Cli.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IRSI.Aloha.Sales.Cli.Data.EntitySettings;

public class BusinessDateSalesSettings : IEntityTypeConfiguration<BusinessDateSales>
{
    public void Configure(EntityTypeBuilder<BusinessDateSales> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.BusinessDateKey);

        builder.Property(p => p.Sales).HasPrecision(10, 2);
        builder.Property(p => p.GrossSales).HasPrecision(10, 2);
        builder.Property(p => p.NetSales).HasPrecision(10, 2);
    }
}