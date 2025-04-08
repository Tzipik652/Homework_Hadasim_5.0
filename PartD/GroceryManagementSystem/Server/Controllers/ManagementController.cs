using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("GroceryManagementSystem/[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IBL _bl;

        public ManagementController(IBL bl)
        {
            _bl = bl;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null)
                return BadRequest("Invalid order data");

            if (request.Quantity <= 0)
                return BadRequest("Quantity must be greater than 0");

            try
            {
                var newOrder = new BLOrder
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    StatusId = 1,
                    OrderDate = DateTime.Now
                };

                await _bl.Order.OrderingGoodsFromSupplierAsync(newOrder); 

                return Ok("Order created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _bl.Products.GetAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("all-orders")]
        public async Task<ActionResult<List<BLOrder>>> GetOrders()
        {
            try
            {
                var orders = await _bl.Order.GetAsync();

               

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }
        [HttpPut("complete-order/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            try
            {
                await _bl.Manager.OrderCompletion(orderId);
                return Ok("Order confirmed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
