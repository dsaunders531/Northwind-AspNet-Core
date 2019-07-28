using duncans.EF;
using Northwind.DAL;

namespace Northwind.BLL.Models
{
    public class CategoryApiModel : ICategory, IApiModel<int>
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public int RowId { get; set; }
    }
}
