using System;
using System.Linq;
using Alinta.CustomersApi.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Alinta.CustomersApi.Data
{
    public static class PopulateCustomerDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CustomerDbContext>());
            }
        }

        private static void SeedData(CustomerDbContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer() { FirstName = "John", LastName = "Wayne", DateOfBirth = new DateTime(1999, 12, 11) },
                    new Customer() { FirstName = "Jane", LastName = "West", DateOfBirth = new DateTime(1988, 5, 23) },
                    new Customer() { FirstName = "Jill", LastName = "Williams", DateOfBirth = new DateTime(2002, 6, 15) }
                );

                context.SaveChanges();
            }
        }
    }
}
