using duncans.EF;
using Northwind.DAL;

namespace Northwind.BLL.Models
{
    public class ProductApiModel : IProduct, IApiModel<int>
    {
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public int RowId { get; set; }
    }
}
