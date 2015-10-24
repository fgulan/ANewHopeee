using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wild8.Startup))]
namespace Wild8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Utils.PasswordHash.Main(null);
            ConfigureAuth(app);
        }
    }
}
