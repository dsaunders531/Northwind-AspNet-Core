using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The supplier repository
    /// </summary>
    public sealed class SupplierRepository : Repository<SupplierDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public SupplierRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<SupplierDbModel> FetchAll
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

        public override void Create(SupplierDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(SupplierDbModel item)
        {
            this.Context.AttachRange(item.Products);
            this.Context.AttachRange(item.Products.Select(c => c.Category));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails.Select(r => r.Order)));

            this.Context.Update(item);
        }

        public override void Delete(SupplierDbModel item)
        {
            this.Context.Remove(item);
        }

        public override SupplierDbModel Fetch(int id)
        {
            return (from SupplierDbModel s in this.FetchAll where s.SupplierId == id select s).FirstOrDefault();
        }
    }
}
