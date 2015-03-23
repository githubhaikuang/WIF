using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteA.Startup))]
namespace SiteA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
