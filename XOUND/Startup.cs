using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XOUND.Startup))]
namespace XOUND
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
