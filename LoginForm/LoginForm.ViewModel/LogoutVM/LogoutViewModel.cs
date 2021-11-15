using System.ComponentModel.DataAnnotations;

namespace LoginForm.ViewModel.LogoutVM
{
    public class LogoutViewModel
    {
        [Required]
        public int UserId { get; set; }
    }
}
