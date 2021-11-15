using System.ComponentModel.DataAnnotations;

namespace LoginForm.Model.Parties
{
    public class RoleEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
