using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MechModels;
using Microsoft.AspNet.SignalR;
using Web.Api.Models;
using Web.Api.Providers;

namespace Web.Api
{
    public class ContractHub : Hub
    {
        ApplicationDbContext Db = new ApplicationDbContext();

        public override Task OnConnected()
        {

            var expiredList = Requestees.Where(r => (DateTime.Now - r.Time).Minutes >= 5).ToList() ;

            if(expiredList!=null && expiredList.Count>0)
            {
                Requestees = Requestees.Except(expiredList).ToList();
            }

            return base.OnConnected();


        }

        private static List<Requestee> Requestees = new List<Requestee>();

        public void Connect(string contactnumber)
        {
           var requestee =Requestees.Where(r => r.ContactNo == contactnumber).FirstOrDefault();
            if (requestee !=null )
            {
                Clients.Caller.requested(requestee.Name,requestee.Latitude,requestee.Longitude);
            }
        }

        public void DisCard(string mechContact)
        {
            var contract = Requestees.Where(c => c.ContactNo == mechContact).FirstOrDefault();

            if(contract != null)
            {
                Requestees.Remove(contract);
            }

            Clients.Others.discard();
        }

        public void SendRequest(string name, string Mechcontact,double lati,double longi)
        {
            if (!(Requestees.Where(r => r.ContactNo == Mechcontact).Count() > 0))
            {
                Requestees.Add(new Requestee(name, Mechcontact,lati,longi));
            } 

           
            Clients.Others.requested(name,lati,longi);

            var mechanic = Db.Mechanics.Where(m => m.ContactNo == Mechcontact).FirstOrDefault();

            if (mechanic != null)
            {
                var notifHub = new NotificationHub();
                notifHub.SendAsync(mechanic.CurrentConnection, $"{name} has sent you a request.");
            }
        }


        public void ConfermRequest( int initialCharges,int chargesPerKM,int chargesPerHour,string km)
        {
          
            Clients.Others.conferm(initialCharges,chargesPerKM,chargesPerHour,km); 
        }


        public void AcceptRequest(int mechanicId, int customerId,int initialCharges,int chargesPerKm,int chargesPerHour,int km)
        {


           

            Clients.All.accepted();



            try
            {
                Contract contract = new Contract
                {
                    MechanicId = mechanicId,
                    CustomerId = customerId,
                    Time = DateTime.UtcNow,
                    InitialCharges = initialCharges,
                    RatePerKM = chargesPerKm,
                    RatePerHour = chargesPerHour,
                    KM = km,
                    Rating = 0

                };
                Db.Contracts.Add(contract);
                Db.SaveChanges();
            }
            catch
            {
            }
            var total = (initialCharges + (chargesPerKm * 2) + chargesPerHour);

            var mechanic = Db.Mechanics.Find(mechanicId);

            var requestee = Requestees.FirstOrDefault(r => r.ContactNo == mechanic.ContactNo);
            Requestees.Remove(requestee);



            var customer = Db.Customers.Find(customerId);

            if (customer != null)
            {
                var notifHub = new NotificationHub();
                notifHub.SendAsync(customer.CurrentConnection, $"{mechanic.Name} Accepted Agreement on {total}.", $"Accepted");

            }

            if (mechanic != null)
            {
                var notifHub = new NotificationHub();
                notifHub.SendAsync(mechanic.CurrentConnection, $"{customer.Name} Accepted Agreement on {total}.", $"Accepted");
            }



            
        }
    }


    class Requestee
    {
        public Requestee(string name,string contact,double lati,double longi)
        {
            this.Name = name;
            this.ContactNo = contact;
            this.Latitude = lati;
            this.Longitude = longi;
        }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Time
        {
            get { return DateTime.Now; }
        }
    }
}