using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace btlAspnet.Models.DataModels
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The ProductName' field is required")]
        [StringLength(100)]
        public string ProductName { get; set; }

        public double Price { get; set; }
        public double PriceOld { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public Boolean Status { get; set; }
        [StringLength(500)]
        public string? Image {  get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }

        //thuộc tính quan hệ navigate
        public Category Category { get; set; }



    }
}
