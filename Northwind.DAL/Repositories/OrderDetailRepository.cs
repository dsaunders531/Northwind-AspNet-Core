using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;
using tools.EF;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order detail repository
    /// </summary>
    public sealed class OrderDetailRepository : Repository<OrderDetail, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public OrderDetailRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<OrderDetail> FetchAll
        {
            get
            {
                return Context.OrderDetails
                            .Include(p => p.Product)
                            .Include(o => o.Order)
                                .ThenInclude(c => c.Customer)
                            .Include(o => o.Order)
                                .ThenInclude(e => e.Employee)
                            .Include(o => o.Order)
                                .ThenInclude(s => s.ShipViaNavigation);
            }
        }

        public override void Create(OrderDetail item)
        {
            Context.Add(item);
        }

        public override void Update(OrderDetail item)
        {
            Context.Attach(item.Product);
            Context.Attach(item.Order);
            Context.Attach(item.Order.Customer);
            Context.Attach(item.Order.Employee);
            Context.Attach(item.Order.ShipViaNavigation);

            Context.Update(item);
        }

        public override void Delete(OrderDetail item)
        {
            Context.Remove(item);
        }

        public override OrderDetail Fetch(int id)
        {
            return (from OrderDetail d in FetchAll where d.OrderId == id select d).FirstOrDefault();
        }

        public IQueryable<OrderDetail> FetchByProductId(int productId)
        {
            return (from OrderDetail d in FetchAll where d.ProductId == productId select d);
        }
    }
}
