
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly ISalesTaxService _taxService;

        public OrderService(DataContext context, 
            ICartService cartService, 
            IAuthService authService, 
            ISalesTaxService taxService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
            _taxService = taxService;
        }

        public async Task<ServiceResponse<string>> GetLastOrderIdByUserEmail(string email)
        {
            var lastOrder = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync(o => o.Email == email);

            if (lastOrder != null)
            {
                return new ServiceResponse<string>
                {
                    Data = lastOrder.Id.ToString()
                };
            }
            else
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Order Not Found"
                };
            }         
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(Order order)
        {
        
            // get products from cartitems
            var cartProducts = (await _cartService.GetDbCartProducts(order.Email)).Data;

            // set products as Sold
            var productIdList = cartProducts.Select(cp => cp.ProductId).ToList();

            var dbProducts = await _context.Products
                .Where(p => productIdList.Contains(p.Id)).ToListAsync();

            dbProducts.ForEach(p => p.Sold = true);

            // set list of orderItems
            var orderItems = new List<OrderItem>();
            cartProducts.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                Price = product.Price
            }));

            //OrderItems to order
            order.OrderItems = orderItems;

            // Calculate SubTotal
            order.SubTotal = order.OrderItems.Sum(o => o.Price);

            // Calculate Sales Tax
            var rate = (await _taxService.GetTaxRate(order.Address.State)).Data / 100;
            order.SalesTax = Math.Round(order.SubTotal * rate, 2);

            // add UserId  
            User user = new User();

            if (await _authService.UserExists(order.Email))
            {
                user = await _authService.GetUserByEmail(order.Email);
            }
            else
            {
                user.Email = order.Email;
                await _authService.Register(user);
                user = await _authService.GetUserByEmail(order.Email);
            }

            order.UserId = user.Id;

            // add order to db
            _context.Orders.Add(order);

            // remove cartItems from db
            _context.CartItems.RemoveRange(_context.CartItems
                .Where(ci => ci.UserEmail == order.Email));


            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
