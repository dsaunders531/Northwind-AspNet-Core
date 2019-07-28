using duncans.EF;

namespace Northwind.DAL
{
    public interface ICategory
    {
        string CategoryName { get; set; }
        string Description { get; set; }
        byte[] Picture { get; set; }
    }
}