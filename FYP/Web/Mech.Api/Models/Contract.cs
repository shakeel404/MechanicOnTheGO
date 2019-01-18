using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mech.Api.Models
{
    public class Contract
    {
        public int Id { get; set; }

        public int MechanicId { get; set; }
        public int CustomerId { get; set; }

        public Mechanic Mechanic { get; set; }

        public Customer Customer { get; set; }

        public DateTime Time { get; set; }

        public int Amount { get; set; }

        public bool PaidByCustomer { get; set; }

        public bool RecievedByMehanic { get; set; }

        public int Rating { get; set; }

    }
}
