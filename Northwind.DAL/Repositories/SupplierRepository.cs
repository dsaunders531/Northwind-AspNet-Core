using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The supplier repository
    /// </summary>
    public sealed class SupplierRepository : Repository<Supplier, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public SupplierRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Supplier> FetchAll
        {
            get
            {
                return this.Context.Suppliers
                            .Include(p => p.Products)
                                .ThenInclude(c => c.Category)
                            .Include(p => p.Products)
                                .ThenInclude(o => o.OrderDetails)
                                    .ThenInclude(r => r.Order);
            }
        }

        public override void Create(Supplier item)
        {
            this.Context.Add(item);
        }

        public override void Update(Supplier item)
        {
            this.Context.AttachRange(item.Products);
            this.Context.AttachRange(item.Products.Select(c => c.Category));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails.Select(r => r.Order)));

            this.Context.Update(item);
        }

        public override void Delete(Supplier item)
        {
            this.Context.Remove(item);
        }

        public override Supplier Fetch(int id)
        {
            return (from Supplier s in this.FetchAll where s.SupplierId == id select s).FirstOrDefault();
        }
    }
}
