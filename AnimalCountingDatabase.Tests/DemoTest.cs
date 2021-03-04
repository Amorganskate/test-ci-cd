using AnimalCountingDatabaseAPI;
using AnimalCountingDatabaseAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnimalCountingDatabase.Tests
{
    public class DemoTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public async Task CustomerIntegrationTest()
        {
            // Create DB Context 
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnviromentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            var context = new CustomerContext(optionsBuilder.Options);


            // Delete all existing customers in db
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create Controller
            var controller = new CustomersController(context);

            // Add Customer 
            await controller.Add(new Customer()
            {
                CustomerName = "Foobar",
            });

            // Check: Does GetALL Return Added customer

            var result = (await controller.GetAll()).ToArray();

            Assert.Single(result);
            Assert.Equal("Foobar", result[0].CustomerName);

        }
    }

}


