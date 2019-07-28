using duncans.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    internal sealed class CustomerHistoryRepository : EFRepositoryBase<CustomerHistoryDbModel, long>
    {
        private new NorthwindDbContext Context { get; set; }

        public CustomerHistoryRepository(NorthwindDbContext context) : base(context)
        {
            this.Context = context;
        }

        public override IQueryable<CustomerHistoryDbModel> FetchAll => this.Context.CustomerHistory;

        public override IQueryable<CustomerHistoryDbModel> FetchRaw => throw new NotImplementedException();

        public override void Create(CustomerHistoryDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(CustomerHistoryDbModel item)
        {
            // history records must not be deleted.
            throw new ApplicationException("History records cannot be deleted.");
        }

        public override CustomerHistoryDbModel Fetch(long id)
        {
            return (from CustomerHistoryDbModel h in this.FetchAll where h.RowId == id select h).First();
        }

        public override void Update(CustomerHistoryDbModel item)
        {
            this.Context.Update(item);
        }

        public override void Ignore(CustomerHistoryDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(CustomerHistoryDbModel item)
        {
            // Do nothing
        }
    }
}
