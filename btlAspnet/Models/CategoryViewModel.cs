namespace btlAspnet.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Image { get; set; }
        public Boolean IsActive { get; set; }

        public int ProductCount { get; set; }
    }
}
