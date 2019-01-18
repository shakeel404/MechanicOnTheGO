using Mech.ViewModels;
using System.Collections;
using System.Collections.Generic; 

namespace Mech.Models
{
    public class Mechanic
    {
        public Mechanic()
        {
            Contracts = new List<Contract>();
        }
        public int Id { get; set; } 
       
         
        public string Title { get; set; }

       
        public string Name { get; set; }

      
        public string ContactNo { get; set; }

      
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

       
        public string CurrentConnection { get; set; }

        
        public string ImageUrl { get; set; }

        public ICollection<Contract> Contracts { get; set; } 
    }
}
