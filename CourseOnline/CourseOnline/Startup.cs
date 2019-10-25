using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseOnline.Startup))]
namespace CourseOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
