using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Altairis.ValidationToolkit.CoreSample {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);

            // Uncomment the following line to turn off the bank code validation globally
            // services.AddSingleton<IBankCodeValidator>(new EmptyBankCodeValidator());

            // Uncomment the following line to use online bank code validation
            // services.AddSingleton<IBankCodeValidator>(new OnlineBankCodeValidator());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }
    }
}
