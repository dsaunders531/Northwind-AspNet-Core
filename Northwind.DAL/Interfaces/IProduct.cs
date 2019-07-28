using duncans.EF;

namespace Northwind.DAL
{
    public interface IProduct
    {
        string ProductName { get; set; }
        int? SupplierId { get; set; }
        int? CategoryId { get; set; }
        string QuantityPerUnit { get; set; }
        decimal? UnitPrice { get; set; }    
        short? UnitsInStock { get; set; }
        short? UnitsOnOrder { get; set; }
        short? ReorderLevel { get; set; }
        bool Discontinued { get; set; }
    }
}
