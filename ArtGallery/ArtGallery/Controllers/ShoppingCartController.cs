using ArtGallery.Core.Abstraction;
using ArtGallery.Models.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtGallery.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public ShoppingCartController(IShoppingCartService cartService, IOrderService orderService, IProductService productService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _productService = productService;
        }
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.AddToCart(productId, currentUserId, quantity);
            return RedirectToAction("Index");
        }
        public ActionResult PlaceOrder()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _cartService.GetShoppingCartByUser(currentUserId);

            // Create order from cart items
            _orderService.CreateOrderFromCart(cartItems);
            // Clear the shopping cart
            _cartService.ClearCart(currentUserId);
            // Redirect to order success page or home page
            return RedirectToAction("OrderSuccess");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int cartItemId)
        {
            _cartService.RemoveFromCart(cartItemId);


            return RedirectToAction("Index");
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCart(int cartItemId, int quantity)
        {
            try
            {
                _cartService.UpdateCart(cartItemId, quantity);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error updating cart.");
            }
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _cartService.GetShoppingCartByUser(currentUserId);
            return View("Index", cartItems);
        }
        // GET: ShoppingCartController
        public ActionResult Index()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);



            List<ShoppingCartIndexVM> cartItems = _cartService.GetShoppingCartByUser(currentUserId)
           .Select(item => new ShoppingCartIndexVM
           {
               Id = item.Id,
               ProductName = item.Product.Name,
               Quantity = item.Quantity,
               QuantityInStock = item.Product.Quantity,
               Price = item.Product.Price,
               Discount = item.Product.Discount,
               UserId = currentUserId,



           }).ToList();
            return View(cartItems);

        }

        // GET: ShoppingCartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
