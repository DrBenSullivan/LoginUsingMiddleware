using LoginUsingMiddleware.Middleware;
using Microsoft.AspNetCore.Builder;

namespace LoginUsingMiddleware
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMethodFiltering();
            app.UseInputValidation();
        }
    }
}
