using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Model.Parties
{
    public class UserTokenEntity : BaseEntity
    {
        public string Token { get; set; }

        public string ExpiryDate { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
