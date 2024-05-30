using dal.MongoDB;
using utils;

namespace dal;

public class User(
    string password = null
    ) : Entity
{
    public string? Email { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Password { get; set; }
    public string HashedPassword { get; set; } = new PasswordHasher().Hash(password);
    public bool Active { get; set; }
}