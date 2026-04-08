using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Email settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// 🔥 CORS (FULL FIX)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🔥 ORDER IS CRITICAL
app.UseCors();          // apply globally FIRST

app.UseRouting();

app.MapControllers();

app.Run();