using System;
namespace TestApi.Model
{
	public class ProductData
	{
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDescription { get; set; }
        public string? Category { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }
        public string? PictureName { get; set; }

        public string? Content { get; set; }

        public List<string>? Subcategories { get; set; }
        public List<int>? SubcategoriesId { get; set; }
        public List<string>? Titles { get; set; }
        public List<string>? Headings { get; set; }
        public List<string>? Descriptions { get; set; }

        public List<int>? deletePdfId { get; set; }
        public List<PdfDetails>? PDFDetails { get; set; } = new List<PdfDetails>();

    }
}

