using System;
using System.Collections.Generic;

namespace Wholesale.BL.Models
{
    public class Delivery
    {
        public Delivery()
        {
            DeliveryDetails = new HashSet<DeliveryDetails>();
        }

        public int DeliveryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        public virtual User Employee { get; set; }
        public virtual ICollection<DeliveryDetails> DeliveryDetails { get; set; }
    }
}
