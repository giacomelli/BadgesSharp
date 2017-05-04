using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Repositories;

namespace BadgesSharp.Infrastructure.Repositories
{
	/// <summary>
	/// Firebase repository.
	/// </summary>
	public class FirebaseRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
        where TEntity : IAggregateRoot, new()
    {
        #region Fields
        private static IFirebaseClient s_client;
        private string m_rootPath;
        #endregion

        #region Constructors
		/// <summary>
		/// Initializes the <see cref="T:BadgesSharp.Infrastructure.Repositories.FirebaseRepository`1"/> class.
		/// </summary>
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

		/// <summary>
		/// Counts all entities that matches the filter.
		/// </summary>
		/// <returns>The number of the entities that matches the filter.</returns>
		/// <param name="filter">The filter.</param>
		public override long CountAll(Expression<Func<TEntity, bool>> filter)
        {
            return FindAll(0, int.MaxValue, (b) => true).Count();
        }

		/// <summary>
		/// Finds all entities that matches the filter.
		/// </summary>
		/// <returns>The found entities.</returns>
		/// <param name="offset">The offset to start the result.</param>
		/// <param name="limit">The result count limit.</param>
		/// <param name="filter">The entities filter.</param>
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

		/// <summary>
		/// Finds all entities that matches the filter in a ascending order.
		/// </summary>
		/// <returns>The found entities.</returns>
		/// <param name="offset">The offset to start the result.</param>
		/// <param name="limit">The result count limit.</param>
		/// <param name="filter">The entities filter.</param>
		/// <param name="orderBy">The order.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		public override IEnumerable<TEntity> FindAllAscending<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Finds all entities that matches the filter in a descending order.
		/// </summary>
		/// <returns>The found entities.</returns>
		/// <param name="offset">The offset to start the result.</param>
		/// <param name="limit">The result count limit.</param>
		/// <param name="filter">The entities filter.</param>
		/// <param name="orderBy">The order.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		public override IEnumerable<TEntity> FindAllDescending<TKey>(int offset, int limit, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderBy)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Finds the entity by the key.
		/// </summary>
		/// <returns>The found entity.</returns>
		/// <param name="key">The key.</param>
		public override TEntity FindBy(object key)
        {
            return FindAll(0, 1, f => f.Key == key).FirstOrDefault();
        }

		/// <summary>
		/// Persists the deleted item.
		/// </summary>
		/// <param name="item">The item.</param>
		protected override void PersistDeletedItem(TEntity item)
        {
            s_client.Delete(GetEntityPath(item));
        }

		/// <summary>
		/// Persists the new item.
		/// </summary>
		/// <param name="item">The item.</param>
		protected override void PersistNewItem(TEntity item)
        {            
            var response = s_client.Push(GetEntityPath(item), item);
            var entityCreated = response.ResultAs<TEntity>();
            item.Key = entityCreated.Key;
        }

		/// <summary>
		/// Persists the updated item.
		/// </summary>
		/// <param name="item">The item.</param>
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