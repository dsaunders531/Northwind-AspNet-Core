﻿using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The region repository
    /// </summary>
    public sealed class RegionRepository : Repository<RegionDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public RegionRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<RegionDbModel> FetchAll
        {
            get
            {
                return this.Context.Region
                        .Include(t => t.Territories)
                            .ThenInclude(e => e.EmployeeTerritories)
                                .ThenInclude(m => m.Employee);
            }
        }

        public override void Create(RegionDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(RegionDbModel item)
        {
            this.Context.AttachRange(item.Territories);
            this.Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories));
            this.Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories.Select(a => a.Employee)));

            this.Context.Update(item);
        }

        public override void Delete(RegionDbModel item)
        {
            this.Context.Remove(item);
        }

        public override RegionDbModel Fetch(int id)
        {
            return (from RegionDbModel r in this.FetchAll where r.RowId == id select r).FirstOrDefault();
        }
    }
}
