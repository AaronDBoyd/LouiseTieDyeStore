
using LouiseTieDyeStore.Shared;
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

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                Product = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.Images[0].Url
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
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                Product = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and" +
                    $" {o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.Images[0].Url
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
