using DAL.Entities;
using ProjectCookie._src.dal.Entities;

namespace ProjectCookie._src.dal;

public class User(
    string password = null
    ) : Entity
{
    public string? Email { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Password { get; set; }
    public bool Active { get; set; }
}