using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TestApi.Data;
using TestApi.Model;

namespace TestApi.Controllers
{
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("PostProduct")]
        public async Task<IActionResult> PostProduct([FromBody] WholeProduct model)
        {
            Products products = new Products();


            int id = Convert.ToInt16(model.Category);
            var details = _context.Categorriees.FirstOrDefault(x => x.Id == id);

            products.Category = details;


            if (model.PdfHeadings != null && model.PdfName != null)
            {
                for (int i = 0; i < model.PdfHeadings.Count && i < model.PdfName.Count; i++)
                {
                    PdfDetails pdfDetails = new PdfDetails
                    {
                        PDFHeading = model.PdfHeadings[i],
                        PDFName = model.PdfName[i]
                    };

                    products.PdfDetailss.Add(pdfDetails);
                }
            }


            foreach (var item1 in model.SubCategoriess)
            {

                ProductSubCategory productSubCategory = new ProductSubCategory();
                int ids = Convert.ToInt16(item1);
                var detailss = _context.SubCategories.FirstOrDefault(x => x.Id == ids);
                productSubCategory.Subcategory = detailss;
                products.ProductSubCategorys.Add(productSubCategory);
            }
            foreach (var item1 in model.Titles)
            {
                if (item1 != null)
                {
                    Title title = new Title();
                    title.Name = item1;
                    products.Title.Add(title);
                    _context.SaveChanges();
                }
            }
            foreach (var item1 in model.Headings)
            {
                if (item1 != null)
                {
                    Heading heading = new Heading();
                    heading.Name = item1;
                    products.Heading.Add(heading);
                    _context.SaveChanges();
                }
            }

            foreach (var item1 in model.Descriptions)
            {
                if (item1 != null)
                {
                    Description description = new Description();
                    description.Name = item1;
                    products.Description.Add(description);
                    _context.SaveChanges();
                }
            }
            products.Id = model.Id;
            products.Name = model.Name;
            products.ShortDescription = model.Shortdecription;
            products.Status = model.Statuss;
            products.PictureName=model.PictureName;
            products.Content = model.Content;
            _context.Products.Add(products);
            _context.SaveChanges();
            return Ok();
        }



        [HttpGet("PdfDetailsByProduct")]
        public async Task<IActionResult> PdfDetailsByProduct(int Id)
        {
            var pdfDetailsList = _context.Products.Where(product => product.Id == Id)
                                    .SelectMany(product => product.PdfDetailss)
                                    .ToList();



            return new JsonResult(pdfDetailsList);
        }


            [HttpPost("PostEditProduct1")]
        public async Task<IActionResult> PostEditProductsssss([FromBody] ProductData model)
        {
           
            Products product3 = _context.Products
                .Include(p => p.ProductSubCategorys).ThenInclude(ps => ps.Subcategory)
                .Include(p => p.Title)
                .Include(p => p.Heading)
                .Include(p => p.Description)
                .Include(p => p.PdfDetailss)
                .FirstOrDefault(p => p.Id == model.ProductId);

            if (product3 != null)
            {
                // Clear existing associations for ProductSubCategorys
                product3.ProductSubCategorys.Clear();

                // Add or update new associations based on the model
                foreach (var subcategoryName in model.Subcategories)
                {
                    int subcategoryId = Convert.ToInt16(subcategoryName);
                    var subcategory = _context.SubCategories.FirstOrDefault(x => x.Id == subcategoryId);

                    if (subcategory != null)
                    {
                        // Add or update the association
                        product3.ProductSubCategorys.Add(new ProductSubCategory { Subcategory = subcategory });
                    }
                }

                // Update other properties
                product3.Name = model.ProductName;
                product3.ShortDescription = model.ShortDescription;
                product3.Status = model.Status;
                product3.Content = model.Content;
                product3.PictureName = model.PictureName;
                // Update Category
                var categoryId = Convert.ToInt16(model.CategoryId);
                var category = _context.Categorriees.FirstOrDefault(x => x.Id == categoryId);
                product3.Category = category;

                if (model.PDFDetails != null)
                {
                    foreach (var pdfDetail in model.PDFDetails)
                    {
                        product3.PdfDetailss.Add(pdfDetail);
                    }
                }
                foreach (var pdfId in model.deletePdfId)
                {
                    var pdfDetail = _context.PdfDetails.FirstOrDefault(x => x.Id == pdfId);

                    if (pdfDetail != null)
                    {
                        _context.PdfDetails.Remove(pdfDetail);
                    }
                }
                // Update Title, Heading, and Description collections
                UpdateCollection(product3.Title, model.Titles);
                UpdateCollection(product3.Heading, model.Headings);
                UpdateCollection(product3.Description, model.Descriptions);

                // Save changes to the database
                _context.SaveChanges();
            }

            return Ok();
        }






        private void UpdateCollection<T>(ICollection<T> collection, List<string> newValues) where T : class
        {
            // Clear existing collection
            collection.Clear();

            // Add new values to the collection
            if (newValues != null)
            {
                foreach (var value in newValues)
                {
                    // Create a new entity and add it to the collection
                    T entity = Activator.CreateInstance<T>();
                    var propertyInfo = typeof(T).GetProperty("Name"); // Assuming there's a "Name" property
                    propertyInfo.SetValue(entity, value);
                    collection.Add(entity);
                }
            }
        }








        [HttpGet("GetProducts")]
        public JsonResult GetProducts()
        {
            var productData = _context.Products
    .Select(p => new ProductData
    {
        ProductId = p.Id,
        ProductName = p.Name,
        Status=p.Status,
        ShortDescription = p.ShortDescription,
        Category = p.Category.Category,
        Subcategories = p.ProductSubCategorys.Select(pc => pc.Subcategory.Name).ToList(),
        Titles = p.Title.Select(t => t.Name).ToList(),
        Headings = p.Heading.Select(h => h.Name).ToList(),
        Descriptions = p.Description.Select(d => d.Name).ToList()
    })
    .ToList();
            return new JsonResult(productData);
        }





        [HttpGet("ProductGetId")]
        public JsonResult CategoryById(int Id)
        {
            var cat = _context.Products.Where(e => e.Id == Id);
            return new JsonResult(cat);
        }

        [HttpGet("GetTitlesByProductId")]
        public JsonResult GetTitlesByProductId(int Id)
        {
            var titles = _context.Titles
                .Where(title => title.Products.Id == Id)
                .ToList();

            return new JsonResult(titles);
        }

        [HttpGet("GetHeadingByProductId")]
        public JsonResult GetHeadingByProductId(int Id)
        {
            var heading = _context.Headings
                .Where(heading => heading.Products.Id == Id)
                .ToList();

            return new JsonResult(heading);
        }

        [HttpGet("GetDescriptionByProductId")]
        public JsonResult GetDescriptionByProductId(int Id)
        {
            var description = _context.Descriptions
                .Where(description => description.Products.Id == Id)
                .ToList();

            return new JsonResult(description);
        }


        [HttpGet("DeleteProductById")]
        public JsonResult DeleteProductById(int Id)
        {

            var titleList = _context.Titles.Where(p => p.Products.Id == Id).ToList();
            var headingList = _context.Headings.Where(p => p.Products.Id == Id).ToList();
            var desriptionList = _context.Descriptions.Where(p => p.Products.Id == Id).ToList();
            var productSubCategoryList = _context.ProductSubCategories.Where(p => p.Product.Id == Id).ToList();
            var PdfList = _context.PdfDetails.Where(p => p.Products.Id == Id).ToList();

            foreach (var item in PdfList)
            {
                _context.PdfDetails.Remove(item);
                _context.SaveChanges();
            }

            foreach (var item in titleList)
            {
                _context.Titles.Remove(item);
                _context.SaveChanges();
            }

            foreach (var item in headingList)
            {
                _context.Headings.Remove(item);
                _context.SaveChanges();
            }

            foreach (var item in desriptionList)
            {
                _context.Descriptions.Remove(item);
                _context.SaveChanges();
            }

            foreach (var item in productSubCategoryList)
            {
                _context.ProductSubCategories.Remove(item);
                _context.SaveChanges();
            }

            Products products = _context.Products.Find(Id);
            _context.Products.Remove(products);
            _context.SaveChanges();
            return new JsonResult("Delete Successfully");
        }

        [HttpGet("GetProductSubCategoryByProductId")]
        public JsonResult GetProductSubCategoryByProductId(int Id)
        {
            var subcategoryIds = _context.ProductSubCategories.Where(ps => ps.Product.Id == Id)
                    .Select(ps => ps.Subcategory.Id).ToList();
            List<string> strings = new List<string>();
            foreach (var item in subcategoryIds)
            {
                var a = _context.SubCategories.Find(item);
                strings.Add(a.Name);
            }
            return new JsonResult(strings);
        }


        [HttpGet("GetCategoryNameByPid")]
        public JsonResult GetCategoryBySubCategoryName(int Id)
        {
            var categoryName = _context.Products.Where(p => p.Id == Id)
                        .Select(p => p.Category.Category)
                        .FirstOrDefault();
            return new JsonResult(categoryName);
        }

        [HttpGet("GetProductDetailByProductId")]
        public JsonResult GetProductDetailByProductId(int Id)
        {
            var productData = _context.Products
            .Where(p => p.Id == Id)
            .Select(p => new ProductData
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ShortDescription = p.ShortDescription,
                Content=p.Content,
                PictureName=p.PictureName,
                CategoryId = p.Category.Id,
                Category = p.Category.Category, // Assuming Category is a navigation property in Products
                Status = p.Status,
                SubcategoriesId = p.ProductSubCategorys.Select(pc => pc.Subcategory.Id).ToList(),
                Subcategories = p.ProductSubCategorys.Select(pc => pc.Subcategory.Name).ToList(),
                Titles = p.Title.Select(t => t.Name).ToList(),
                Headings = p.Heading.Select(h => h.Name).ToList(),
                Descriptions = p.Description.Select(d => d.Name).ToList(),
                PDFDetails = p.PdfDetailss.Select(pdf => new PdfDetails
                {
                    Id = pdf.Id,
                    PDFHeading = pdf.PDFHeading,
                    PDFName = pdf.PDFName
                }).ToList()
            })
            .FirstOrDefault();
            return new JsonResult(productData);
        }


    }
}

