using BL.Api;
using BL.Models;
using BL.Services;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("GroceryManagementSystem/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly IBL _bl;

        public SuppliersController(IBL bl)
        {
            _bl = bl;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] BLProduct product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                await _bl.Products.CreateAsync(product);
                return Ok("Product created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] BLProduct product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                await _bl.Products.UpdateAsync(product);
                return Ok("Product updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var product = await _bl.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                await _bl.Products.DeleteAsync(product);
                return Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      

      

        [HttpGet("get-product-by-supplier/{supplierId}")]
        public async Task<IActionResult> GetProductsBySupplierId(int supplierId)
        {
            try
            {
                var products = await _bl.Products.GetProductsBySupplierIdAsync(supplierId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
      
        [HttpGet("all-orders/{supplierId}")]
        public async Task<ActionResult<List<BLOrder>>> GetOrders(int supplierId)
        {
            try
            {
                var orders = await _bl.Order.GetOrdersBySupplierIdAsync(supplierId);


                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }
        [HttpPut("confirm-order/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            try
            {
                await _bl.Supplier.OrderConfirmation(orderId);
                return Ok("Order confirmed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
