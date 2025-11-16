namespace Kukirmash.Core.Models;

public class User
{
    private User(Guid id, string login, string passwordHash, string email)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; set; }
    public string Login { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public static User Create(Guid id, string login, string email,string passwordHash)
    {
        return new User(id, login, passwordHash, email);
    }
}
