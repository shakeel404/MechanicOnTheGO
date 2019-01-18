using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MechModels
{
    public class Contract
    {
        
        public int Id { get; set; }

        public int MechanicId { get; set; }
        public int CustomerId { get; set; }

        public Mechanic Mechanic { get; set; }

        public Customer Customer { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Time { get; set; } 

        public bool PaidByCustomer { get; set; }

        public bool RecievedByMehanic { get; set; }

        public int Rating { get; set; }

        public int InitialCharges { get; set; }

        public int RatePerKM { get; set; }

        public int RatePerHour { get; set; }

        public int KM { get; set; }

       [Column(TypeName = "datetime2")]
        public DateTime? WorkStarted { get; set; } 

        public bool IsWorkStarted { get; set; }

        public bool IsWorkFinished { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? WorkFinished { get; set; }

    }
}
