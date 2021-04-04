using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PenzugySzovetseg.Startup))]
namespace PenzugySzovetseg
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
