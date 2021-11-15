using System;
using System.Threading.Tasks;

namespace LoginForm.Data
{
    public abstract class BaseUow : IDisposable
    {
        #region fields
        private AppDbContext _context;
        #endregion

        #region constructor
        public BaseUow(AppDbContext context)
        {
            _context = context;
        }
        //public BaseUow(string connectionString)
        //{
        //    _context = new ReviewBotDbContext(connectionString);
        //}
        #endregion

        #region members
        public void Commit()
        {
            this._context.applyRules = true;
            this._context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            this._context.applyRules = true;
            await this._context.SaveChangesAsync();
        }

        public void CommitApplyRulesFalse()
        {
            this._context.applyRules = false;
            this._context.SaveChanges();
        }
        #endregion

        #region properties
        public AppDbContext Context
        {
            get { return _context; }
        }
        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                }
            }
        }

        #endregion
    }
}
