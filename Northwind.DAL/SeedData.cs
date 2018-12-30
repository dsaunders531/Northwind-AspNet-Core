using mezzanine;
using mezzanine.DbClient;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.DAL.Services;
using Northwind.DAL.Repositories;

namespace Northwind.DAL
{
    /// <summary>
    /// Create the seed data for the application.
    /// </summary>
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider)
        {
            NorthwindDbContext context = serviceProvider.GetRequiredService<NorthwindDbContext>();

            // Seed data will need adding. It will be difficult as there are remapped column names.

            // make changes so transactions work better.
            using (DbClient client = context.GetAdvancedClient())
            {
                client.RunDML("ALTER DATABASE [Northwind] SET ALLOW_SNAPSHOT_ISOLATION ON");
                client.RunDML("ALTER DATABASE[Northwind] SET READ_COMMITTED_SNAPSHOT ON");
            }
        }
    }
}
