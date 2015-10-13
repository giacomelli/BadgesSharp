using System.Linq.Expressions;
using Parse;

namespace BadgesSharp.Infrastructure.Repositories
{
    /// <summary>
    /// Parse query converter.
    /// </summary>
    public class ParseQueryConverter : ExpressionVisitor
    {
        #region Fields
        private ParseQuery<ParseObject> m_parseQuery;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BadgesSharp.Infrastructure.Repositories.ParseQueryConverter"/> class.
        /// </summary>
        /// <param name="className">Class name.</param>
        private ParseQueryConverter(string className)
        {
            m_parseQuery = ParseObject.GetQuery(className);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts the expression to a ParseQuery.
        /// </summary>
        /// <returns>The ParseQuery.</returns>
        /// <param name="className">The cass name.</param>
        /// <param name="expression">The expression.</param>
        public static ParseQuery<ParseObject> FromExpression(string className, Expression expression)
        {
            var converter = new ParseQueryConverter(className);
            converter.Visit(expression);

            return converter.m_parseQuery;
        }

        /// <summary>
        /// Visits the binary.
        /// </summary>
        /// <returns>The binary.</returns>
        /// <param name="node">The node.</param>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                var keyExpression = node.Left as MemberExpression;

                string value = null;
                var valueExpression = node.Right as ConstantExpression;

                if (valueExpression == null)
                {
                    var memberExpression = node.Right as MemberExpression;

                    if (memberExpression != null)
                    {
                        value = Expression.Lambda(memberExpression).Compile().DynamicInvoke().ToString();
                    }
                }
                else
                {
                    value = valueExpression.Value.ToString();
                }

                if (keyExpression != null && value != null)
                {
                    var key = keyExpression.Member.Name;

                    m_parseQuery = m_parseQuery.WhereEqualTo(key, value);
                }
            }

            return base.VisitBinary(node);
        }
        #endregion
    }
}
