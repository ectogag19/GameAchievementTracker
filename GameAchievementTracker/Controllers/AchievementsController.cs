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
        
        [AllowAnonymous]
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
        
        [Authorize]
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
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var userAchievements = await _context.UserAchievements
                .Where(ua => ua.UserId == user.Id)
                .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
                .ToListAsync();

            return View(userAchievements);
        }
        
        [Authorize]
        public IActionResult Create([FromQuery] int gameId)
        {
            var achievement = new Achievement { GameId = gameId };
            return View(achievement);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,GameId")] Achievement achievement)
        {
            Console.WriteLine("Received POST to create: " + achievement.Title);

            if (ModelState.IsValid)
            {
                _context.Achievements.Add(achievement);
                await _context.SaveChangesAsync();

                var added = await _context.Achievements
                    .Where(a => a.GameId == achievement.GameId)
                    .ToListAsync();

                Console.WriteLine("Current achievements for game ID " + achievement.GameId + ":");
                foreach (var a in added)
                {
                    Console.WriteLine($"- {a.Title} (ID: {a.Id})");
                }

                return RedirectToAction("Details", "Games", new { id = achievement.GameId });
            }

            Console.WriteLine("ModelState is invalid. Errors:");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"- {state.Key}: {error.ErrorMessage}");
                }
            }

            return View(achievement);
        }
        
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
                return NotFound();

            return View(achievement);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,GameId")] Achievement achievement)
        {
            if (id != achievement.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Games", new { id = achievement.GameId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Achievements.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(achievement);
        }
        
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (achievement == null)
                return NotFound();

            return View(achievement);
        }
        
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Games", new { id = achievement.GameId });
        }

    }
}