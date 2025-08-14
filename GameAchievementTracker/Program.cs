using GameAchievementTracker.Data;
using GameAchievementTracker.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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