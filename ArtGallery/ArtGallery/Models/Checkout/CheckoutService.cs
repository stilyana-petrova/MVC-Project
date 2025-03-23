using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Checkout;
using ArtGallery.Models.ShoppingCart;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ApplicationDbContext _context;

        public CheckoutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUserDetails(string userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }

        public bool ProcessCheckout(string userId, string fullName, string address, string email, string secondAddress, string country, string city, string zipCode, string phoneNumber, string paymentMethod)
        {
           
            return true;
        }
        public bool SaveCheckout(string userId, string fullname, string address, string email, string address2, string country, string city,
            string zip, string phone, string pay)
        {
            Checkout checkout = new Checkout
            {
                UserId = userId,
                FullName = fullname,
                Address = address,
                Email = email,
                Address2 = address2,
                Country = country,
                City = city,
                ZipCode = zip,
                PhoneNumber = phone,
                PaymentMethod = pay,
                CheckoutDate = DateTime.Now
            };

            _context.Checkouts.Add(checkout);
            return _context.SaveChanges() > 0;
        }
        public CheckoutFormVM PrepareCheckoutForm(string userId)
        {
            var user = GetUserDetails(userId);

            var cartItems = _context.ShoppingCartItems
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemVM
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Picture = c.Product.Picture,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                })
                .ToList();

            return new CheckoutFormVM
            {
                FullName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                Address = user?.Address,
                Email = user?.Email,
                Products = cartItems
            };
        }

    }
}
