using Microsoft.EntityFrameworkCore;
using TvMazeIntegration.Data;
using TvMazeIntegration.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TvMazeDb") ?? "Data Source=TvMazeDb.db";

builder.Services.AddLogging(builder => builder.AddConsole());

builder.Services.AddDbContext<TvMazeDb>(
    options =>
    {
        options.UseSqlite("Data Source=LocalDatabase.db",
            optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly("TvMazeIntegration.Api");
            }
        );
        options.EnableSensitiveDataLogging();
        
    }); //TIL DbContext is scoped. This trickles down to other server making use of this

builder.Services.AddScoped<IShowsService, ShowsService>(); 
builder.Services.AddScoped<IStatusService, StatusService>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
options.AddDefaultPolicy(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowAnyOrigin();
});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();