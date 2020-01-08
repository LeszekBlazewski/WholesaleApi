namespace Wholesale.BL.Models
{
    public class DeliveryDetails
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Delivery Delivery { get; set; }
        public virtual Product Product { get; set; }
    }
}
