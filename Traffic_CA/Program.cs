using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Traffic_CA.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LoadsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LoadsContext") ?? throw new InvalidOperationException("Connection string 'LoadsContext' not found.")));
builder.Services.AddDbContext<DocklotsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DocklotsContext") ?? throw new InvalidOperationException("Connection string 'DocklotsContext' not found.")));
builder.Services.AddDbContext<TimeSlotsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TimeSlotsContext") ?? throw new InvalidOperationException("Connection string 'TimeSlotsContext' not found.")));
builder.Services.AddDbContext<LotsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LotsContext") ?? throw new InvalidOperationException("Connection string 'LotsContext' not found.")));
builder.Services.AddDbContext<DoorsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DoorsContext") ?? throw new InvalidOperationException("Connection string 'DoorsContext' not found.")));
builder.Services.AddDbContext<DefaultTimesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultTimesContext") ?? throw new InvalidOperationException("Connection string 'DefaultTimesContext' not found.")));
builder.Services.AddDbContext<StatesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StatesContext") ?? throw new InvalidOperationException("Connection string 'StatesContext' not found.")));
builder.Services.AddDbContext<CustomersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersContext") ?? throw new InvalidOperationException("Connection string 'CustomersContext' not found.")));
builder.Services.AddDbContext<CoordinatorsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoordinatorsContext") ?? throw new InvalidOperationException("Connection string 'CoordinatorsContext' not found.")));
builder.Services.AddDbContext<CitiesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CitiesContext") ?? throw new InvalidOperationException("Connection string 'CitiesContext' not found.")));
builder.Services.AddDbContext<CarriersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarriersContext") ?? throw new InvalidOperationException("Connection string 'CarriersContext' not found.")));
builder.Services.AddDbContext<AssociatesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AssociatesContext") ?? throw new InvalidOperationException("Connection string 'AssociatesContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
