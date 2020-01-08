using System;
using System.Collections.Generic;
using Wholesale.BL.Enums;

namespace Wholesale.BL.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int? CourierId { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
