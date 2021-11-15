using LoginForm.Model.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoginForm.Model.Parties
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Picture { get; set; }

        public Status Status { get; set; }
        public string Location { get; set; }
        public string DOB { get; set; }

        public List<UserTokenEntity> ApiTokens { get; set; }

        public List<UserRoleEntity> Roles { get; set; }

    }
}
