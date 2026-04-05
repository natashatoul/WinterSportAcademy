using Microsoft.EntityFrameworkCore;
using WinterSportAcademy.Data;
using Microsoft.AspNetCore.Identity;
using WinterSportAcademy.Services;
using Microsoft.IdentityModel.Tokens;
using WinterSportAcademy.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using WinterSportAcademy.Repositories;

// program is the starting point of the application

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Winter Sport Academy API", 
        Version = "v1",
        Description = "API for managing trainees, instructors, and training sessions."
    });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddControllers();

builder.Services.AddDbContext<WinterSportAcademyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<WinterSportAcademyContext>().AddDefaultTokenProviders();
builder.Services.AddOptions<EmailSettings>()
    .Bind(builder.Configuration.GetSection("EmailSettings"))
    .ValidateDataAnnotations()
    .Validate(s => !string.IsNullOrWhiteSpace(s.SmtpServer), "EmailSettings:SmtpServer is required.")
    .Validate(s => !string.IsNullOrWhiteSpace(s.SmtpUsername), "EmailSettings:SmtpUsername is required.")
    .Validate(s => !string.IsNullOrWhiteSpace(s.SmtpPassword), "EmailSettings:SmtpPassword is required.")
    .ValidateOnStart();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RolesController>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
    var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
    if (jwtKey.Length < 32)
    {
        throw new InvalidOperationException("Jwt:Key must be at least 32 characters and should be loaded from environment variables.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//Dependency Injection
builder.Services.AddScoped<TraineeRepository>();
builder.Services.AddScoped<ITraineeService, TraineeService>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<TrainingSessionRepository>();
builder.Services.AddScoped<ITrainingSessionService, TrainingSessionService>();
builder.Services.AddScoped<EquipmentRepository>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<RegistrationRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Global Exception Handling Middleware to catch and log all unhandled exceptions.
// Promotes Clean Code and Separation of Concerns by removing try-catch blocks from controllers.
app.UseMiddleware<WinterSportAcademy.Middleware.ExceptionMiddleware>();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
app.MapControllers();
app.UseCors("AllowReactApp");
app.UseAuthentication();// Authentication is that everyone who has an account should be able to login.
app.UseAuthorization();//Authorisation is the one who has got the role or the privilege should be able to, you know, do certain bits.
app.MapHealthChecks("/health");



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
