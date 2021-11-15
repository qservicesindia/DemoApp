using LoginForm.Model.Parties;

namespace LoginForm.Data.Repositories.Parties
{
    public class UserTokenRepository : BaseRepository<UserTokenEntity, AppDbContext>
    {
        #region constructor
        public UserTokenRepository(AppDbContext context) : base(context)
        { }
        #endregion
    }
}
