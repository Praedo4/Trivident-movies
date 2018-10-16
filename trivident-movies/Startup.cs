using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(trivident_movies.Startup))]
namespace trivident_movies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
