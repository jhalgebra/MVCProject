using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCProject.WebClient.Startup))]
namespace MVCProject.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
