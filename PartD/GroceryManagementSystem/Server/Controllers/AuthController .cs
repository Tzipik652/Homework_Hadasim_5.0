using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("GroceryManagementSystem/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IBL _bl;

        public AuthController(IBL bl)
        {
            _bl = bl;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSupplierAsync([FromBody] RegisterRequest request)
        {
            try
            {
                await _bl.Auth.RegisterSupplierAsync(request.Username, request.Password, request.SupplierDetails);
                return Ok("Supplier registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("register-manager")]
        public async Task<IActionResult> RegisterManagerAsync([FromBody] RegisterManagerRequest request)
        {
            try
            {
                await _bl.Auth.RegisterManagerAsync(request.Username, request.Password);
                return Ok("Manager registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _bl.Auth.LoginAsync(request.Username, request.Password);
                return Ok(new { Role = result.Role, Id = result.Id }); 
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid username or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public BLSupplier SupplierDetails { get; set; }
    }
    public class RegisterManagerRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    
    }
}
