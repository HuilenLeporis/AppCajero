using AppCajero.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<DbcajeroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CajeroConnection"))
);

var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DbcajeroContext>();
    context.Database.Migrate();
}
*/

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Users}/{action=Ingreso}/{id?}");
    endpoints.MapControllerRoute(
        name: "Users",
        pattern: "{controller=Users}/{action=Balance}/{id?}");
    endpoints.MapControllerRoute(
        name: "Retiro",
        pattern: "{controller=Users}/{action=Retiro}/{id?}");
    endpoints.MapControllerRoute(
        name: "Aceptar",
        pattern: "{controller=Users}/{action=Aceptar}/{id?}");
    endpoints.MapControllerRoute(
        name: "Salir",
        pattern: "{controller=Users}/{action=Salir}/{id?}");
    endpoints.MapControllerRoute(
        name: "Operaciones",
        pattern: "{controller=Users}/{action=Operaciones}/{id?}");
});



app.Run();
