using LoginForm.Model.Parties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginForm.Data.Configuration.Parties
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.Property(t => t.RecordVersion).IsConcurrencyToken().IsRowVersion().IsRequired();
            builder.ToTable("UserRoles");
        }
    }
}
