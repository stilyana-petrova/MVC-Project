using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // GET: Checkout
        public IActionResult Index()
        {

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // var model = new CheckoutFormVM();
            var model = _checkoutService.PrepareCheckoutForm(currentUserId);
            if (!string.IsNullOrEmpty(currentUserId))
            {
                var user = _checkoutService.GetUserDetails(currentUserId);
                if (user != null)
                {
                    model.FullName = $"{user.FirstName} {user.LastName}";
                    model.Address = user.Address;
                    model.Email = user.Email;
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm] CheckoutFormVM model)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var isSaved = _checkoutService.SaveCheckout(
               currentUserId,
               model.FullName,
               model.Address,
               model.Email,
               model.Address2,
               model.Country,
               model.City,
               model.ZipCode,
               model.PhoneNumber,
               model.PaymentMethod
                );

                if (isSaved)
                {
                    return RedirectToAction("PlaceOrder", "ShoppingCart");
                }

            }
            return View();
        }



        // GET: Checkout/Success
        public IActionResult Success()
        {
            return View();
        }

    }
}
