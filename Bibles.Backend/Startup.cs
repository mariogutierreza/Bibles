using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bibles.Backend.Startup))]
namespace Bibles.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
