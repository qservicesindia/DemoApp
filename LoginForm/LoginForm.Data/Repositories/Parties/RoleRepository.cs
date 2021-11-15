using LoginForm.Model.Parties;

namespace LoginForm.Data.Repositories.Parties
{
    public class RoleRepository : BaseRepository<RoleEntity, AppDbContext>
    {
        #region constructor
        public RoleRepository(AppDbContext context) : base(context)
        { }
        #endregion
    }
}
