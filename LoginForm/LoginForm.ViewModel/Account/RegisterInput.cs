using System.ComponentModel.DataAnnotations;

namespace LoginForm.ViewModel.Account
{
    public class RegisterInput
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Location { get; set; }
        public string DOB { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
