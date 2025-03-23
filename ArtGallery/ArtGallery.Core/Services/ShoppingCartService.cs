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
    public class ShoppingCartService:IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddToCart(int productId, string userId, int quantity)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return false;
            }

            var cartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UserId = userId
                };
                _context.ShoppingCartItems.Add(cartItem);
            }

            return _context.SaveChanges() != 0;
        }


        public bool ClearCart(string userId)
        {
            var items = _context.ShoppingCartItems.Where(x => x.UserId == userId);
            foreach (var item in items)
            {
                _context.ShoppingCartItems.Remove(item);
            }

            return _context.SaveChanges() != 0;
        }

        public List<ShoppingCartItem> GetShoppingCartByUser(string userId)
        {
            return _context.ShoppingCartItems.Where(x => x.UserId == userId).ToList();
        }



        public bool RemoveFromCart(int cartItemId)
        {
            var cartItem = _context.ShoppingCartItems.FirstOrDefault(item => item.Id == cartItemId);
            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);

            }
            return _context.SaveChanges() != 0;
        }

        public bool UpdateCart(int cartItemId, int quantity)
        {
            var cartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.Id == cartItemId);
            if (cartItem == default(ShoppingCartItem))
            {
                return false;
            }

            var product = _context.Products.FirstOrDefault(x => x.Id == cartItem.ProductId);
            if (product == null)
            {
                return false;
            }

            if (quantity <= 0)
            {
                return false;
            }
            else if (quantity > product.Quantity)
            {
                return false;
            }
            else
            {
                cartItem.Quantity = quantity;
            }
            _context.Update(cartItem);
            return _context.SaveChanges() != null;
        }
    }
}
