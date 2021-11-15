using LoginForm.Model.Parties;

namespace LoginForm.Data.Repositories.Parties
{
    public class UserRoleRepository : BaseRepository<UserRoleEntity, AppDbContext>
    {
        #region constructor
        public UserRoleRepository(AppDbContext context) : base(context)
        { }
        #endregion
    }
}
