using HotelManagement.WebApp.Persistance.DataSeeder;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//***********************Service Registration area****************************
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:HotelManagementSystemDb"])
);



//************************Middleware********************
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    await SeedRunner.RunAsync(db);
}


app.UseStaticFiles();
app.UseRouting();


//*****************************Configurations area **********************************
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
