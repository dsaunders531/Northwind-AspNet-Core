using duncans.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The supplier repository
    /// </summary>
    public sealed class SupplierRepository : EFRepositoryBase<SupplierDbModel, int>
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

        public override IQueryable<SupplierDbModel> FetchRaw => throw new System.NotImplementedException();

        public override void Create(SupplierDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(SupplierDbModel item)
        {
            this.IgnoreRelations(item);
            this.Context.Update(item);
        }

        public override void Delete(SupplierDbModel item)
        {
            this.IgnoreRelations(item);
            this.Context.Remove(item);
        }

        public override SupplierDbModel Fetch(int id)
        {
            return (from SupplierDbModel s in this.FetchAll where s.RowId == id select s).FirstOrDefault();
        }

        public override void Ignore(SupplierDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(SupplierDbModel item)
        {
            this.Context.AttachRange(item.Products);
            this.Context.AttachRange(item.Products.Select(c => c.Category));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails));
            this.Context.AttachRange(item.Products.Select(o => o.OrderDetails.Select(r => r.Order)));
        }
    }
}
