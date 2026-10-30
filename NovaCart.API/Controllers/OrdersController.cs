using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaCart.Application.DTOs;
using NovaCart.Application.Interfaces;
using NovaCart.Domain.Entities;

namespace NovaCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 1. جلب كل الطلبات (GET: api/orders) - يمكن جعله متاحاً للمديرين فقط لاحقاً
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        // 2. جلب طلب بالـ ID (GET: api/orders/{id})
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        // 3. إنشاء طلب جديد (POST: api/orders) - محمي ويتطلب تسجيل دخول
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null || createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
            {
                return BadRequest(new { message = "Invalid order data. Order must contain at least one item." });
            }

            // استخراج معرف المستخدم الحالي من الـ JWT Token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _mapper.Map<Order>(createOrderDto);
            order.UserId = userId;
            order.OrderStatus = "Pending";
            order.CreatedAt = DateTime.UtcNow;

            decimal totalAmount = 0;
            order.OrderItems.Clear();

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    return BadRequest(new { message = $"Product with ID {itemDto.ProductId} not found." });
                }

                if (product.StockQuantity < itemDto.Quantity)
                {
                    return BadRequest(new { message = $"Insufficient stock for product: {product.Name}" });
                }

                // خصم الكمية من مخزون المنتج
                product.StockQuantity -= itemDto.Quantity;
                _unitOfWork.Products.Update(product);

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Quantity = itemDto.Quantity
                };

                order.OrderItems.Add(orderItem);
                totalAmount += product.Price * itemDto.Quantity;
            }

            order.TotalAmount = totalAmount;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var orderDto = _mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetById), new { id = orderDto.Id }, orderDto);
        }

        // 4. تحديث حالة الطلب فقط (PUT: api/orders/{id}/status) - مخصص للمشرفين
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            order.OrderStatus = newStatus;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Order status updated to '{newStatus}' successfully" });
        }

        // 5. إلغاء الطلب (DELETE أو Cancel: api/orders/{id}/cancel) - بديل آمن للحذف
        [Authorize]
        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            if (order.OrderStatus == "Cancelled")
            {
                return BadRequest(new { message = "Order is already cancelled." });
            }

            // إعادة المنتجات للمخزون عند إلغاء الطلب
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += item.Quantity;
                        _unitOfWork.Products.Update(product);
                    }
                }
            }

            order.OrderStatus = "Cancelled";
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Order cancelled successfully and stock restored" });
        }
    }
}