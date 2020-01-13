using Wholesale.BL.Enums;

namespace Wholesale.BL.Models.Query
{
    public class UpdateOrderStatusQuery
    {
        public int OrderId { get; set; }
        public int CourierId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
