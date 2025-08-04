using GameAchievementTracker.Data;
using GameAchievementTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameAchievementTracker.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AchievementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (achievement == null)
                return NotFound();
            
            bool earned = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                earned = await _context.UserAchievements.AnyAsync(ua => ua.UserId == user.Id && ua.AchievementId == id);
            }
            
            ViewData["Earned"] = earned;
            
            return View(achievement);
        }

        public async Task<IActionResult> Earn(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
                return NotFound();
            
            var user = await _userManager.GetUserAsync(User);

            var alreadyEarned = await _context.UserAchievements
                .AnyAsync(ua => ua.UserId == user.Id && ua.AchievementId == id);

            if (!alreadyEarned)
            {
                var userAchievement = new UserAchievement
                {
                    UserId = user.Id,
                    AchievementId = id,
                    DateEarned = DateTime.UtcNow
                };
                
                _context.UserAchievements.Add(userAchievement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }
        
        public async Task<IActionResult> Index()
        {
            var achievements = await _context.Achievements
                .Include(a => a.Game)
                .ToListAsync();
            return View(achievements);
        }
        
        [HttpGet]
        public IActionResult Create(int gameId)
        {
            var achievement = new Achievement { GameId = gameId };
            return View(achievement);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,GameId")] Achievement achievement)
        {
            Console.WriteLine("Received POST to create: " + achievement.Title);

            if (ModelState.IsValid)
            {
                _context.Achievements.Add(achievement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Games", new { id = achievement.GameId });
            }

            Console.WriteLine("Model is NOT valid");
            return View(achievement);
        }
    }
}