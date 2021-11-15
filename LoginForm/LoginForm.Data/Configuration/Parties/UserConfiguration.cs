using LoginForm.Model.Enums;
using LoginForm.Model.Parties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginForm.Data.Configuration.Parties
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(t => t.RecordVersion).IsConcurrencyToken().IsRowVersion().IsRequired();
            builder.HasMany(x => x.Roles).WithOne(x => x.User).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.ApiTokens).WithOne(x => x.User).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            builder.Property(x => x.Status).IsRequired(true).HasDefaultValue(Status.Active);
            builder.ToTable("Users");
        }
    }
}
