namespace Wholesale.BL.Models.Views
{
    public class ProductStatsV
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? CurrentPrice { get; set; }
        public long? NumberSold { get; set; }
    }
}
