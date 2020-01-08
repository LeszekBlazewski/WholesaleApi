namespace Wholesale.BL.Models
{
    public class Address
    {
        public int UserId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string AddressDetails { get; set; }

        public virtual User User { get; set; }
    }
}
