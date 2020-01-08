using System.Collections.Generic;

namespace Wholesale.BL.Models
{
    public class Product
    {
        public Product()
        {
            DeliveryDetails = new HashSet<DeliveryDetails>();
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<DeliveryDetails> DeliveryDetails { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
