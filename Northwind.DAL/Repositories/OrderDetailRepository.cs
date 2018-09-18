using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order detail repository
    /// </summary>
    public sealed class OrderDetailRepository : Repository<OrderDetail, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public OrderDetailRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<OrderDetail> FetchAll
        {
            get
            {
                return this.Context.OrderDetails
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
            this.Context.Add(item);
        }

        public override void Update(OrderDetail item)
        {
            this.Context.Attach(item.Product);
            this.Context.Attach(item.Order);
            this.Context.Attach(item.Order.Customer);
            this.Context.Attach(item.Order.Employee);
            this.Context.Attach(item.Order.ShipViaNavigation);

            this.Context.Update(item);
        }

        public override void Delete(OrderDetail item)
        {
            this.Context.Remove(item);
        }

        public override OrderDetail Fetch(int id)
        {
            return (from OrderDetail d in this.FetchAll where d.OrderId == id select d).FirstOrDefault();
        }

        public IQueryable<OrderDetail> FetchByProductId(int productId)
        {
            return (from OrderDetail d in this.FetchAll where d.ProductId == productId select d);
        }
    }
}
