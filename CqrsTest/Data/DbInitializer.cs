using CqrsTest.Categories;
using CqrsTest.Products;

namespace CqrsTest.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(DataContext context)
        {
            await context.Database.EnsureCreatedAsync();

            var categories = new[]
            {
                new Category()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-0000000001"),
                    Name = "Category 1",
                },
                new Category()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-0000000002"),
                    Name = "Category 2",
                },
            };

            await context.Categories.AddRangeAsync(categories);

            var products = new[]
            {
                new Product()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Product 1",
                    Description = "Product description 1",
                    CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                },
                new Product()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Product 2",
                    Description = "Product description 2",
                    CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
