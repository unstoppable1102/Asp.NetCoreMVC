using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace btlAspnet.Models.DataModels
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Image { get; set; }
        public Boolean IsActive { get; set; }

        //thuộc tính quan hệ navigate
        public ICollection<Product> Products { get; set; }
    }
}
