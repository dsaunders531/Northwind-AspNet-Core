using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The product repository
    /// </summary>
    internal sealed class ProductRepository : Repository<ProductDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public ProductRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<ProductDbModel> FetchAll
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

        public override void Create(ProductDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(ProductDbModel item)
        {
            this.Context.Attach(item.Category);
            this.Context.Attach(item.Supplier);
            this.Context.AttachRange(item.OrderDetails);
            this.Context.AttachRange(item.OrderDetails.Select(o => o.Order));

            this.Context.Update(item);
        }

        public override void Delete(ProductDbModel item)
        {
            this.Context.Remove(item);
        }

        public override ProductDbModel Fetch(int id)
        {
            return (from ProductDbModel p in this.FetchAll where p.RowId == id select p).FirstOrDefault();
        }

        public IQueryable<ProductDbModel> FetchBySupplierId(int supplierId)
        {
            return (from ProductDbModel p in this.FetchAll where p.SupplierId == supplierId select p);
        }

        public IQueryable<ProductDbModel> FetchByCategoryId(int categoryId)
        {
            return (from ProductDbModel p in this.FetchAll where p.CategoryId == categoryId select p);
        }
    }
}
