using ProjectCookie.DAL.BaseClasses;

namespace ProjectCookie.DAL.Entities;

public class Score : Entity
{
    public int Points { get; set; }
    public DateTime Timestamp { get; set; }
}