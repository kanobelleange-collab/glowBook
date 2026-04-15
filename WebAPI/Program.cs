using Application.Common.Interfaces;
using Application.Features.Etablissements.Commands.CreateEtablissement;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Notifications.Interfaces;
using Application.Features.Clients.Interfaces;
using Application.Features.Payements.Interfaces;
using Application.Features.Aviss.Interfaces;
using Application.Features.Employees.Interfaces;
using Application.Features.Prestations.Interfaces;
using Application.Features.Users.Interfaces;
using Application.Features.Admin.Interfaces;
using Infrastructure.DBcontext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Data;
using WebAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);

// --- ⚙️ CONFIGURATION SERVEUR ---
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

// --- 🔐 SÉCURITÉ JWT ---
var jwtKey = builder.Configuration["Jwt:Key"] ?? "GlowBook_Super_Secret_Key_2026_Secure_Long_String!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

// --- 🛠️ SERVICES & INJECTION ---

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlowBook API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez : Bearer {votre token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ✅ Persistence & Dapper
builder.Services.AddScoped<ApplicationDbContext>(provider => new ApplicationDbContext(builder.Configuration));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<ApplicationDbContext>().CreateConnection());

// ✅ Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEtablissementRepository, EtablissementRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IRendezVousRepository, RendezVousRepository>();
builder.Services.AddScoped<IAvisRepository, AvisRepository>();
builder.Services.AddScoped<IPrestationRepository, PrestationRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPaymentService, CinetPayService>();

// ✅ Géocodage
builder.Services.AddHttpClient<IGeocodageService, GeocodageService>(client =>
{
    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "GlowBook/1.0");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// ✅ MediatR & AutoMapper (Scan de l'assembly Application)
// On utilise CreateEtablissementHandler comme point d'ancrage pour scanner tout le projet Application
var applicationAssembly = typeof(CreateEtablissementHandler).Assembly;

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
builder.Services.AddAutoMapper(cfg => { }, applicationAssembly); // FIX : Réactivation d'AutoMapper

var app = builder.Build();

// --- 🌊 MIDDLEWARE PIPELINE ---

// ✅ Initialisation Auto de la BDD
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    await dbContext.InitializeAsync();
}

// 🚨 L'ExceptionHandlingMiddleware doit être le TOUT PREMIER pour tout attraper
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlowBook API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Ordre vital pour la sécurité et le routage
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();