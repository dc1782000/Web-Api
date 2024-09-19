using System;
namespace TestApi.Model
{
	public class Demosignup
	{
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public bool IsActive { get; set; }
    }
}

