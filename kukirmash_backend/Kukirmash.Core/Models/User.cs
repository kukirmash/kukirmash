namespace Kukirmash.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public List<Server> Servers{ get; private set; }
    public List<Server> CreatedServers { get; private set; }


    private User(Guid id, string login, string passwordHash, string email, List<Server> servers, List<Server> createdServers)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
        Servers = servers;
        CreatedServers = createdServers;
    }
    public static User Create(Guid id, string login, string email,string passwordHash, List<Server> servers, List<Server> createdServers)
    {
        return new User(id, login, passwordHash, email, servers, createdServers);
    }
}
