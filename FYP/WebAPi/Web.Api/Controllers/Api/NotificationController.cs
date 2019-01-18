using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FCM.Net;
using Web.Api.Models;
using Web.Api.Models.MechModels;
using Web.Api.Providers;

namespace Web.Api.Controllers.Api
{
    public class NotificationController : ApiController
    {

        public const string CUSTOMER = "Customer";
        public const string MECHANIC = "Mechanic";

        ApplicationDbContext Db = new ApplicationDbContext();

        public async System.Threading.Tasks.Task<string> GetAsync()
        {

            
               
            return $"Notification Hub";
        }

        
        public string Get(int id)
        {
            return "Push Notification Hub";
        }

      
        public void Post(int id)
        {

           
        }

        
        public void Put(int id, NotificationPayLoad notificationPayLoad)
        {
            var sender = notificationPayLoad.Sender;

            if (sender == CUSTOMER)
            {
                var customer = Db.Customers.Find(id);
                customer.CurrentConnection = notificationPayLoad.Token;
                Db.SaveChanges();
            }
            else if (sender == MECHANIC)
            {
                var mechanic = Db.Mechanics.Find(id);
                mechanic.CurrentConnection = notificationPayLoad.Token;
                Db.SaveChanges();
            }
        }

        
        public void Delete(int id)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
        }
    }
}
