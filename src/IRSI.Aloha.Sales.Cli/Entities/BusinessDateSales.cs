namespace IRSI.Aloha.Sales.Cli.Entities;

public class BusinessDateSales
{
    public Guid Id { get; set; }
    public required string BusinessDateKey { get; set; }
    public int ConceptId { get; set; }
    public int StoreId { get; set; }
    public DateTime BusinessDate { get; set; }
    public decimal Sales { get; set; }  // TYPE 1
    public decimal GrossSales { get; set; } // TYPE 53
    public decimal NetSales { get; set; } // TYPE 52
}