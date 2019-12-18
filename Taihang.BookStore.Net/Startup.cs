using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Taihang.BookStore.Net.Startup))]
namespace Taihang.BookStore.Net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
