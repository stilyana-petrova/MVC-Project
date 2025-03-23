using ArtGallery.Core.Abstraction;
using ArtGallery.Core.Services;
using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Wishlist;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtGallery.Controllers
{
    public class WishListController : Controller
    {
        private readonly IWishlistService _wishListService;
        private readonly IProductService _productService;

        public WishListController(IWishlistService wishListService, IProductService productService)
        {
            _wishListService = wishListService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishList = _wishListService.GetWishListByUserId(currentUserId);
            if (wishList.IsNullOrEmpty()) return NoContent();

            var model = wishList.Select(w => new WishlistDTO
            {
                Id = w.Id,
                ProductId = w.Product.Id,
                ProductName = w.Product.Name,
                Picture = w.Product.Picture,
                Price = w.Product.Price
            }).ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult Add(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            _wishListService.AddToWishList(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _wishListService.RemoveFromWishList(currentUserId, productId);
            if (currentUserId == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
