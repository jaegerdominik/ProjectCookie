using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ProjectCookie.DAL.BaseClasses;

namespace ProjectCookie.DAL.Entities;

public class Score : Entity
{
    public int Points { get; set; }
    public DateTime Timestamp { get; set; }
    
    [ForeignKey("FK_User")]
    [JsonProperty(Required = Required.Default)]
    [JsonIgnore] public User User { get; set; }
    public int FK_User { get; set; }
}