using GameAchievementTracker.Data;
using GameAchievementTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameAchievementTracker.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View(new List<Game>());
            }

            var games = await _context.Games
                .Where(g => g.UserId == user.Id)
                .ToListAsync();

            return View(games);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == user.Id);

            if (game == null)
                return NotFound();

            return View(game);
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Game game)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            game.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }
        
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == user.Id);

            if (game == null)
                return NotFound();

            return View(game);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageUrl")] Game game)
        {
            if (id != game.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var existingGame = await _context.Games.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == user.Id);

            if (existingGame == null)
                return NotFound();

            game.UserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }
        
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);

            if (game == null)
                return NotFound();

            return View(game);
        }
        
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == user.Id);

            if (game == null)
                return NotFound();

            _context.Achievements.RemoveRange(game.Achievements);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}