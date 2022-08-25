using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;
using tools.EF;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The Category Repository
    /// </summary>
    public sealed class CategoryRepository : Repository<Category, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public CategoryRepository(NorthwindContext context) : base(context) { Context = context; }

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
            Context.Add(item);
            Context.Add<Category>(item);
        }

        public override void Delete(Category item)
        {
            Context.Remove(item);
        }

        public override Category Fetch(int id)
        {
            return (from Category c in FetchAll where c.CategoryId == id select c).FirstOrDefault();
        }

        public override void Update(Category item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            Context.AttachRange(item.Products);
            Context.AttachRange(item.Products.Select(s => s.Supplier));
            Context.AttachRange(item.Products.Select(o => o.OrderDetails));
            Context.AttachRange(item.Products.Select(b => b.OrderDetails.Select(m => m.Order)));

            Context.Update(item);
        }
    }
}
