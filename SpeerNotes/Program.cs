using Serilog;
using SpeerNotes.Db;
using Microsoft.EntityFrameworkCore;
using SpeerNotes.Definitions;
using SpeerNotes.Services;
using SpeerNotes.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SpeerNotes;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using SpeerNotes.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

//DI
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection(nameof(JwtSetting)));
var _jwtSetting = builder.Configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
builder.Services.AddAuthentication().AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = _jwtSetting?.Issuer,
    ValidAudience = _jwtSetting?.Issuer,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting?.Key ?? ""))
});
var connectionString = builder.Configuration.GetConnectionString("NotesConnection");
builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Rate limit
var _rateLimitSetting = builder.Configuration.GetSection(nameof(RateLimitSetting)).Get<RateLimitSetting>();
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter(policyName: ApplicationConstants.RateLimitName, options =>
    {
        options.PermitLimit = _rateLimitSetting?.PermitLimit ?? 4;
        options.Window = TimeSpan.FromSeconds(_rateLimitSetting?.Window ?? 10);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = _rateLimitSetting?.QueueLimit ?? 2;
    });
});

//Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Speer Notes Api Service");
});

app.Run();
