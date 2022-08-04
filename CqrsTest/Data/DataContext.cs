using CqrsTest.Categories;
using CqrsTest.Products;
using Microsoft.EntityFrameworkCore;

namespace CqrsTest.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
