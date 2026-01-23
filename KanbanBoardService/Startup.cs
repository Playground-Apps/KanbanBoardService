using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KanbanBoardService.Data;

namespace KanbanBoardService
{
    /// <summary>
    /// Startup class that registers services (including the DbContext) and configures the request pipeline.
    /// To use this Startup with the generic host, wire it in Program.cs (for example:
    /// builder.WebHost.UseStartup&lt;Startup&gt;() or webBuilder.UseStartup&lt;Startup&gt;()).
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // Register controllers (API endpoints)
            services.AddControllers();

            // Register EF Core DbContext. Expects a connection string named "DefaultConnection" in configuration.
            services.AddDbContext<KanbanBoardDatabaseContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // TODO: register repositories, services, and other application services here.
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}