namespace ArtGallery.Models.Wishlist
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
    }
}
