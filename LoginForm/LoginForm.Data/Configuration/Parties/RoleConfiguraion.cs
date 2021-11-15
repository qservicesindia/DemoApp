using LoginForm.Model.Parties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LoginForm.Data.Configuration.Parties
{
    public class RoleConfiguraion : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.Property(t => t.RecordVersion).IsConcurrencyToken().IsRowVersion().IsRequired();
            builder.ToTable("Roles");

            builder.HasData(
                new RoleEntity() { Id = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = DataConstants.Roles.Admin },
                new RoleEntity() { Id = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = DataConstants.Roles.User }
                );
        }
    }
}
