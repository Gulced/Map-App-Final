using AutoMapper;
using MapApp.Application.Features.Areas.Handlers;
using MapApp.Application.Features.Points.Handlers;
using MapApp.Application.Features.Auth.Handlers;
using MapApp.Application.Interfaces;
using MapApp.Application.Mappings;
using MapApp.Infrastructure.Entities;
using MapApp.Infrastructure.Mappings;
using MapApp.Infrastructure.Persistence;
using MapApp.Infrastructure.Services;
using MapApp.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Database & DbContext ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseNetTopologySuite()
    )
);
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

// --- 2. Identity Configuration ---
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --- 3. JWT Settings & Services ---
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IIdentityService, IdentityService>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JWTSettings>()
    ?? throw new InvalidOperationException("JWT ayarları yüklenemedi.");

if (string.IsNullOrWhiteSpace(jwtSettings.Secret) || jwtSettings.Secret.Length < 16)
    throw new InvalidOperationException("JWT Secret appsettings.json içinde en az 16 karakter olmalı.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = signingKey
    };
});

// --- 4. AutoMapper Profiles ---
builder.Services.AddAutoMapper(
    Assembly.GetAssembly(typeof(UserMappingProfile)),
    Assembly.GetAssembly(typeof(AreaMappingProfile)),
    Assembly.GetAssembly(typeof(AppUserMappingProfile))
);

// --- 5. MediatR Handlers ---
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateAreaCommandHandler).Assembly,
        typeof(CreatePointCommandHandler).Assembly,
        typeof(RegisterUserCommandHandler).Assembly
    );
});

// --- 6. CORS for Frontend ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --- 7. Swagger & JWT Support ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapApp API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- 8. Controllers & JSON Options ---
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });

var app = builder.Build();

// --- 9. Development Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- 10. Middleware Pipeline ---
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// --- 11. Seed Roles & Admin User ---
await SeedRolesAndAdminUserAsync(app.Services);

app.Run();

static async Task SeedRolesAndAdminUserAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    foreach (var role in new[] { "USER", "ADMIN" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
    }

    const string adminEmail = "admin@admin.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = "admin",
            Email = adminEmail,
            FullName = "Administrator"
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
    }
    else
    {
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);
        await userManager.ResetPasswordAsync(adminUser, resetToken, "Admin123!");
    }
}
