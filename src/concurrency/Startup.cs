using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Concurrency
{
       public class Startup {
        public void ConfigureServices(IServiceCollection services) {        
           services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(routes =>
                        {                          
                            routes.MapControllerRoute(
                                name: "sync_task",
                                pattern: "Home/SynchTasks/{action:regex(^GetFromResult$|^GetTaskCompletionSource$)}",
                                defaults: new { controller = "Home" });

                            routes.MapControllerRoute(
                                name: "Retry",
                                pattern: "SimulationRetry",
                                defaults: new { controller = "Home", action = "Retry" });

                            routes.MapControllerRoute(
                                name: "RetryResult",
                                pattern: "SimulationRetryResult",
                                defaults: new { controller = "Home", action = "DownloadWithRetry" });

                            routes.MapControllerRoute(
                                name: "LinkedToken",
                                pattern: "LinkedToken",
                                defaults: new { controller = "Home", action = "CancelAsync" });

                            routes.MapControllerRoute(
                                name: "LinkedTokenStart",
                                pattern: "LinkedTokenResult",
                                defaults: new { controller = "Home", action = "StartAsyncCode" });

                            routes.MapControllerRoute(
                                name: "LinkedTokenCancel",
                                pattern: "LinkedTokenResult",
                                defaults: new { controller = "Home", action = "CancelAsyncCode" });

                            routes.MapControllerRoute(
                                name: "Throw",
                                pattern: "Throw/CancellationToken",
                                defaults: new { controller = "Home", action = "CancellationTokenLoop" });

                            routes.MapControllerRoute(
                                name: "ex_task",
                                pattern: "MultipleTasks/Aggregation",
                                defaults: new { controller = "Home", action = "AggregationException" });

                            routes.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");

                        });
        }
    }
}
