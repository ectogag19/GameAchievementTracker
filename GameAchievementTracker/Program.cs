using GameAchievementTracker.Data;
using GameAchievementTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add controllers and views
builder.Services.AddControllersWithViews();

// Add Razor Pages (required for Identity scaffolding)
builder.Services.AddRazorPages();

var app = builder.Build();

// Enable developer error page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Set up routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Required for Identity UI to work
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    if (!context.Games.Any())
    {
        var game1 = new Game
        {
            Title = "The Witcher 3",
            Description = "Epic open-world RPG",
            Achievements = new List<Achievement>
            {
                new Achievement { Title = "Butcher of Blaviken", Description = "Kill 500 enemies" },
                new Achievement { Title = "Master Witcher", Description = "Reach level 50" }
            }
        };
        
        var game2 = new Game
        {
            Title = "Stardew Valley",
            Description = "Peaceful farming simulator",
            Achievements = new List<Achievement>
            {
                new Achievement { Title = "Green Thumb", Description = "Harvest 100 crops" }
            }
        };
        
        context.Games.AddRange(game1, game2);
        context.SaveChanges();
    }
}

app.Run();