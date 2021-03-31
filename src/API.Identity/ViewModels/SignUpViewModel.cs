using System.ComponentModel.DataAnnotations;

namespace API.Identity.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(50, ErrorMessage = "Email must not exceed 50 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        // At least one upper case english letter • At least one lower case english letter • At least one digit • At least one special character • Minimum 8 in length • Maximum 50 in length
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,50}$", ErrorMessage = "Password does not meet the criteria")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Re-typed password is required")]
        [Compare("Password", ErrorMessage = "Re-typed Password must be the same as Password")]
        public string ConfirmPassword { get; set; }
    }
}
