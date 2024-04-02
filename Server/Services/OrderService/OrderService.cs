
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        public async Task<ServiceResponse<bool>> PlaceOrder(Order order)
        {
            Console.WriteLine("!!!!Order: " + JsonConvert.SerializeObject(order));


            // get cartItems by email

            // get products from cartitems

            // set products as Sold

            // set list of orderItems

            // add Guid, UserId and OrderItems to order 

            // add order to db

            // remove cartItems from db


            return new ServiceResponse<bool> { Data = true };
        }
    }
}
