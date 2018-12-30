using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    internal sealed class CustomerHistoryRepository : Repository<CustomerHistoryDbModel, long>
    {
        private new NorthwindDbContext Context { get; set; }

        public CustomerHistoryRepository(NorthwindDbContext context) : base(context)
        {
            this.Context = context;
        }

        public override IQueryable<CustomerHistoryDbModel> FetchAll => this.Context.CustomerHistory;

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
    }
}
