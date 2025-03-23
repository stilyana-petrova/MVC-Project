using ArtGallery.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Abstraction
{
    public interface IShoppingCartService
    {
        bool AddToCart(int productId, string userId, int quantity);
        bool RemoveFromCart(int cartItemId);

        List<ShoppingCartItem> GetShoppingCartByUser(string userId);

        bool ClearCart(string userId);

        bool UpdateCart(int cartItemId, int quantity);

    }
}
