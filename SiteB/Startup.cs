using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteB.Startup))]
namespace SiteB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
