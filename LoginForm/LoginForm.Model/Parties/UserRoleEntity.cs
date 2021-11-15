using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Model.Parties
{
    public class UserRoleEntity : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [ForeignKey("RoleId")]
        public RoleEntity Role { get; set; }

    }
}
