using System;
namespace TestApi.Model
{
	public class ChangePas
	{
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}

