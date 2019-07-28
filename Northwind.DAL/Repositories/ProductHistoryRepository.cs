using duncans.EF;
using Northwind.DAL.Models;
using System;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    internal sealed class ProductHistoryRepository : EFRepositoryBase<ProductHistoryDbModel, long>
    {
        private new NorthwindDbContext Context { get; set; }

        public ProductHistoryRepository(NorthwindDbContext context) : base(context)
        {
            this.Context = context;
        }

        public override IQueryable<ProductHistoryDbModel> FetchAll => this.Context.ProductHistory;

        public override IQueryable<ProductHistoryDbModel> FetchRaw => throw new NotImplementedException();

        public override void Create(ProductHistoryDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(ProductHistoryDbModel item)
        {
            // history records must not be deleted.
            throw new ApplicationException("History records cannot be deleted.");
        }

        public override ProductHistoryDbModel Fetch(long id)
        {
            return (from ProductHistoryDbModel h in this.FetchAll where h.RowId == id select h).First();
        }

        public override void Update(ProductHistoryDbModel item)
        {
            this.Context.Update(item);
        }

        public override void Ignore(ProductHistoryDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(ProductHistoryDbModel item)
        {
            // Do nothing
        }
    }
}
