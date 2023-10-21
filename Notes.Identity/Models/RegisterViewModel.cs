using System.ComponentModel.DataAnnotations;

namespace Notes.Identity.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "username")]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "password confirmation")]
        public string? ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
