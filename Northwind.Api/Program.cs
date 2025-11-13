using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Northwind.Auth;
using Northwind.Dashboard.Handlers;
using Northwind.Models;
using Northwind.Models.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NorthwindContext>()
    .AddSignInManager();

// JWT Auth
var jwt = builder.Configuration.GetSection("Jwt");
var key = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SupplierOnly", p => p.RequireRole(Roles.Supplier));
    options.AddPolicy("Managers", p => p.RequireRole(Roles.Manager, Roles.Admin));
});

// Engine & helpers
builder.Services.AddScoped<Northwind.Dashboard.Engine.ConditionEvaluator>();
builder.Services.AddScoped<Northwind.Dashboard.Engine.DynamicDataService>();
builder.Services.AddScoped<Northwind.Dashboard.Engine.ParameterEvaluator>();
builder.Services.AddScoped<Northwind.Dashboard.Engine.PageExecutor>();
builder.Services.AddScoped<Northwind.Dashboard.Engine.DashboardRuntimeService>();

// Handlers
builder.Services.AddScoped<IPageItemHandler, LatestOrdersHandler>();
builder.Services.AddScoped<IPageItemHandler, SalesPerMonthHandler>();
builder.Services.AddScoped<IPageItemHandler, SalesPerRegionHandler>();
builder.Services.AddScoped<IPageItemHandler, TopCustomersHandler>();
builder.Services.AddScoped<IPageItemHandler, TopEmployeesHandler>();
builder.Services.AddScoped<IPageItemHandler, TopProductsHandler>();
builder.Services.AddScoped<IPageItemHandler, TotalSalesHandler>();
builder.Services.AddScoped<IPageItemHandler, TotalCustomersHandler>();
builder.Services.AddScoped<IPageItemHandler, TotalOrdersHandler>();
builder.Services.AddScoped<IPageItemHandler, SupplierTotalSalesHandler>();
builder.Services.AddScoped<IPageItemHandler, AvgOrderProcessingTimeHandler>();
builder.Services.AddScoped<IPageItemHandler, ProductLifecycleHandler>();
builder.Services.AddScoped<IPageItemHandler, EmployeeEfficiencyHandler>();
builder.Services.AddScoped<IPageItemHandler, SalesForecastHandler>();
builder.Services.AddScoped<IPageItemHandler, OutlierProductsHandler>();
builder.Services.AddScoped<IPageItemHandler, DiscountImpactHandler>();
builder.Services.AddScoped<IPageItemHandler, EmployeeWorkloadHandler>();




builder.Services.AddOpenApi();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

//async Task SeedAdminAsync(IApplicationBuilder app)
//{
//    using var scope = app.ApplicationServices.CreateScope();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    const string adminEmail = "admin@northwind.com";
//    const string adminPassword = "Admin123!";

//    // Skapa rollen om den inte finns
//    if (!await roleManager.RoleExistsAsync("Admin"))
//        await roleManager.CreateAsync(new IdentityRole("Admin"));

//    // Skapa användaren om den inte finns
//    var user = await userManager.FindByEmailAsync(adminEmail);
//    if (user == null)
//    {
//        user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
//        await userManager.CreateAsync(user, adminPassword);
//        await userManager.AddToRoleAsync(user, "Admin");
//    }
//}

//await SeedAdminAsync(app);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowReactApp");

app.Run();
