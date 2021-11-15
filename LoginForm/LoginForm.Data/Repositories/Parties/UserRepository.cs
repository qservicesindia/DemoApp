using LoginForm.Model.Parties;

namespace LoginForm.Data.Repositories.Parties
{
    public class UserRepository : BaseRepository<UserEntity, AppDbContext>
    {
        #region constructor
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        #endregion
    }
}
