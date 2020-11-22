using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoInc.Server.Biz;
using TodoInc.Server.Persistence;

namespace TodoInc.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ITodosRepository, TodosInMemoryRepository>()
                .AddScoped<TodosService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                application
                    .UseDeveloperExceptionPage()
                    .UseWebAssemblyDebugging();
            }
            else
            {
                application
                    .UseExceptionHandler("/Error")
                    .UseHsts();
            }

            application
                .UseHttpsRedirection()
                .UseBlazorFrameworkFiles()
                .UseStaticFiles()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("index.html");
                });
        }
    }
}
