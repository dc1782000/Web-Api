using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class Products
	{
        [Key]
        public int Id { get; set; }


        public Categories? Category { get; set; }

        public List<ProductSubCategory>? ProductSubCategorys { get; set; }= new List<ProductSubCategory>();



        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public bool Status { get; set; }

        public string? PictureName { get; set; }

        public string? Content { get; set; }

        public List<Title>? Title { get; set; }= new List<Title>();


        public List<Heading>? Heading { get; set; } = new List<Heading>();

        public List<Description>? Description { get; set; } = new List<Description>();

        public List<PdfDetails>? PdfDetailss { get; set; } = new List<PdfDetails>();
        
    }
}

