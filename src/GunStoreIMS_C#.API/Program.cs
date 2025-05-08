using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Application.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- Configuration ----------------------
builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
       .AddUserSecrets<Program>(optional: true)
       .AddEnvironmentVariables();

// ---------------------- Services ---------------------------
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<FirearmsInventoryDB>(opt =>
    opt.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string is missing!")));

builder.Services.AddAutoMapper(typeof(Form4473MappingProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Firearms Inventory Management System API",
        Version = "v1",
        Description = "HTTP API for 4473 processing & bound-book operations"
    });
});

// ---------------------- Pipeline ---------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "FIMS API v1");
        ui.DocumentTitle = "FIMS API Explorer";
    });

    // ✅ This ensures "/" redirects to Swagger UI
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
