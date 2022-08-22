using tools.EF;
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

        public ShipperRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Shipper> FetchAll
        {
            get
            {
                return Context.Shippers
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
            Context.Add(item);
        }

        public override void Update(Shipper item)
        {
            Context.AttachRange(item.Orders);
            Context.AttachRange(item.Orders.Select(c => c.Customer));
            Context.AttachRange(item.Orders.Select(e => e.Employee));
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));

            Context.Update(item);
        }

        public override void Delete(Shipper item)
        {
            Context.Remove(item);
        }

        public override Shipper Fetch(int id)
        {
            return (from Shipper s in FetchAll where s.ShipperId == id select s).FirstOrDefault();
        }
    }
}
