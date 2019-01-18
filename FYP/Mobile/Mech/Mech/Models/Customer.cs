using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace Mech.Models
{
    public class Customer
    {

        public Customer()
        {
            Contracts = new List<Contract>();
        }

        public int Id { get; set; }

         
        public string Name { get; set; }

         
        public string ContactNo { get; set; }

        
        public string Vehicle { get; set; }

          
        public string Model { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        
        public string CurrentConnection { get; set; }

       
        public string ImageUrl { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
