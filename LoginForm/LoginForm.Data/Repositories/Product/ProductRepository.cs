using LoginForm.Model.Product;

namespace LoginForm.Data.Repositories.Product
{
    public class ProductRepository : BaseRepository<ProductEntity, AppDbContext>
    {
        #region constructor
        public ProductRepository(AppDbContext context) : base(context)
        { }
        #endregion
    }
}
