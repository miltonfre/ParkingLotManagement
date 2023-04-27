using ParkingLotManagement.Application.Extensions;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.Services;
using ParkingLotManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


#region Repositories
builder.Services.AddSingleton<IParkingServices, ParkingServices>();
builder.Services.AddSingleton<IParkingRepository, ParkingRepository>();
builder.Services.AddSingleton<ICustomConfigureServices, CustomConfigureServices>();
//builder.Services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
//builder.Services.AddTransient<IParkingRepository, ParkingRepository>();
//builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
app.MigrateDatabase<Program>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
