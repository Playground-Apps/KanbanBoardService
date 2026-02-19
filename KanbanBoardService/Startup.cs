using Microsoft.EntityFrameworkCore;

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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()                
                        .AllowAnyMethod();     
                });
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            // Register controllers (API endpoints)
            services.AddControllers();
      
            // Register EF Core DbContext. Expects a connection string named "DefaultConnection" in configuration.
            services.AddDbContext<KanbanBoardDatabaseContext>(options =>
                options.UseNpgsql(GetDBConnectionString()));
        }

        private static string GetDBConnectionString()
        {
            var hostAddress = Environment.GetEnvironmentVariable("databaseHost");
            var dbUserName = Environment.GetEnvironmentVariable("databaseUsername");
            var dbName = Environment.GetEnvironmentVariable("databaseName");
            var dbPassword = Environment.GetEnvironmentVariable("databasePassword");
            return $"Host={hostAddress};Port=5432;Database={dbName};Username={dbUserName};Password={dbPassword}";
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<KanbanBoardDatabaseContext>();
                db.Database.Migrate();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => "Hello World");
                endpoints.MapControllers();
            });
        }
    }
}