using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolProcurement.Api.Service;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Infrastructure.Auditing;
using SchoolProcurement.Infrastructure.Middleware;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

// ----------------------------------------------------------------------
// Database Connection (SQL Auth – IIS safe)
// ----------------------------------------------------------------------
var connectionString =
    configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("SP_DEFAULT_CONNECTION")
    ?? throw new InvalidOperationException("Database connection string not configured");

// ----------------------------------------------------------------------
// Services
// ----------------------------------------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Audit interceptor MUST be singleton when using DbContextPool
builder.Services.AddSingleton<EntityAuditSaveChangesInterceptor>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddDbContextPool<SchoolDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(sp.GetRequiredService<EntityAuditSaveChangesInterceptor>());
});

// Application services
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMasterDetailService, MasterDetailService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductStockService, ProductStockService>();
builder.Services.AddScoped<ISorService, SorService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ISmtpEmailService, SmtpEmailService>();
builder.Services.AddScoped<ISorContactMappingService, SorContactMappingService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ISorChatService, SorChatService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// ----------------------------------------------------------------------
// JWT Authentication
// ----------------------------------------------------------------------
var jwtSection = configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var jwtSettings = jwtSection.Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings missing");

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) ||
    string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
    string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    throw new InvalidOperationException("JwtSettings are invalid");
}

var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            var logger = ctx.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogError(ctx.Exception, "JWT authentication failed");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ----------------------------------------------------------------------
// Swagger (Dev only)
// ----------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SOH API",
        Version = "v1"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ----------------------------------------------------------------------
// Build app
// ----------------------------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------------------------
// SAFE startup seeding (Development only)
// ----------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    try
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        await SchoolProcurement.Api.Hosting.RoleSeeder.SeedAsync(
            services,
            loggerFactory.CreateLogger("RoleSeeder"),
            CancellationToken.None
        );

        await SchoolProcurement.Api.Hosting.MasterDetailMultiSeeder.SeedAsync(
            services,
            loggerFactory.CreateLogger("MasterDetailSeeder"),
            CancellationToken.None
        );
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Startup seeding failed");
    }
}

// ----------------------------------------------------------------------
// Middleware pipeline (IIS/Plesk safe order)
// ----------------------------------------------------------------------
app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseAuthorization();

app.UseMiddleware<RequestCorrelationMiddleware>();
app.UseMiddleware<ApiAuditMiddleware>();
app.UseMiddleware<GlobalResponseMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SOH API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.Run();
