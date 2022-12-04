var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.EnvironmentName != "Test")
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "4000";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();

public partial class Program
{ }