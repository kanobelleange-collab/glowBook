using Application.Common.Interfaces;
using Application.Features.Etablissements.Commands.CreateEtablissement;
using Application.Features.Etablissements.Interfaces;
using Infrastructure.DBcontext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Application.Features.Employees.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// ✅ Désactiver validation au démarrage — test partiel
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes  = false;
    options.ValidateOnBuild = false;
});

// ✅ Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()));

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "GlowBook API",
        Version     = "v1",
        Description = "API de gestion des établissements esthétiques",
        Contact     = new OpenApiContact
        {
            Name  = "GlowBook",
            Email = "contact@glowbook.com"
        }
    });

    // ✅ Support JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.ApiKey,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Entrez : Bearer {votre token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ✅ Base de données// ✅ Passer IConfiguration explicitement
builder.Services.AddSingleton<IApplicationDbContext>(provider =>
    new ApplicationDbContext(
        builder.Configuration)); // ✅ IConfiguration passé directement

// ✅ Uniquement Etablissement pour les tests
builder.Services.AddScoped<IEtablissementRepository, EtablissementRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// ✅ Géocodage OpenStreetMap
builder.Services.AddHttpClient<IGeocodageService, GeocodageService>(client =>
{
    client.DefaultRequestHeaders.TryAddWithoutValidation(
        "User-Agent", "GlowBook/1.0 (contact@glowbook.com)");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// ✅ MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(CreateEtablissementHandler).Assembly));

// ✅ AutoMapper
builder.Services.AddAutoMapper(
    typeof(CreateEtablissementHandler).Assembly);

var app = builder.Build();

// ✅ Initialiser la BDD
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<IApplicationDbContext>();
    await dbContext.InitializeAsync();
}

// ✅ Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlowBook API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization(); // ✅ Authentication supprimée — pas de JWT pour les tests
app.MapControllers();
app.Run();