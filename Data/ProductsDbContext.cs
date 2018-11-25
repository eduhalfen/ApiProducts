using System;
using Microsoft.EntityFrameworkCore;
using Module2Edu.Models;

namespace Module2Edu.Data
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options): base(options)
        {
        }
    }
}
