using System;

namespace Northwind.DAL
{
    /// <summary>
    /// Create the seed data for the application.
    /// </summary>
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider)
        {
            // There is no seed data. The original database already has data.

            //NorthwindContext context = serviceProvider.GetRequiredService<NorthwindContext>();
            //context.SaveChanges();
        }
    }
}
