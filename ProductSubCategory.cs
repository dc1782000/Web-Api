using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class ProductSubCategory
	{
        [Key]
        public int ProductSubcategoryId { get; set; }

        
        public Products Product { get; set; }

        
        public SubCategories Subcategory { get; set; }
    }
}

