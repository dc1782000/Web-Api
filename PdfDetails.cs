using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class PdfDetails
	{
		[Key]
		public int Id { get; set; }


        public string PDFHeading { get; set; }

		public string PDFName { get; set; }

        public Products Products { get; set; }
    }
}

