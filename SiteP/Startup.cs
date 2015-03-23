using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteP.Startup))]
namespace SiteP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
