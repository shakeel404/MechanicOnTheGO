using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MechModels
{
    public class Mechanic
    {
        public Mechanic()
        {
            Contracts = new List<Contract>();
        }
        public int Id { get; set; } 
       
        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(30)] 
        public string Name { get; set; }

        [MaxLength(16)]
        public string ContactNo { get; set; }

        [MaxLength(200)] 
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; } 

        [MaxLength(200)]
        public string CurrentConnection { get; set; }  

        [MaxLength(200)]
        public string ImageUrl { get; set; }

        public ICollection<Contract> Contracts { get; set; } 
    }
}
