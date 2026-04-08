using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Bind EmailSettings from appsettings.json
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ✅ CORS (for React frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ✅ Swagger (optional but useful)
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ✅ Enable Swagger

// ❌ Remove HTTPS if causing issues (optional)
// app.UseHttpsRedirection();

// ✅ Enable CORS
app.UseCors("AllowFrontend");

// ✅ Map Controllers
app.MapControllers();

app.Run();