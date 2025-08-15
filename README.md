<img width="1433" height="691" alt="Screenshot 2025-08-14 at 6 42 43 PM" src="https://github.com/user-attachments/assets/ea247ea8-29e8-4412-99f2-c48ce5d82e1d" />

# 🎮 Game Achievement Tracker  

A web application built with **ASP.NET Core MVC** that allows users to track games they own and the achievements they’ve earned.  
Each user can log in, add their own games, view and manage achievements, and track progress separately.  

---

## 🚀 Features  
- **User Accounts & Authentication** — Users can register, log in, and log out securely using ASP.NET Core Identity.  
- **Personal Game Library** — Each user has their own list of games.  
- **Achievement Tracking** — Track progress for each game’s achievements.  
- **SQLite Database** — Lightweight and easy to set up.  
- **Pre-Seeded Data** — Sample games and achievements so you can test right away.  
- **Responsive UI** — Works on desktop and mobile.  

---

## 🛠 Tech Stack  
| Layer | Technology |
|-------|------------|
| **Backend** | ASP.NET Core MVC (C#) |
| **Frontend** | Razor Views, Bootstrap (or custom CSS) |
| **Database** | SQLite with Entity Framework Core |
| **Authentication** | ASP.NET Core Identity |
| **IDE** | JetBrains Rider (macOS) or Visual Studio |
| **OS Compatibility** | macOS, Windows, Linux |

---

## 📂 Project Structure  
```plaintext
GameAchievementTracker/
├── Controllers/      # Handles requests & responses (e.g., GamesController, AchievementsController)
├── Models/           # Data structures (Game, Achievement, ApplicationUser, UserAchievement)
├── Data/             # Database context and EF Core migrations
├── Views/            # Razor views for displaying pages
├── wwwroot/          # Static files (CSS, JavaScript, images)
├── Program.cs        # App startup configuration
└── appsettings.json  # Configuration (database connection, Identity settings)
