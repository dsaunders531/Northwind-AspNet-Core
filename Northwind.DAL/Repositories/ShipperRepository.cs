using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The shipper repository
    /// </summary>
    public sealed class ShipperRepository : Repository<ShipperDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public ShipperRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<ShipperDbModel> FetchAll
        {
            get
            {
                return this.Context.Shippers
                            .Include(o => o.Orders)
                                .ThenInclude(c => c.Customer)
                            .Include(o => o.Orders)
                                .ThenInclude(e => e.Employee)
                            .Include(o => o.Orders)
                                .ThenInclude(d => d.OrderDetails)
                                    .ThenInclude(p => p.Product);
            }
        }

        public override void Create(ShipperDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(ShipperDbModel item)
        {
            this.Context.AttachRange(item.Orders);
            this.Context.AttachRange(item.Orders.Select(c => c.Customer));
            this.Context.AttachRange(item.Orders.Select(e => e.Employee));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));

            this.Context.Update(item);
        }

        public override void Delete(ShipperDbModel item)
        {
            this.Context.Remove(item);
        }

        public override ShipperDbModel Fetch(int id)
        {
            return (from ShipperDbModel s in this.FetchAll where s.ShipperId == id select s).FirstOrDefault();
        }
    }
}
