using System.ComponentModel.DataAnnotations;

namespace IdentityServerDemo.Quickstart.Account
{
    public class RegisterInputModel
    {
//        [Display(Name = "First Name")]
//        [Required]
//        public string FirstName { get; set; }
//        [Display(Name = "Last Name")]
//        [Required]
//        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}