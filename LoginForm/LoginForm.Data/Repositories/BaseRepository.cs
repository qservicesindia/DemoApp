using LoginForm.Data.Extensions;
using LoginForm.Model;
using LoginForm.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace LoginForm.Data.Repositories
{
    public class BaseRepository<T, D> where T : class, IIdentifier
        where D : DbContext
    {
        #region constants
        public const int DEFAULTPAGESIZE = 50;
        public const int DEFAULTPAGE = 1;
        #endregion

        #region fields
        private D _context;
        //private ILog InternalLogger { get; set; }
        #endregion

        #region constructor
        public BaseRepository(D context)
        {
            //InternalLogger = LogManager.GetLogger("EntityFramework.Log");
            _context = context;

            //if (ConfigurationManager.AppSettings["LogEntityFramework"] == "1")
            //{
            //    _context.Database.Log = delegate (string value)
            //    {
            //        InternalLogger.Info(value);
            //    };
            //}
        }
        #endregion

        #region protected properties
        protected D Context
        {
            get
            {
                return _context;
            }
        }
        #endregion

        #region public methods


        public void DeleteAll<T>() where T : class
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var queryable = _context.Set<T>().Where(q => q.Id.Equals(id));

            queryable = queryable.WithIncludes(includes);

            return queryable.FirstOrDefault();
        }

        public T GetByIdReadOnly(int id, params Expression<Func<T, object>>[] includes)
        {
            var queryable = _context.Set<T>().Where(q => q.Id.Equals(id));

            queryable = queryable.WithIncludes(includes);

            return queryable.AsNoTracking().FirstOrDefault();
        }

        public virtual T InsertOrUpdate(T entity)
        {
            if (entity.Id.Equals(0))
            {
                if (entity is IAuditInfo)
                {
                    ((IAuditInfo)entity).CreatedOn = DateTime.Now;
                    ((IAuditInfo)entity).ModifiedOn = DateTime.Now;
                }

                _context.Set<T>().Add(entity);
            }
            else
            {
                if (entity is IAuditInfo)
                {
                    ((IAuditInfo)entity).ModifiedOn = DateTime.Now;
                }

                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            return (T)entity;
        }

        public virtual List<T> GetAll(Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            query = query.WithIncludes(includes);

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.ToList();
        }

        public virtual List<T> GetAllReadOnly(Func<IQueryable<T>
           , IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            query = query.WithIncludes(includes);

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.AsNoTracking().ToList();
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _context.Set<T>().Remove(entity);
        }

        public void Delete(int id)
        {
            if (id.Equals(0)) throw new ArgumentNullException("id");

            T entity = GetById(id);
            Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            _context.Set<T>().RemoveRange(entities);
        }

        public virtual int Count(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual List<T> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //int totalCount = query.Count();

            query = query.WithIncludes(includes);

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            //int filteredCount = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.ToList();
        }

        public virtual List<T> FindReadOnly(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>
           , IOrderedQueryable<T>> orderBy = null, int? page = null,
           int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //int totalCount = query.Count();

            query = query.WithIncludes(includes);

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            //int filteredCount = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.AsNoTracking().ToList();
        }
        public virtual List<T> FindDistinct(Expression<Func<T, bool>> filter = null, Func<T, object> distinctBy = null, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.WithIncludes(includes);

            if (distinctBy != null)
            {
                query = query.DistinctBy<T, object>(distinctBy).AsQueryable();
            }

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.ToList();
        }

        public virtual List<T> FindDistinctReadOnly(Expression<Func<T, bool>> filter = null, Func<T, object> distinctBy = null, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.WithIncludes(includes);

            if (distinctBy != null)
            {
                query = query.DistinctBy<T, object>(distinctBy).AsQueryable();
            }

            if (orderBy != null)
            {
                var orderByQuery = orderBy(query);
                if (orderByQuery == null)
                {
                    query = query.OrderByDescending(q => q.Id);
                }
                else
                {
                    query = orderByQuery;
                }
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query.AsNoTracking().ToList();
        }

        public virtual PagedList<T> PagedFind(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            List<T> findResult = FindReadOnly(filter, orderBy, page, pageSize, includes);
            int TotalCount = this.Count(filter);

            return new PagedList<T>(findResult, TotalCount, pageSize, page);
        }


        public virtual PagedList<T> PagedFindIQueryable(IQueryable<T> queryable, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            int TotalCount = queryable.Count();

            queryable = queryable.WithIncludes(includes);


            if (orderBy != null)
            {
                var orderByQuery = orderBy(queryable);
                if (queryable == null)
                {
                    queryable = queryable.OrderByDescending(x => x.Id);
                }
                else
                {
                    queryable = orderByQuery;
                }

            }

            if (page.HasValue && pageSize.HasValue)
            {
                queryable = queryable.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                queryable = queryable.Take(DEFAULTPAGESIZE);
            }

            return new PagedList<T>(queryable.AsNoTracking().ToList(), TotalCount, pageSize, page);
        }


        public virtual PagedList<T> PagedFindDistinct(Expression<Func<T, bool>> filter = null, Func<T, object> distinct = null, Func<IQueryable<T>
            , IOrderedQueryable<T>> orderBy = null, int? page = null,
            int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            List<T> findResult = FindDistinctReadOnly(filter, distinct, orderBy, page, pageSize, includes);
            int TotalCount = this.Count(filter);

            return new PagedList<T>(findResult, TotalCount, pageSize, page);
        }


        public virtual IQueryable<T> FindByQueryable(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.WithIncludes(includes);

            return query;
        }


        public virtual IQueryable<T> FindByQueryable(Expression<Func<T, bool>> filter = null, string[] orderBy = null, int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includes)
        {
            const string orderFieldPattern = "{0} {1}";
            const string ASCENDING = "asc";
            const string DESCENDING = "desc";

            IQueryable<T> query = FindByQueryable(filter, includes);

            if (orderBy == null)
            {
                query = query.OrderBy(o => o.Id);
            }
            else
            {
                bool hasSortFieldAppended = false;

                foreach (string orderField in orderBy)
                {
                    if (orderField.Trim().StartsWith("-"))
                    {
                        if (typeof(T).GetProperty(orderField.Trim().Substring(1), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
                        {
                            query = query.OrderBy(string.Format(orderFieldPattern, orderField.Trim().Substring(1), DESCENDING));
                            hasSortFieldAppended = true;
                        }
                        else
                        {
                            if (!hasSortFieldAppended)
                            {
                                query = query.OrderBy(o => o.Id);
                            }
                        }
                    }
                    else
                    {
                        if (typeof(T).GetProperty(orderField.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
                        {
                            query = query.OrderBy(string.Format(orderFieldPattern, orderField.Trim(), ASCENDING));
                            hasSortFieldAppended = true;
                        }
                        else
                        {
                            if (!hasSortFieldAppended)
                            {
                                query = query.OrderBy(o => o.Id);
                            }
                        }
                    }
                }
            }

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            else
            {
                query = query.Take(DEFAULTPAGESIZE);
            }

            return query;
        }

        public virtual T FindOne(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.WithIncludes(includes);

            return (T)query.FirstOrDefault();
        }

        public virtual T FindOneReadOnly(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.WithIncludes(includes);

            return (T)query.AsNoTracking().FirstOrDefault();
        }

        //example insert into CustomerUsers (id) values (@P0)
        //public virtual int ExecSQL(string sql, params object[] parameters)
        //{
        //    return this.Context.Database.ExecuteSqlCommand(sql, parameters);
        //}

        public virtual void SetStateDeleted(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;

        }

        #endregion
    }
}
