using System.ComponentModel.DataAnnotations;

namespace LoginForm.ViewModel.Account
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}