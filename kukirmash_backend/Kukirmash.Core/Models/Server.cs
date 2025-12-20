namespace Kukirmash.Core.Models;

public class Server
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public User Creator { get; set; }
    public List<User> Users { get; private set; }
    
    //TODO: TextChats, VoiceChats
    
    Server(Guid id, string name, string desc, User creator, List<User> users)
    {
        Id = id;
        Name = name;
        Description = desc;
        Creator = creator;
        Users = users;
    }
    public static Server Create(Guid id, string name, string desc, User creator, List<User> users)
    {
        return new Server(id, name, desc, creator, users);
    }
}
