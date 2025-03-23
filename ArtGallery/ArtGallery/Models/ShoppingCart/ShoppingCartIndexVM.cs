namespace ArtGallery.Models.ShoppingCart
{
    public class ShoppingCartIndexVM
    {
        public int Id { get; set; }


        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public int QuantityInStock { get; set; }


        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }


        public string UserId { get; set; } = null!;


        public decimal Total => Quantity * Price - Quantity * Price * Discount / 100;
    }
}
