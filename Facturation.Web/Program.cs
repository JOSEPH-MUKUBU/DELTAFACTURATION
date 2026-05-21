using Facturation.Web;
using Facturation.Web.Components;
using Facturation.Infrastructure.Data;
using MudBlazor.Services;
using Radzen;

using Facturation.Core.Interfaces;
using Facturation.Infrastructure.Repositories;
using Facturation.Services.Services;
using Microsoft.EntityFrameworkCore;
using Facturation.Web.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddRadzenComponents();

// Configure Entity Framework Core with MySQL Pomelo
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFactureRepository, FactureRepository>();

// Configure Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<IFactureService, FactureService>();
builder.Services.AddScoped<IAnalyseService, AnalyseService>();
builder.Services.AddScoped<IParametreService, ParametreService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Peupler la base de données avec des données de démonstration
await DatabaseSeeder.SeedAsync(app.Services);

app.Run();
