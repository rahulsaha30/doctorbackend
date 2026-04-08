using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Bind EmailSettings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ✅ CORS (FIXED)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()      // 🔥 TEMP: ensures it works 100%
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ✅ Middleware Order (CRITICAL)
app.UseRouting();

app.UseCors("AllowFrontend");   // MUST be here

app.UseAuthorization();

app.MapControllers();

app.Run();