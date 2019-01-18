using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Mech.Api.Models
{
    public class Customer
    {

        public Customer()
        {
            Contracts = new List<Contract>();
        }

        public int Id { get; set; }

        [MaxLength(30)] 
        public string Name { get; set; }

        [MaxLength(16)] 
        public string ContactNo { get; set; }

        [MaxLength(15)] 
        public string Vehicle { get; set; }

        [MaxLength(10)] 
        public string Model { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
