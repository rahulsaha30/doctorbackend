using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Bind EmailSettings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173",
                "https://doctorfrontend-five.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // 🔥 IMPORTANT
    });
});

var app = builder.Build();

// ✅ ORDER MATTERS 🔥
app.UseRouting();

app.UseCors("AllowFrontend"); // must be after routing

app.UseAuthorization();

app.MapControllers();

app.Run();