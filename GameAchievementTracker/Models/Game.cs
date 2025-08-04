namespace GameAchievementTracker.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    }
}