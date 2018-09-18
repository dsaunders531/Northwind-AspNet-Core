using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer demographic repository
    /// </summary>
    public sealed class CustomerDemographicRepository : Repository<CustomerDemographic, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public CustomerDemographicRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<CustomerDemographic> FetchAll
        {
            get
            {
                return this.Context.CustomerDemographics
                                .Include(d => d.CustomerCustomerDemo)
                                    .ThenInclude(c => c.Customer);
            }
        }       

        public override void Create(CustomerDemographic item)
        {
            this.Context.Add(item);
        }

        public override void Delete(CustomerDemographic item)
        {
            this.Context.Remove(item);
        }

        public override CustomerDemographic Fetch(string id)
        {
            return (from CustomerDemographic d in this.FetchAll where d.CustomerTypeId == id select d).FirstOrDefault();
        }

        public override void Update(CustomerDemographic item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            this.Context.AttachRange(item.CustomerCustomerDemo);
            this.Context.AttachRange(item.CustomerCustomerDemo.Select(c => c.Customer));

            this.Context.Update(item);
        }
    }
}
