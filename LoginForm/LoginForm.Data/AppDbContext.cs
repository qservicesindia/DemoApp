using LoginForm.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LoginForm.Data
{
    public class AppDbContext : DbContext
    {
        #region fields
        internal bool applyRules = true;
        #endregion

        #region constructor

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region overrides
        public override int SaveChanges()
        {
            try
            {
                if (applyRules)
                {
                    this.ApplyRules();
                }
                return base.SaveChanges();
            }
            catch (ValidationException ex)
            {
                throw ex;

                //var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                //var fullErrorMessage = string.Join("; ", errorMessages);

                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //throw new ValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #endregion

        #region private methods
        private void ApplyRules()
        {
            var changeSet = this.ChangeTracker.Entries();

            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(
                            e => e.Entity is IAuditInfo && (e.State == EntityState.Added) || (e.State == EntityState.Modified)
                        ))
                {
                    IAuditInfo e = (IAuditInfo)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        e.CreatedOn = DateTime.UtcNow;
                    }

                    e.ModifiedOn = DateTime.UtcNow;
                }


                var validationResults = new List<ValidationResult>();
                foreach (var entity in changeSet)
                {
                    if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                    {
                        throw new ValidationException();// or do whatever you want
                    }
                }
            }
        }

        #endregion
    }
}
