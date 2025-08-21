using Microsoft.AspNetCore.Identity;

namespace GameAchievementTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserAchievement> UserAchievements { get; set; }
    }
}