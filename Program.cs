using doctor.Model;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Email config
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://doctorfrontend-five.vercel.app") // <-- your frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // only if you need to send cookies/credentials
    });
});

var app = builder.Build();

// ✅ IMPORTANT ORDER
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();