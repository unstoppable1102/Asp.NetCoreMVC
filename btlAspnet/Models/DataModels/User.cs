using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace btlAspnet.Models.DataModels
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="The UserName's field is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password's field is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The FullName's field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The Email's field is required")]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public Boolean IsAdmin { get; set; }
        public string? Avatar {  get; set; }
        public Boolean Status { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
