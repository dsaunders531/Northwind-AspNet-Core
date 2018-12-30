using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    internal sealed class EmployeeHistoryRepository : Repository<EmployeeHistoryDbModel, long>
    {
        private new NorthwindDbContext Context { get; set; }

        public EmployeeHistoryRepository(NorthwindDbContext context) : base(context)
        {
            this.Context = context;
        }

        public override IQueryable<EmployeeHistoryDbModel> FetchAll => this.Context.EmployeeHistory;

        public override void Create(EmployeeHistoryDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(EmployeeHistoryDbModel item)
        {
            // history records must not be deleted.
            throw new ApplicationException("History records cannot be deleted.");
        }

        public override EmployeeHistoryDbModel Fetch(long id)
        {
            return (from EmployeeHistoryDbModel e in this.FetchAll where e.RowId == id select e).First();
        }

        public override void Update(EmployeeHistoryDbModel item)
        {
            this.Context.Update(item);
        }
    }
}
