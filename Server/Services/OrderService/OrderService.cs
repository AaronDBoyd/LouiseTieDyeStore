
using LouiseTieDyeStore.Shared;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Security.Cryptography;

namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly ISalesTaxService _taxService;
        private readonly IConfiguration _configuration;

        public OrderService(DataContext context,
            ICartService cartService,
            IAuthService authService,
            ISalesTaxService taxService,
            IConfiguration configuration)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
            _taxService = taxService;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> ChangeOrderStatus(Guid orderId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            switch (status)
            {
                case "Placed":
                    order.Status = Status.Placed;
                    break;
                case "Packed":
                    order.Status = Status.Packed;
                    break;
                case "Shipped":
                    order.Status = Status.Shipped;
                    break;
                case "Delivered":
                    order.Status = Status.Delivered;
                    break;
                default:
                    break;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Data = order.Status.ToString()
            };
        }

        public async Task<ServiceResponse<Order>> GetAdminOrder(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                return new ServiceResponse<Order>
                {
                    Data = order
                };
            }
            else
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Order Not Found"
                };
            }
        }

        public async Task<ServiceResponse<OrderPageResults>> GetAdminOrders(int page, string? statusFilter = null, bool orderByNewest = true)
        {
            var pageResults = 10f;

            int count;

            if (statusFilter == null)
            {
                count = await _context.Orders
                    .CountAsync();
            }
            else
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                count = await _context.Orders
                    .Where(o => o.Status == status)
                    .CountAsync();
            }

            if (count == 0)
            {
                return new ServiceResponse<OrderPageResults>
                {
                    Success = false,
                    Message = "No Orders Found"
                };
            }

            var pageCount = Math.Ceiling(count / pageResults);

            List<Order> orders = new List<Order>();

            if (statusFilter == null && orderByNewest)
            {
                orders = await _context.Orders
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter == null && !orderByNewest)
            {
                orders = await _context.Orders
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .OrderBy(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter != null && orderByNewest)
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                orders = await _context.Orders
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.Status == status)
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter != null && !orderByNewest)
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                orders = await _context.Orders
                    .Include(o => o.Address)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.Status == status)
                    .OrderBy(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }

            if (orders.Count == 0)
            {
                return new ServiceResponse<OrderPageResults>
                {
                    Success = false,
                    Message = "No Orders Found"
                };
            }

            Console.WriteLine("!!! Orders:" + JsonConvert.SerializeObject(orders));

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = TimeZoneInfo.ConvertTime(o.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles")),
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                OrderTitle = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                OrderImageUrl = o.OrderItems.First().Product.Images.First(i => i.IsMainImage).Url,
                CustomerName = $"{o.Address.FirstName} {o.Address.LastName}",
            }));

            return new ServiceResponse<OrderPageResults>
            {
                Data = new OrderPageResults
                {
                    Orders = orderResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
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

        public async Task<ServiceResponse<Order>> GetOrder(Guid orderId)
        {
            var email = await _authService.GetUserEmail();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.Email == email);

            if (order != null)
            {
                return new ServiceResponse<Order>
                {
                    Data = order
                };
            }
            else
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Order Not Found"
                };
            }
        }

        public async Task<ServiceResponse<OrderPageResults>> GetOrders(int page, string? statusFilter = null, bool orderByNewest = true)
        {
            var userId = await _authService.GetUserId();
            var pageResults = 10f;           
            int count;

            if (statusFilter == null)
            {
                count = await _context.Orders
                    .Where(o => o.UserId == userId).CountAsync();
            }
            else
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                count = await _context.Orders
                    .Where(o => o.UserId == userId &&
                    o.Status == status).CountAsync();
            }

            if (count == 0)
            {
                return new ServiceResponse<OrderPageResults>
                {
                    Success = false,
                    Message = "No Orders Found"
                };
            }

            var pageCount = Math.Ceiling(count / pageResults);

            List<Order> orders = new List<Order>();

            if (statusFilter == null && orderByNewest)
            {
                orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter == null && !orderByNewest)
            {
                orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.UserId == userId)
                    .OrderBy(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter != null && orderByNewest)
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.UserId == userId && o.Status == status)
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else if (statusFilter != null && !orderByNewest)
            {
                Status status = (Status)Enum.Parse(typeof(Status), statusFilter);

                orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Images)
                    .Where(o => o.UserId == userId && o.Status == status)
                    .OrderBy(o => o.OrderDate)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }

            if (orders.Count == 0)
            {
                return new ServiceResponse<OrderPageResults>
                {
                    Success = false,
                    Message = "No Orders Found"
                };
            }

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = TimeZoneInfo.ConvertTime(o.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles")),
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                OrderTitle = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                OrderImageUrl = o.OrderItems.First().Product.Images.First(i => i.IsMainImage).Url
            }));

            return new ServiceResponse<OrderPageResults>
            {
                Data = new OrderPageResults
                {
                    Orders = orderResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
        }

        public async Task<ServiceResponse<List<string>>> GetOrderSearchSuggestions(string searchText)
        {
            var orders = await _context.Orders
                .Where(o => o.Email.ToLower().Contains(searchText.ToLower()) 
                || o.Id.ToString().ToLower().Contains(searchText.ToLower())
                || o.Address.FirstName.ToLower().Contains(searchText.ToLower())
                || o.Address.LastName.ToLower().Contains(searchText.ToLower()))
                .Include(o => o.Address).ToListAsync();

            List<string> result = new();

            if (orders != null)
            {
                foreach (var order in orders)
                {
                    if (order.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        && !result.Contains(order.Email))
                    {
                        result.Add(order.Email);
                    }

                    string fullName = $"{order.Address.FirstName} {order.Address.LastName}";

                    if (!result.Contains(fullName) 
                        && order.Address.FirstName.ToLower().Contains(searchText.ToLower())
                        || order.Address.LastName.ToLower().Contains(searchText.ToLower()))
                    {
                        result.Add(fullName);
                    }

                    if (order.Id.ToString().ToLower().Contains(searchText.ToLower())
                        && !result.Contains($"{order.Id}"))
                    {
                        result.Add($"{order.Id}");
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(Order order)
        {
            try
            {
                // get products from cartitems
                var cartProducts = (await _cartService.GetDbCartProducts(order.Email)).Data;

                if (cartProducts.Count == 0 || cartProducts == null)
                {
                    return new ServiceResponse<bool> { Data = false, Success = true, Message = "Cart Items already removed. Square Webhook must have fired multiple times" };
                }

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
                //var rate = (await _taxService.GetTaxRate(order.Address.State)).Data / 100;
                //order.SalesTax = Math.Round(order.SubTotal * rate, 2);

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

                Console.WriteLine("!!! Order: " + JsonConvert.SerializeObject(order));

                // add order to db
                _context.Orders.Add(order);

                // remove cartItems from db
                _context.CartItems.RemoveRange(_context.CartItems
                    .Where(ci => ci.UserEmail == order.Email));


                await _context.SaveChangesAsync();

                await SendOrderNotification(order);

                return new ServiceResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!! Exception: " + ex.Message);

                return new ServiceResponse<bool> { Data = false, Success = true, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<OrderPageResults>> SearchOrders(string searchText, int page)
        {
            var pageResults = 10f;
            var count = await _context.Orders
                .Where(o => o.Email.ToLower().Contains(searchText.ToLower())
                || o.Id.ToString().ToLower().Contains(searchText.ToLower())
                || o.Address.FirstName.ToLower().Contains(searchText.ToLower())
                || o.Address.LastName.ToLower().Contains(searchText.ToLower())
                || (o.Address.FirstName + " " + o.Address.LastName).ToLower() == searchText.ToLower())
                .Include(o => o.Address).CountAsync();

            var pageCount = Math.Ceiling(count / pageResults);

            var orders = await _context.Orders
                .Where(o => o.Email.ToLower().Contains(searchText.ToLower())
                || o.Id.ToString().ToLower().Contains(searchText.ToLower())
                || o.Address.FirstName.ToLower().Contains(searchText.ToLower())
                || o.Address.LastName.ToLower().Contains(searchText.ToLower())
                || (o.Address.FirstName + " " + o.Address.LastName).ToLower() == searchText.ToLower())
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();

            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = TimeZoneInfo.ConvertTime(o.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles")),
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                OrderTitle = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                OrderImageUrl = o.OrderItems.First().Product.Images.First(i => i.IsMainImage).Url,
                CustomerName = $"{o.Address.FirstName} {o.Address.LastName}",
            }));

            return new ServiceResponse<OrderPageResults>
            {
                Data = new OrderPageResults
                {
                    Orders = orderResponse,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
        }

        // SendGrid.com
        private async Task SendOrderNotification(Order order)
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGrid_ApiKey") ?? _configuration["SendGrid_ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(Environment.GetEnvironmentVariable("SendGrid_Email") ?? _configuration["SendGrid_Email"], "Z Creates");
            var to = new EmailAddress(Environment.GetEnvironmentVariable("SendGrid_Email") ?? _configuration["SendGrid_Email"], "Z Creates Admin");

            var subject = $"Order Placed for {order.OrderItems.Count} items";
            var plainTextContent = ""; // TODO: Should I add this?
            var htmlContent = $"<p>{@TimeZoneInfo.ConvertTime(order.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles"))}</p><br />"
                    + "<h3>Customer: </h3>"
                    + $"<p><strong>Name:</strong> {order.Address.FirstName} {order.Address.LastName}</p>"
                    + $"<p><strong>Email:</strong> {order.Email}</p>"
                    + $"<p><strong>Phone:</strong> {order.Address.PhoneNumber}</p>"
                    + $"<p><strong>Address:</strong> {order.Address.LineOne}, {order.Address.LineTwo}, {order.Address.City}, {order.Address.State}, {order.Address.Zip}</p>"
                    + $"<h3>Items Sold: <h3>";
            foreach(var item in order.OrderItems)
            {
                htmlContent += $"<p><strong>{item.Product.Title} - ${item.Price}</strong></p>";
            }
            htmlContent += $"<br /><p><strong>Subtotal:</strong> ${order.SubTotal}</p>"
                + $"<p><strong>Taxes:</strong> ${order.SalesTax}</p>"
                + $"<p><strong>Shipping:</strong> ${order.ShippingCost}</p>"
                + $"<p><strong>Total:</strong> ${order.TotalPrice}</p>"
                + "<br /><a href=\"https://tiedyestore.onrender.com/admin/orders/1\"><strong>View Orders</strong></a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            //Console.WriteLine(JsonConvert.SerializeObject(response));
        }
    }
}
