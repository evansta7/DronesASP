using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Drones.Startup))]
namespace Drones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
