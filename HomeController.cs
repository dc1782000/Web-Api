using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;
using TestApi.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: /<controller>/
        //[HttpPost("SignUp")]
        //public async Task<IActionResult> Add([FromBody] SignUpDetails model)
        //{
        //    SignUpDetails employee = new SignUpDetails()
        //    {
        //        Email = model.Email,
        //        Password = model.Password
        //    };
        //    _context.SignUpDetails.Add(employee);
        //    _context.SaveChanges();
        //    return Ok("Saved Successfully");
        //}

        //[HttpGet("getcategories")]
        //public async Task<IActionResult> Categories()
        //{
        //    IEnumerable<Categories> objDetailsList = _context.Categorriees!;
        //    return Ok(objDetailsList);
        //}
    }
}

