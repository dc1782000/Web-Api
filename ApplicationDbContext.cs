using System;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using TestApi.Model;

namespace TestApi.Data
{
	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SignUpDetails>? SignUpDetails { get; set; }

        

        public DbSet<Categories>? Categorriees { get; set; }

        public DbSet<Products>? Products { get; set; }
        
        public DbSet<PdfDetails> PdfDetails { get; set; }

        public DbSet<SubCategories> SubCategories { get; set; }

        public DbSet<Title> Titles { get; set; }

        public DbSet<Heading> Headings { get; set; }

        public DbSet<Description> Descriptions { get; set; }

        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
    }
}

