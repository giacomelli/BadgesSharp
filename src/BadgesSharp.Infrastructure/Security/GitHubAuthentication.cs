using System;
using System.Threading.Tasks;
using HelperSharp;
using Octokit;

namespace BadgesSharp.Infrastructure.Security
{
    /// <summary>
    /// A GitHub implementation of IAuthentication.
    /// </summary>
    public class GitHubAuthentication : IAuthentication
    {
        #region Fields
        private string m_repositoryOwner;
        private string m_repositoryName;
        private string m_accessToken;
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAuthentication"/> class.
        /// </summary>
        /// <param name="repositoryOwner">The repository owner.</param>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="accessToken">The access token.</param>
        public GitHubAuthentication(string repositoryOwner, string repositoryName, string accessToken)
        {
            ExceptionHelper.ThrowIfNullOrEmpty("repositoryOwner", repositoryOwner);
            ExceptionHelper.ThrowIfNullOrEmpty("repositoryName", repositoryName);

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("The Personal access token should be informed to generate the badge. Create one on https://github.com/settings/tokens.");
            }

            m_repositoryOwner = repositoryOwner;
            m_repositoryName = repositoryName;
            m_accessToken = accessToken;
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Performs the authentication.
        /// </summary>
        /// <returns>
        /// The authentication result.
        /// </returns>
        public async Task<AuthenticationResult> Authenticate()
        {
            var result = new AuthenticationResult()
            {
                Success = true
            };

            var github = new GitHubClient(new Octokit.ProductHeaderValue("BadgesSharp"));
            github.Credentials = new Credentials(m_accessToken);
            Octokit.Repository repo;

            try
            {
                repo = await github.Repository.Get(m_repositoryOwner, m_repositoryName);

                if (!repo.Permissions.Push)
                {
                    result.Success = false;
                    result.Message = "You do not have push permission to {0}/{1}".With(m_repositoryOwner, m_repositoryName);
                }
            }
            catch (ApiException ex)
            {
                result.Success = false;

                if (ex.Message.Equals("Not Found", StringComparison.OrdinalIgnoreCase))
                {
                    result.Message = "Your personal access token is valid, but it does not have access to {0}/{1}. If you're trying to generate a badge to a private repository, you need to check the scope \"repo\" at https://github.com/settings/tokens".With(m_repositoryOwner, m_repositoryName);
                }
                else
                {
                    result.Message = "Your personal access token seems be invalid. Certified you've generated it on https://github.com/settings/tokens: {0}".With(ex.Message);
                }
            }

            return result;
        }
        #endregion 
    }
}
