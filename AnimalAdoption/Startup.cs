using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnimalAdoption.Startup))]
namespace AnimalAdoption
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
