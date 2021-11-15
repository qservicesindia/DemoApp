using LoginForm.Data.Repositories.Parties;
using LoginForm.Data.Repositories.Product;
using Microsoft.Extensions.Configuration;

namespace LoginForm.Data
{
    public class AppUnitOfWork : BaseUow
    {
        #region Constructor
        public AppUnitOfWork(AppDbContext context) : base(context) { }

        public AppUnitOfWork(AppDbContext context, IConfiguration config) : base(context) { _config = config; }

        #endregion

        #region Users

        private UserRepository _userRepository;
        IConfiguration _config;
        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(Context);
                }

                return _userRepository;
            }
        }

        private UserTokenRepository _userTokenRepository;

        public UserTokenRepository UserTokenRepository
        {
            get
            {
                if (_userTokenRepository == null)
                {
                    _userTokenRepository = new UserTokenRepository(Context);
                }

                return _userTokenRepository;
            }
        }


        private RoleRepository _roleRepository;

        public RoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new RoleRepository(Context);
                }

                return _roleRepository;
            }
        }

        #endregion


        #region Product

        private ProductRepository _productRepository;

        public ProductRepository productRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(Context);
                }

                return _productRepository;
            }
        }
        #endregion
    }
}
