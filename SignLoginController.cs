using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestApi.Data;
using TestApi.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [ApiController]
    public class SignLoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public SignLoginController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: /<controller>/
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] Demosignup model)
        {
            SignUpDetails obj = new SignUpDetails();
            obj.Email = model.Email;
            obj.Password = model.Password;
            obj.IsActive = model.IsActive;
            _context.SignUpDetails.Add(obj);
            _context.SaveChanges();
            return Ok("Add Successfully");
        }

        //[HttpPost("ChangePw")]
        //public async Task<IActionResult> ChangePw([FromBody] SignUpDetails model)
        //{
            
        //    _context.SignUpDetails.Add(model);
        //    _context.SaveChanges();
        //    return Ok("Add Successfully");
        //}

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] DemoLogin model)
        {
            var obj = _context.SignUpDetails.FirstOrDefault(x => x.Email == model.Email);
            if (obj.Email == model.Email && obj.Password == model.Password)
            {
                obj.IsActive = true;
                _context.SignUpDetails.Update(obj);
                _context.SaveChanges();
                var token = GenerateJwtToken(model.Email);
                
                // Return the token in the response
                return Ok(new { Token = token });
            }
            else
            {
                // SignUpDetails with the specified email was not found
                return Unauthorized();
                // This will set the HTTP response status code to 404
            }




        }
        [HttpPost("GenerateJwtToken")]
        public string GenerateJwtToken(string email)
        {
            string secretKey = _configuration["AppSettings:SecretKey"];
            var key = Encoding.UTF8.GetBytes(secretKey); // Same secret key as used in the configuration
            var issuer = _configuration["AppSettings:Issuer"]; // Set your issuer
            var audience = _configuration["AppSettings:Audience"]; // Set your audience
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
        // Add additional claims as needed
            };

            var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        [HttpGet("CategoryGetByEmail")]
        public IActionResult GetCategoryByEmail([FromQuery] string email)
        {
            // Your logic to retrieve category using the email
            // ...
            var obj = _context.SignUpDetails.FirstOrDefault(x => x.Email == email);
            return Ok(obj);
        }


        [HttpGet("Logout")]
        public IActionResult GetCategoryByEmailLogout([FromQuery] string email)
        {
            // Your logic to retrieve category using the email
            // ...
            var obj = _context.SignUpDetails.FirstOrDefault(x => x.Email == email);
            obj.IsActive = false;
            _context.SignUpDetails.Update(obj);
            _context.SaveChanges();
            return Ok("Add Successfully");
        }

        [HttpPost("ChangePw")]
        public IActionResult ChangePass([FromBody] ChangePas model)
        {
            try
            {
                var obj = _context.SignUpDetails.FirstOrDefault(x => x.Email == model.Email);
                if (obj != null && model.NewPassword == model.ConfirmPassword && model.CurrentPassword == obj.Password)
                {
                    obj.Password = model.NewPassword;
                    _context.SignUpDetails.Update(obj);
                    _context.SaveChanges();
                    return Ok("Password changed successfully");
                }
                else
                {
                    return NotFound("Invalid email or password mismatch");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

