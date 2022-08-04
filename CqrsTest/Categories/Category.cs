using CqrsTest.Data;
using CqrsTest.Products;

namespace CqrsTest.Categories
{
    public class Category : Model
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
