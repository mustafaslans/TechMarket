using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechMarket.Startup))]
namespace TechMarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
