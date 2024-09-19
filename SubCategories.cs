using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class SubCategories
	{
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool? Status { get; set; }


        public Categories? Category { get; set; }
    }
}

