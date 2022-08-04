using CqrsTest.Categories;
using CqrsTest.Data;

namespace CqrsTest.Products
{
    public class Product : Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
