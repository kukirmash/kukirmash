namespace Kukirmash.Core.Models;

public class User
{
    public Guid Id { get; private set; }
    public string Login { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    private User(Guid id, string login, string passwordHash, string email)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
    }
    public static User Create(Guid id, string login, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Логин не может быть пустым", nameof(login));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email не может быть пустым", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Хэш пароля не может быть пустым", nameof(passwordHash));

        return new User(id, login, passwordHash, email);
    }
}
