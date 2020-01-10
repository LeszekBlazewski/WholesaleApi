namespace Wholesale.BL.Models.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public ProductCategoryDto Category { get; set; }
    }
}
