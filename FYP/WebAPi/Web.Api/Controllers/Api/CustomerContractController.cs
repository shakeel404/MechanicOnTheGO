using MechModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Api.Models;
using Web.Api.Providers;

namespace Web.Api.Controllers.Api
{
    public class CustomerContractController : ApiController
    {
        ApplicationDbContext Db = new ApplicationDbContext();
      
        public IEnumerable<string> Get()
        {
            return null;
        }

         
        public IQueryable<Contract> Get(int id)
        {
            return Db.Contracts.Include(x=>x.Mechanic).Where(c => c.CustomerId == id).OrderByDescending(c=>c.Time);
        }

         
        public void Post([FromBody]string value)
        {
        }

        
        public async Task<IHttpActionResult> Put(int id)
        {
            var contract = Db.Contracts.Find(id);

            if (contract == null)
            {
                return NotFound();
            }

            contract.PaidByCustomer = true;
            Db.SaveChanges();


            var customer = Db.Customers.Find(contract.CustomerId);

            var mechanic = Db.Mechanics.Find(contract.MechanicId);

            var notificationMessage = $"{customer.Name} has confirmed payment.";

            var notifHub = new NotificationHub();

          await  notifHub.SendAsync(mechanic.CurrentConnection, notificationMessage, "Confirmation");

            return Ok(contract);
        }

        
        public void Delete(int id)
        {
        }
    }
}
