using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Abstraction
{
    public interface ICheckoutService
    {
        ApplicationUser GetUserDetails(string userId);
        bool ProcessCheckout(string userId, string fullName, string address, string email, string secondAddress, string country, string city, string zipCode, string phoneNumber, string paymentMethod);
       bool SaveCheckout(string userId, string fullname, string address, string email, string address2, string country, string city,
            string zip, string phone, string pay);
        public CheckoutFormVM PrepareCheckoutForm(string userId);
    }
}
