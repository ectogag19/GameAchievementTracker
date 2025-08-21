using Microsoft.AspNetCore.Mvc;
using GameAchievementTracker.Models;
using GameAchievementTracker.Data;
using System.Linq;
using System.Security.Claims;


namespace GameAchievementTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<Game>());
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return View(new List<Game>());
            }

            var userGames = _context.Games
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.Id)
                .Take(3)
                .ToList();

            return View(userGames);
        }
    }
}