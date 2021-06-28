using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WilliamBell_LTC_Application.Startup))]
namespace WilliamBell_LTC_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
