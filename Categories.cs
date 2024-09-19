using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class Categories
	{
		[Key]
		public int Id { get; set; }

        public string? Category { get; set; }

		public bool Status { get; set; }

	}
}

