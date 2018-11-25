using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order detail repository
    /// </summary>
    public sealed class OrderDetailRepository : Repository<OrderDetailDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public OrderDetailRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<OrderDetailDbModel> FetchAll
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

        public override void Create(OrderDetailDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(OrderDetailDbModel item)
        {
            this.Context.Attach(item.Product);
            this.Context.Attach(item.Order);
            this.Context.Attach(item.Order.Customer);
            this.Context.Attach(item.Order.Employee);
            this.Context.Attach(item.Order.ShipViaNavigation);

            this.Context.Update(item);
        }

        public override void Delete(OrderDetailDbModel item)
        {
            this.Context.Remove(item);
        }

        public override OrderDetailDbModel Fetch(int id)
        {
            return (from OrderDetailDbModel d in this.FetchAll where d.OrderId == id select d).FirstOrDefault();
        }

        public IQueryable<OrderDetailDbModel> FetchByProductId(int productId)
        {
            return (from OrderDetailDbModel d in this.FetchAll where d.ProductId == productId select d);
        }
    }
}
