using API_LookUp.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
builder.Services.AddScoped<IGeminiService, GeminiService>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API Óticas Look Up");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/openapi/v1.json"))
   .ExcludeFromDescription();

app.Run();
