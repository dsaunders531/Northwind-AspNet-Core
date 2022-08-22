using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The product repository
    /// </summary>
    public sealed class ProductRepository : Repository<Product, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public ProductRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Product> FetchAll
        {
            get
            {
                return Context.Products
                            .Include(c => c.Category)
                            .Include(s => s.Supplier)
                            .Include(o => o.OrderDetails)
                                .ThenInclude(a => a.Order);
            }
        }

        public override void Create(Product item)
        {
            Context.Add(item);
        }

        public override void Update(Product item)
        {
            Context.Attach(item.Category);
            Context.Attach(item.Supplier);
            Context.AttachRange(item.OrderDetails);
            Context.AttachRange(item.OrderDetails.Select(o => o.Order));

            Context.Update(item);
        }

        public override void Delete(Product item)
        {
            Context.Remove(item);
        }

        public override Product Fetch(int id)
        {
            return (from Product p in FetchAll where p.ProductId == id select p).FirstOrDefault();
        }

        public IQueryable<Product> FetchBySupplierId(int supplierId)
        {
            return (from Product p in FetchAll where p.SupplierId == supplierId select p);
        }

        public IQueryable<Product> FetchByCategoryId(int categoryId)
        {
            return (from Product p in FetchAll where p.CategoryId == categoryId select p);
        }
    }
}
