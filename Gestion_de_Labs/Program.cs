using Gestion_de_Labs.Data;
using Microsoft.EntityFrameworkCore;
using Gestion_de_Labs.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var cs = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// Updated upstream
builder.Services.AddScoped<LaboratorioService>();

// Stashed changes

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
