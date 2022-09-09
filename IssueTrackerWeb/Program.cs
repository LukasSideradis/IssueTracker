using IssueTracker.DataAccess;
using IssueTracker.DataAccess.Repository.IRepository;
using IssueTracker.DataAccess.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using IssueTracker.Utility;
using IssueTracker.Utility.Options;
using Microsoft.Extensions.Options;
using IssueTracker.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureOptions<DatabaseOptionsSetups>();

builder.Services.AddDbContext<ApplicationDbContext>(
    (serviceProvider, options) =>
    {
        var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

        options.UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
        {
            sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);

            sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

        options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
        options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
SeedDatabase();

app.UseAuthentication();;
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "extra_param",
    pattern: "{area=Guest}/{controller=Home}/{action=Index}/{id?}/{param?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Guest}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}