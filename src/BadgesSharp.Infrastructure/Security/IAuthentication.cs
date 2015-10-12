using System.Threading.Tasks;

namespace BadgesSharp.Infrastructure.Security
{
    /// <summary>
    /// Defines an interface to an authentication used to verify if the user can generate a badge.
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Performs the authentication.
        /// </summary>
        /// <returns>The authentication result.</returns>
        Task<AuthenticationResult> Authenticate();
    }
}
