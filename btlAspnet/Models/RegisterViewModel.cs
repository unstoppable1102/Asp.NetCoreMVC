

using System.ComponentModel.DataAnnotations;

namespace btlAspnet.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The UserName's field is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password's field is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The FullName's field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The Email's field is required")]
        public string Email { get; set; }
    }
}
