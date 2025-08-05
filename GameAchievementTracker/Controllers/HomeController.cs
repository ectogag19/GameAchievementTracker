using Microsoft.AspNetCore.Mvc;
using GameAchievementTracker.Models;
using GameAchievementTracker.Data;
using System.Linq;

namespace GameAchievementTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var games = _context.Games
                .Where(g => g.Id == null)
                .OrderByDescending(g => g.Id)
                .Take(3)
                .ToList();
            return View(games);
        }
    }
}