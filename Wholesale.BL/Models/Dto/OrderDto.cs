using System;
using System.Collections.Generic;
using System.Linq;
using Wholesale.BL.Enums;

namespace Wholesale.BL.Models.Dto
{
    public class OrderDto
    {
        public int? OrderId { get; set; }
        public int? ClientId { get; set; }
        public UserDto Client { get; set; }
        public int? CourierId { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
        public decimal? TotalValue => OrderDetails.Sum(x => x.Product.Price * x.Amount);
        public IList<OrderDetailsDto> OrderDetails { get; set; }
    }
}
