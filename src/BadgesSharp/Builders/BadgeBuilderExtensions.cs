using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgesSharp.Builders
{
    /// <summary>
    /// BadgeBuilder extension methods.
    /// </summary>
    public static class BadgeBuilderExtensions
    {
        /// <summary>
        /// Sets the status by percent.
        /// </summary>
        /// <param name="builder">The badge builder.</param>
        /// <param name="percent">The percent.</param>
        public static void SetStatusByPercent(this BadgeBuilderBase builder, float percent)
        {
            if (percent < 50)
            {
                builder.Status = BadgeStatus.Failed;
            }
            else if (percent < 90)
            {
                builder.Status = BadgeStatus.Warning;
            }
            else
            {
                builder.Status = BadgeStatus.Success;
            }
        }
    }
}
