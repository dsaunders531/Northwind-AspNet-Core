using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer demographic repository
    /// </summary>
    public sealed class CustomerDemographicRepository : Repository<CustomerDemographicDbModel, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public CustomerDemographicRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<CustomerDemographicDbModel> FetchAll
        {
            get
            {
                return this.Context.CustomerDemographics
                                .Include(d => d.CustomerCustomerDemo)
                                    .ThenInclude(c => c.Customer);
            }
        }       

        public override void Create(CustomerDemographicDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(CustomerDemographicDbModel item)
        {
            this.Context.Remove(item);
        }

        public override CustomerDemographicDbModel Fetch(string id)
        {
            return (from CustomerDemographicDbModel d in this.FetchAll where d.CustomerTypeId == id select d).FirstOrDefault();
        }

        public override void Update(CustomerDemographicDbModel item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            this.Context.AttachRange(item.CustomerCustomerDemo);
            this.Context.AttachRange(item.CustomerCustomerDemo.Select(c => c.Customer));

            this.Context.Update(item);
        }
    }
}
