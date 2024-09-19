using System;
namespace TestApi.Model
{
	public class UpdateProductModel
	{
        public int Id { get; set; }




        public string? Categoryy { get; set; }




        public List<string>? SubCategoriessName { get; set; } = new List<string>();

        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public bool Status { get; set; }

        public string? Title { get; set; }

        public string? Heading { get; set; }

        public string? Description { get; set; }
    }
}

