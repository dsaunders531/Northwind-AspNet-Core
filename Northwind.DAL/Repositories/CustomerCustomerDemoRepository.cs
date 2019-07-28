﻿using duncans.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer demographic repository
    /// </summary>
    public sealed class CustomerCustomerDemoRepository : EFRepositoryBase<CustomerCustomerDemoDbModel, long>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public CustomerCustomerDemoRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<CustomerCustomerDemoDbModel> FetchAll
        {
            get
            {
                return this.Context.CustomerCustomerDemo.Include(c => c.Customer)
                                                            .ThenInclude(o => o.Orders)
                                                            .ThenInclude(a => a.OrderDetails)
                                                        .Include(d => d.CustomerType);
            }
        }

        public override IQueryable<CustomerCustomerDemoDbModel> FetchRaw => throw new System.NotImplementedException();

        public override void Create(CustomerCustomerDemoDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(CustomerCustomerDemoDbModel item)
        {
            this.Context.Remove(item);
        }

        public override CustomerCustomerDemoDbModel Fetch(long id)
        {
            return (from CustomerCustomerDemoDbModel c in this.FetchAll where c.CustomerId == id select c).FirstOrDefault();
        }

        public IQueryable<CustomerCustomerDemoDbModel> FetchByCustomerId(long customerId)
        {
            return from CustomerCustomerDemoDbModel c in this.FetchAll where c.CustomerId == customerId select c;
        }

        public IQueryable<CustomerCustomerDemoDbModel> FetchByCustomerTypeId(long customerTypeId)
        {
            return (from CustomerCustomerDemoDbModel c in this.FetchAll where c.CustomerTypeId == customerTypeId select c);
        }

        public override void Update(CustomerCustomerDemoDbModel item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            this.IgnoreRelations(item);
            this.Context.Update(item);
        }

        public override void Ignore(CustomerCustomerDemoDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(CustomerCustomerDemoDbModel item)
        {
            this.Context.Attach(item.Customer);
            this.Context.AttachRange(item.Customer.Orders);
            this.Context.AttachRange(item.Customer.Orders.Select(o => o.OrderDetails));
            this.Context.Attach(item.CustomerType);

        }
    }
}
