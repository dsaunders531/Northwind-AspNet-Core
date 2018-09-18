using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The shipper repository
    /// </summary>
    public sealed class ShipperRepository : Repository<Shipper, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public ShipperRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Shipper> FetchAll
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

        public override void Create(Shipper item)
        {
            this.Context.Add(item);
        }

        public override void Update(Shipper item)
        {
            this.Context.AttachRange(item.Orders);
            this.Context.AttachRange(item.Orders.Select(c => c.Customer));
            this.Context.AttachRange(item.Orders.Select(e => e.Employee));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));

            this.Context.Update(item);
        }

        public override void Delete(Shipper item)
        {
            this.Context.Remove(item);
        }

        public override Shipper Fetch(int id)
        {
            return (from Shipper s in this.FetchAll where s.ShipperId == id select s).FirstOrDefault();
        }
    }
}
