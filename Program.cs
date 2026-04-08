using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Email settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// 🔥 CORS Configuration (v2)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => true) // More robust origin matching
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Support cookies/auth headers if needed
        });
});

var app = builder.Build();

// 🔥 CRITICAL (v2): Move UseCors to the ABSOLUTE TOP of the pipeline
// This ensures headers are added before any other middleware (routing, auth, etc.) can interfere.
app.UseCors("AllowAll");

app.UseRouting();

app.MapControllers();

app.Run();