using System.ComponentModel.DataAnnotations;

namespace GameAchievementTracker.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        
        public int GameId { get; set; }
        public Game? Game { get; set; }
    }
}