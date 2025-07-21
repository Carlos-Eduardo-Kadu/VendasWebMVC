using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using VendasWebMvc.Data;
using VendasWebMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Configura o DbContext com Pomelo MySQL
builder.Services.AddDbContext<VendasWebMvcContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("VendasWebMvcContext"),
        ServerVersion.Create(8, 0, 34, ServerType.MySql),
        mySqlOptions => mySqlOptions.MigrationsAssembly("VendasWebMvc")
    )

);


// Injeta o servi�o de seeding
builder.Services.AddScoped<SeedingService>();

// injeta o servi�o Services
builder.Services.AddScoped<SellerService>();

// Adiciona servi�os MVC
builder.Services.AddControllersWithViews();

//injeta o servi�o DepartmentService
builder.Services.AddScoped<DepartmentService>();



var app = builder.Build();

// Configura��o do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Seguran�a: HTTP Strict Transport Security
}
else
{
    // Executa o seeding apenas no ambiente de desenvolvimento
    using (var scope = app.Services.CreateScope())
    {
        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions);
