using Northwind.BLL.Workers;
using Northwind.DAL.Models;
using tools.EF;

namespace Northwind.BLL.Services
{
    public sealed class CategoryService : CategoryWorker
    {
        public CategoryService(IRepository<Category, int> categories) : base(categories)
        {
        }
    }
}
