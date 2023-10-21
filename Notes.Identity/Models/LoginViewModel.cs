using System.ComponentModel.DataAnnotations;

namespace Notes.Identity.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "username")]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
