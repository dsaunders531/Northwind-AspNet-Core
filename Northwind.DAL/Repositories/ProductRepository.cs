using tools.EF;
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

        public ProductRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Product> FetchAll
        {
            get
            {
                return this.Context.Products
                            .Include(c => c.Category)
                            .Include(s => s.Supplier)
                            .Include(o => o.OrderDetails)
                                .ThenInclude(a => a.Order);
            }
        }

        public override void Create(Product item)
        {
            this.Context.Add(item);
        }

        public override void Update(Product item)
        {
            this.Context.Attach(item.Category);
            this.Context.Attach(item.Supplier);
            this.Context.AttachRange(item.OrderDetails);
            this.Context.AttachRange(item.OrderDetails.Select(o => o.Order));

            this.Context.Update(item);
        }

        public override void Delete(Product item)
        {
            this.Context.Remove(item);
        }

        public override Product Fetch(int id)
        {
            return (from Product p in this.FetchAll where p.ProductId == id select p).FirstOrDefault();
        }

        public IQueryable<Product> FetchBySupplierId(int supplierId)
        {
            return (from Product p in this.FetchAll where p.SupplierId == supplierId select p);
        }

        public IQueryable<Product> FetchByCategoryId(int categoryId)
        {
            return (from Product p in this.FetchAll where p.CategoryId == categoryId select p);
        }
    }
}
