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
                .OrderByDescending(g => g.Id)  // or some date property if you have one
                .Take(3)
                .ToList();
            return View(games);
        }

    }
}