using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mech.Api.Context;
using Mech.Api.Models;
using Microsoft.AspNetCore.Mvc;
namespace Mech.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TestController :  ControllerBase
    {
        MechDbContext Db;

        public TestController(MechDbContext context)
        {
            Db = context;
                
        }

        [HttpPost]
        public ActionResult NewCustomer(int id)
        {
            var customer = new Customer();

            string number = string.Empty;

            Random random = new Random(1);

            while (number.Length <= 11)
                number += random.Next(0, 9).ToString();


            customer.ContactNo = number;
            customer.Name = "Muhammad Shakeel" + id.ToString();
            customer.Vehicle = "vehcle" + id.ToString();
            customer.Model = "model" + id.ToString();
            customer.Latitude =  random.Next(234, 675); 
            customer.Longitude = random.Next(234,675);

            Db.Customers.Add(customer);

            Db.SaveChanges();

            return Ok(customer.Id);
        }

        [HttpGet]
        public List<Customer> GetAll()
        {
            return Db.Customers.ToList();
        }
    }
}
