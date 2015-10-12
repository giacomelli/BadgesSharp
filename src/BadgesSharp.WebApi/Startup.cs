using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BadgesSharp.WebApi.Startup))]

namespace BadgesSharp.WebApi
{
    /// <summary>
    /// The app startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Config the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
