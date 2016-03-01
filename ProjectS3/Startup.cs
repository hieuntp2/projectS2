using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectS3.Startup))]
namespace ProjectS3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
