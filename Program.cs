using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Email settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// 🔥 CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelFrontend",
        policy =>
        {
            policy.WithOrigins("https://doctorfrontend-five.vercel.app", 
                               "http://localhost:5173", 
                               "http://localhost:3000") // Added local dev just in case
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseRouting();

// 🔥 ORDER IS CRITICAL: UseCors must be after UseRouting but before MapControllers
app.UseCors("AllowVercelFrontend");

app.MapControllers();

app.Run();