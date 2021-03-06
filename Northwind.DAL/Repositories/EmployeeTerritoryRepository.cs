﻿using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    public sealed class EmployeeTerritoryRepository : Repository<EmployeeTerritory, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public EmployeeTerritoryRepository(NorthwindContext context) : base(context) { this.Context = context;  }

        public override IQueryable<EmployeeTerritory> FetchAll
        {
            get
            {
                return this.Context.EmployeeTerritories
                                .Include(e => e.Employee)
                                .Include(t => t.Territory);
            }
        }

        public override void Create(EmployeeTerritory item)
        {
            this.Context.Add(item);
        }

        public override void Delete(EmployeeTerritory item)
        {
            this.Context.Remove(item);
        }

        public override EmployeeTerritory Fetch(int id)
        {
            return (from EmployeeTerritory t in this.FetchAll where id == t.EmployeeId select t).FirstOrDefault();
        }

        public IQueryable<EmployeeTerritory> FetchByEmployeeId(int employeeId)
        {
            return from EmployeeTerritory t in this.FetchAll where employeeId == t.EmployeeId select t;
        }

        public IQueryable<EmployeeTerritory> FetchByTerritoryId(string territoryId)
        {
            return from EmployeeTerritory t in this.FetchAll where territoryId == t.TerritoryId select t;
        }

        public override void Update(EmployeeTerritory item)
        {
            this.Context.Attach(item.Employee);
            this.Context.Attach(item.Territory);

            this.Context.Update(item);
        }
    }
}
