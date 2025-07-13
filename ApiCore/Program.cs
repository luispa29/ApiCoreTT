using ApiCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Token

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);


//JWT
var appSettings = appSettingsSection.Get<AppSettings>();
if (appSettings == null || string.IsNullOrEmpty(appSettings.Secret))
{
    throw new InvalidOperationException("AppSettings or Secret is not configured properly.");
}

var llave = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(d =>
    {
        d.RequireHttpsMetadata = false;
        d.SaveToken = true;
        d.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(llave),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure the HTTP request pipeline.
Dependency.AddDependencyDeclaration(builder.Services);

if (appSettings.Docker) builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("all");

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var errorResponse = new
        {
            message = ex.Message,
            stackTrace = app.Environment.IsDevelopment() ? ex.StackTrace : null
        };

        var json = System.Text.Json.JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(json);
    }
});


app.Run();
