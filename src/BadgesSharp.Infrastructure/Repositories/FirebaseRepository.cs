using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace BadgesSharp.Infrastructure.Repositories
{
    public class FirebaseRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
        where TEntity : IAggregateRoot, new()
    {
        #region Fields
        private static IFirebaseClient s_client;
        private string m_rootPath;
        #endregion

        #region Constructors
        static FirebaseRepository()
        {
            var config = new FirebaseConfig
            {
                AuthSecret = ConfigHelper.FirebaseAuthSecret,
                BasePath = "https://badgessharp.firebaseio.com/"
            };

            s_client = new FirebaseClient(config);            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgesSharp.Infrastructure.Repositories.FirebaseRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public FirebaseRepository(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            m_rootPath = typeof(TEntity).Name;
        }

        public override long CountAll(Expression<Func<TEntity, bool>> filter)
        {
            return FindAll(0, int.MaxValue, (b) => true).Count();
        }

        public override IEnumerable<TEntity> FindAll(int offset, int limit, Expression<Func<TEntity, bool>> filter)
        {
            var response = s_client.Get(m_rootPath);

            if (response.Body.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return new TEntity[0];
            }

            return response
            .ResultAs<List<TEntity>>()
            .Where(filter.Compile())
            .Skip(offset)
            .Take(limit);
        }

        public override IEnumerable<TEntity> FindAllAscending<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TEntity> FindAllDescending<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy)
        {
            throw new NotImplementedException();
        }

        public override TEntity FindBy(object key)
        {
            return FindAll(0, 1, f => f.Key == key).FirstOrDefault();
        }

        protected override void PersistDeletedItem(TEntity item)
        {
            s_client.Delete(GetEntityPath(item));
        }

        protected override void PersistNewItem(TEntity item)
        {            
            var response = s_client.Push(GetEntityPath(item), item);
            var entityCreated = response.ResultAs<TEntity>();
            item.Key = entityCreated.Key;
        }

        protected override void PersistUpdatedItem(TEntity item)
        {
            s_client.Update(GetEntityPath(item), item);
        }

        private string GetEntityPath(TEntity item)
        {
            return "{0}/{1}".With(m_rootPath, item.Key);
        }
        #endregion
    }
}