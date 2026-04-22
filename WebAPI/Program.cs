using Application.Common.Interfaces;
using Application.Features.Etablissements.Commands.CreateEtablissement;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Notifications.Interfaces;
using Application.Features.Clients.Interfaces;
using Application.Features.Payements.Interfaces;
using Application.Features.Aviss.Interfaces;
using Application.Features.Paiements.Commands.InitialiserPaiement;
using Application.Features.Employees.Interfaces;
using Application.Features.Prestations.Interfaces;
using Application.Features.Users.Interfaces;
using Application.Features.Admin.Interfaces;

using Infrastructure.DBcontext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Service;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// ⚙️ CONFIG SERVEUR
// =======================================================
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

// =======================================================
// 🔐 JWT AUTHENTICATION
// =======================================================
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? "Clef_De_Secours_Tres_Longue_32_Caracteres_Minimum";

var key = Encoding.ASCII.GetBytes(jwtKey);

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
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

// =======================================================
// ✅ CORS — SOLUTION DÉFINITIVE FLUTTER WEB
// Flutter Web change de port à chaque hot restart →
// on autorise TOUS les ports sur localhost/127.0.0.1
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterPolicy", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                try
                {
                    var uri = new Uri(origin);
                    return uri.Host is "localhost" or "127.0.0.1" or "::1";
                }
                catch { return false; }
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =======================================================
// 🧠 CONTROLLERS
// =======================================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// =======================================================
// 📘 SWAGGER
// =======================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlowBook API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
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
            new string[] {}
        }
    });
});

// =======================================================
// 🗄️ DATABASE
// =======================================================
builder.Services.AddScoped<ApplicationDbContext>(provider =>
    new ApplicationDbContext(builder.Configuration));
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// =======================================================
// 🔧 REPOSITORIES + SERVICES
// =======================================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEtablissementRepository, EtablissementRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IRendezVousRepository, RendezVousRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAvisRepository, AvisRepository>();
builder.Services.AddScoped<IPrestationRepository, PrestationRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();
builder.Services.AddScoped<IPaymentService, CinetPayService>();

// =======================================================
// 🌍 HTTP CLIENT (Géocodage)
// =======================================================
builder.Services.AddHttpClient<IGeocodageService, GeocodageService>(client =>
{
    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "GlowBook/1.0");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// =======================================================
// 🧠 MEDIATR + AUTOMAPPER
// =======================================================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateEtablissementHandler).Assembly));
builder.Services.AddAutoMapper(typeof(CreateEtablissementHandler).Assembly);

// =======================================================
// 🚀 BUILD APP
// =======================================================
var app = builder.Build();

// =======================================================
// 🗄️ INIT DATABASE
// =======================================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    await dbContext.InitializeAsync();
}

// =======================================================
// 📘 SWAGGER DEV
// =======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlowBook API v1");
        c.RoutePrefix = string.Empty;
    });
}

// =======================================================
// 🌐 MIDDLEWARE — ORDRE DÉFINITIF
// ⚠️ L'ordre est CRITIQUE — ne pas changer
// =======================================================

// ✅ 1. CORS EN PREMIER (avant UseHttpsRedirection)
//    Raison : UseHttpsRedirection redirige HTTP→HTTPS avec un 307
//    Le navigateur perd les headers CORS sur cette redirection
//    → Flutter Web reçoit un CORS error avant même d'atteindre l'API
app.UseCors("FlutterPolicy");

// ✅ 2. HTTPS après CORS
//    Si tu développes en HTTP pur (flutter run -d chrome sans HTTPS),
//    tu peux commenter cette ligne temporairement
app.UseHttpsRedirection();

// ✅ 3. Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();