using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Parse;
using Skahal.Infrastructure.Framework.Repositories;
using Skahal.Infrastructure.Framework.Domain;
using System.Reflection;

namespace BadgesSharp.Infrastructure.Repositories
{
	/// <summary>
	/// Parse repository.
	/// </summary>
	public class ParseRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity> 
		where TEntity : IAggregateRoot, new()
    {
		#region Fields
		private string m_className;
		private IEnumerable<PropertyInfo> m_properies;
		#endregion

		#region Constructors
		static ParseRepository()
		{
			ParseClient.Initialize (ConfigHelper.ParseApplicationId, ConfigHelper.ParseDotNetKey);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BadgesSharp.Infrastructure.Repositories.ParseRepository{TEntity}"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
	    public ParseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
			var entityType = typeof(TEntity);
			m_className = entityType.Name;
			m_properies = entityType
				.GetProperties 
				(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty)
				.Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) && p.CanRead && p.CanWrite);
        }
		#endregion

		#region Public methods
		/// <summary>
		/// Counts all entities that matches the filter.
		/// </summary>
		/// <returns>The number of the entities that matches the filter.</returns>
		/// <param name="filter">Filter.</param>
        public override long CountAll(Expression<Func<TEntity, bool>> filter)
        {
			var result =  Task.Run(() => GetQuery().CountAsync()).Result;

			return result;
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
			var query = ParseQueryConverter.FromExpression (m_className, filter);
		    return Task.Run(() => query.FindAsync()).Result.Select(r => FromParse(r));
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
		/// <param name="key">Key.</param>
        public override TEntity FindBy(object key)
        {
            return FromParse(GetParseObject(key.ToString()));
        }
		#endregion

		#region Protected methods
		/// <summary>
		/// Persists the deleted item.
		/// </summary>
		/// <param name="item">Item.</param>
        protected override void PersistDeletedItem(TEntity item)
        {
			var po = GetParseObject (item.Key.ToString());
			po.DeleteAsync ().Wait ();
	    }

		/// <summary>
		/// Persists the new item.
		/// </summary>
		/// <param name="item">Item.</param>
        protected override void PersistNewItem(TEntity item)
        {
            var po = ToParse(item);
			po.ObjectId = null;
            var parseTask = po.SaveAsync();
            parseTask.Wait();

			item.Key = po.ObjectId;
        }      

		/// <summary>
		/// Persists the updated item.
		/// </summary>
		/// <param name="item">Item.</param>
        protected override void PersistUpdatedItem(TEntity item)
        {
			var po = ToParse(item);
			var parseTask = po.SaveAsync();
			parseTask.Wait();
        }
		#endregion

		#region Private methods
        private ParseObject ToParse(TEntity item)
        {
            var po = new ParseObject(m_className);

			foreach (var property in m_properies) {
				po [property.Name] = property.GetValue (item);
			}

			po.ObjectId = item.Key == null ? (string)null : item.Key.ToString();

            return po;
        }

        private TEntity FromParse(ParseObject item)
        {
            if(item == null)
            {
				return default(TEntity);
            }

			var entity = new TEntity ();

			foreach (var property in m_properies) {
				property.SetValue (entity, Convert.ChangeType(item[property.Name], property.PropertyType));
			}

			entity.Key = item.ObjectId;

            return entity;
        }

        private ParseQuery<ParseObject> GetQuery()
        {
			return ParseObject.GetQuery(m_className);
        }

        private ParseObject GetParseObject(string objectId)
        {
            try
            {                
                return Task.Run(() => GetQuery().GetAsync(objectId)).Result;
            }
            catch (AggregateException ex)
            {
                var parseException = ex.InnerExceptions[0] as ParseException;

                if (parseException != null && parseException.Code == ParseException.ErrorCode.ObjectNotFound)
                {
                    return null;
                }

                throw;
            }
        }
		#endregion
    }
}
