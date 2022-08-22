using tools.EF;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    /// <summary>
    /// The retail inventory service.
    /// This exposes the worker as a service and provides a place for any code which does not need to be in the worker.
    /// </summary>
    public sealed class RetailInventoryService : RetailInventoryWorker
    {
        public RetailInventoryService(IRepository<Product, int> products, IRepository<Category, int> categories) : base(products, categories)
        { }
    }
}
