using btlAspnet.Models.DataModels;

namespace btlAspnet.Models
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }

		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
