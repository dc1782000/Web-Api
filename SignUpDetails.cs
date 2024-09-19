using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class SignUpDetails
	{
		[Key]
		public int Id { get; set; }

		public string? Email { get; set; }

		public string? Password { get; set; }

        public bool IsActive { get; set; }
    }
}

