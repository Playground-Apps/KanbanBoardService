using KanbanBoardService;

var builder = WebApplication.CreateBuilder(args);

// Use Startup-style registration explicitly (UseStartup is not supported on WebApplicationBuilder.WebHost)
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure pipeline via Startup
startup.Configure(app, app.Environment);

app.Run();
