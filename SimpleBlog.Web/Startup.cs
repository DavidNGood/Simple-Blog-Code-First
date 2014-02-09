using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleBlog.Web.Startup))]
namespace SimpleBlog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
