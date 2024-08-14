using btlAspnet.Models.DataModels;

namespace btlAspnet.Models
{
    public class ProductCreateViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
