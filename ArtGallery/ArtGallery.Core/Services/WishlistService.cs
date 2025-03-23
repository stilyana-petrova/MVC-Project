using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool AddToWishList(string userId, int productId)
        {
            if (_context.Wishlists.Any(w => w.UserId == userId && w.ProductId == productId))
                return false;

            var product = _context.Products.SingleOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return false;
            }
            var wishItem = _context.Wishlists.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

             wishItem = new Wishlist
            {
                UserId = userId,
                ProductId = productId
            };

            _context.Wishlists.Add(wishItem);
            return _context.SaveChanges() > 0;
        }

        public List<Wishlist> GetWishListByUserId(string userId)
        {
            return _context.Wishlists.Where(w => w.UserId == userId).ToList();

        }

        public bool RemoveFromWishList(string userId, int productId)
        {
            var item = _context.Wishlists.FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);
            if (item == null) return false;

            _context.Wishlists.Remove(item);
            return _context.SaveChanges() > 0;
        }
    }
}
