using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.Model;

namespace TestApi.Controllers
{
    [ApiController]
    //[Authorize]
    public class CategoriesController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Category")]
        public JsonResult Category()
        {
            var cat = _context.Categorriees.ToList();

            return new JsonResult(cat);
        }

        [HttpGet("CategoryActive")]
        public JsonResult CategoryActive()
        {
            var cat = _context.Categorriees.Where(c => c.Status).ToList(); 

            return new JsonResult(cat);
        }


        [HttpGet("CategoryGet")]
        public JsonResult CategoryById(int Id)
        {
            var cat = _context.Categorriees.Where(e => e.Id == Id).ToList();
            return new JsonResult(cat);
        }



        [HttpGet("GetCategoryBySubcategoryid")]
        public JsonResult GetCategoryIdByPid(int Id)
        {
            var categories = _context.SubCategories
    .Where(e => e.Id == Id)
    .Select(e => e.Category)
    .ToList();
            return new JsonResult(categories);
        }


        [HttpGet("SubCategoryGet")]
        public JsonResult SubCategoryGet(int Id)
        {

            var cat = _context.SubCategories.Include(sc => sc.Category).FirstOrDefault(s => s.Id == Id);
            return new JsonResult(cat);
        }


        [HttpPost("CategoryPost")]
        public async Task<IActionResult> AddCategories([FromBody] Categories model)
        {
            _context.Categorriees.Add(model);
            _context.SaveChanges();
            return Ok("Saved Successfully");
        }

        
        [HttpPost("EditCategory")]
        public async Task<IActionResult> EditCategories([FromBody] Categories model)
        {
            _context.Categorriees.Update(model);
            _context.SaveChanges();
            return Ok("Update Successfully");
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var details = _context.Categorriees.Find(id);

            var subcategoriesForCategory = _context.SubCategories.Where(sub => sub.Category.Id == id)
    .ToList();
            if (subcategoriesForCategory.Count == 0) { 
            _context.Categorriees.Remove(details);
            _context.SaveChanges();
            }
            return Ok(details);
        }

        [HttpGet("DeleteSub")]
        public async Task<IActionResult> DeleteSub(int id)
        {
            var details = _context.SubCategories.Find(id);
            _context.SubCategories.Remove(details);
            _context.SaveChanges();

            return Ok(details);
        }

        


        [HttpPost("SubCategoryPost")]
        public async Task<IActionResult> AddSubCategories([FromBody] DemoSubCategory model)
        {
            try
            {
                var cid = Convert.ToInt32(model.Categoryy);
                var details = _context.Categorriees.Find(cid);

                // Create a new SubCategories instance and set its properties
                SubCategories subCategories = new SubCategories()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Status = model.Status,
                    Category = details
                };

                // Add the new SubCategories instance to the context
                _context.SubCategories.Add(subCategories);

                // Save changes to the database
                _context.SaveChanges();

                return Ok("Saved Successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("EditSubCategory")]
        public async Task<IActionResult> EditSubCategory([FromBody] DemoSubCategory model)
        {
            var cid = Convert.ToInt32(model.Categoryy);
            var details = _context.Categorriees.Find(cid);

            // Create a new SubCategories instance and set its properties
            SubCategories subCategories = new SubCategories()
            {
                Id = model.Id,
                Name = model.Name,
                Status = model.Status,
                Category = details
            };
            _context.SubCategories.Update(subCategories);
            _context.SaveChanges();
            return Ok("Update Successfully");
        }

        

        [HttpGet("SubCategory")]
        public JsonResult SubCategory()
        {
            var cat = _context.SubCategories.Include(sc => sc.Category).ToList();

            return new JsonResult(cat);
        }


        [HttpGet("SubCategoryGetByCid")]
        public JsonResult SubCategoryGetByCid(int Id)
        {

            var cat = _context.SubCategories.Include(sc => sc.Category).Where(s => s.Category.Id == Id ).ToList();
            return new JsonResult(cat);
        }


        [HttpGet("SubCategoryGetByCidActive")]
        public JsonResult SubCategoryGetByCidActive(int Id)
        {
            var subcategories = _context.SubCategories
                .Include(sc => sc.Category)
                .Where(s => s.Category.Id == Id && s.Status == true) // Add condition for Status
                .ToList();

            return new JsonResult(subcategories);
        }


    }
}

