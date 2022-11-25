using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using TvMazeIntegration.Data;
using TvMazeIntegration.Models;
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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //TODO: Fix this adding specific classes 
});

var assemblyList = new List<Assembly> { typeof(ModelMapperProfile).Assembly };
assemblyList.AddRange(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(assemblyList);

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
builder.Services.AddSwaggerGen(options =>
    {
        options.CustomOperationIds(apiDesc =>
        {
            // use ControllerName_Method as operation id. That will group the methods in the generated client
            if (apiDesc.ActionDescriptor is ControllerActionDescriptor desc)
            {
                var operationAttribute = (desc.EndpointMetadata
                    .FirstOrDefault(a => a is SwaggerOperationAttribute) as SwaggerOperationAttribute);
                return $"{desc.ControllerName}_{operationAttribute?.OperationId ?? desc.ActionName}";
            }

            // otherwise get the method name from the methodInfo
            var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
            apiDesc.TryGetMethodInfo(out MethodInfo methodInfo);
            var methodName = methodInfo?.Name ?? null;
            return $"{controller}_{methodName}";
        });
    }
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();