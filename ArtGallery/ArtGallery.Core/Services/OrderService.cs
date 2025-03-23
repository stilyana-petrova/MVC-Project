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
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;


        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(int productId, string userId, int quantity)
        {
            //намираме продукта по неговото id
            var product = this._context.Products.SingleOrDefault(x => x.Id == productId);

            //проверяваме дали има  такъв продукт
            if (product == null)
            {
                return false;
            }

            //създаване на поръчка
            Order item = new Order
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount,
            };

            //намаляване на количеството на продукта
            product.Quantity -= quantity;

            //отразяване на промените в колекциите
            this._context.Products.Update(product);
            this._context.Orders.Add(item);

            //запис на промените в БД
            return _context.SaveChanges() != 0;
        }

        public bool CreateOrderFromCart(List<ShoppingCartItem> cartItems)
        {
            // Create order for each cart item
            foreach (var item in cartItems)
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    ProductId = item.ProductId,
                    UserId = item.UserId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    Discount = item.Product.Discount,
                };
                _context.Orders.Add(order);
                // Update product quantity
                var product = _context.Products.Find(item.ProductId);
                product.Quantity -= item.Quantity;
                _context.Products.Update(product);
            }
            return _context.SaveChanges() != null;

        }

        public Order GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToList();
        }

        public bool RemoveById(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool Update(int orderId, int productId, string userId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
