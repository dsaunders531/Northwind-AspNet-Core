using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The Category Repository
    /// </summary>
    public sealed class CategoryRepository : Repository<Category, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public CategoryRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Category> FetchAll
        {
            get
            {
                // Get the categories and all the related data.
                IQueryable<Category> result = Context.Categories
                        .Include(p => p.Products)
                            .ThenInclude(s => s.Supplier)
                        .Include(p => p.Products)
                            .ThenInclude(o => o.OrderDetails)
                                .ThenInclude(a => a.Order);

                return result;
            }
        }

        public override void Create(Category item)
        {
            this.Context.Add(item);
            this.Context.Add<Category>(item);
        }

        public override void Delete(Category item)
        {
            this.Context.Remove(item);
        }

        public override Category Fetch(int id)
        {
            return (from Category c in this.FetchAll where c.CategoryId == id select c).FirstOrDefault();
        }

        public override void Update(Category item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            this.Context.AttachRange(item.Products);
            this.Context.AttachRange(item.Products.Select(s => s.Supplier));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails));
            this.Context.AttachRange(item.Products.Select(b => b.OrderDetails.Select(m => m.Order)));

            this.Context.Update(item);
        }
    }
}
