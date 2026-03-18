using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BS_WebSystem.Startup))]
namespace BS_WebSystem
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
