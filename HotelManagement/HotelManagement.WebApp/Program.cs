using HotelManagement.WebApp.Application.Facades;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services;
using HotelManagement.WebApp.Application.Services.Stays;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.DataSeeder;
using HotelManagement.WebApp.Persistance.DbContext;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.DAL;
using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//***********************Service Registration area****************************
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:HotelManagementSystemDb"])
);
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:AuthDb"])
);

builder.Services.AddIdentity<ApplicationEmployee, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IEmployeeDAL, EmployeeDAL>();
builder.Services.AddScoped<IRoomDAL, RoomDAL>();
builder.Services.AddScoped<ICabDriverDAL, CabDriverDAL>();
builder.Services.AddScoped<ICustomerDAL, CustomerDAL>();
builder.Services.AddScoped<IStayDAL, StayDAL>();
builder.Services.AddScoped<IDropPickRequestDAL, DropPickRequestDAL>();
builder.Services.AddScoped<IStayChargeCalculator, StayChargeCalculator>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ICabDriverService, CabDriverService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStayService, StayService>();
builder.Services.AddScoped<IDropPickRequestService, DropPickRequestService>();
builder.Services.AddScoped<IAdminServiceFacade, AdminServiceFacade>();
builder.Services.AddScoped<IReceptionistServiceFacade, ReceptionistServiceFacade>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/denied";
});


//************************Middleware********************
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    await SeedRunner.RunAsync(db);
    await IdentitySeedData.PopulateAsync(app);
}



app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//This is for test purpose only. It will be removed in production.**************8
//*****************************Configurations area **********************************

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
