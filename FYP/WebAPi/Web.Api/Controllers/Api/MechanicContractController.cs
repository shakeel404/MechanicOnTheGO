using MechModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Models;
using System.Data.Entity;
using Web.Api.Providers;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Api
{
    public class MechanicContractController : ApiController
    {

        ApplicationDbContext Db = new ApplicationDbContext();

        public IEnumerable<string> Get()
        {
            return null;
        }

        
        public IQueryable<Contract> Get(int id)
        {
            return Db.Contracts.Include(x=>x.Customer).Where(c => c.MechanicId == id).OrderByDescending(c=>c.Time);
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

            contract.RecievedByMehanic = true;
            Db.SaveChanges();


            var customer = Db.Customers.Find(contract.CustomerId);

            var mechanic = Db.Mechanics.Find(contract.MechanicId);

            var notificationMessage = $"{mechanic.Name} has confirmed payment.";

            var notifHub = new NotificationHub();

          await  notifHub.SendAsync(customer.CurrentConnection, notificationMessage, "Confirmation");

            return Ok(contract);
        }

        
        public void Delete(int id)
        {
        }
    }
}
