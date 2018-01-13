using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vision_V1.Startup))]
namespace Vision_V1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
