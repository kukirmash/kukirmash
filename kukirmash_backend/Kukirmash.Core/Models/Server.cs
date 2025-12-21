namespace Kukirmash.Core.Models;

public class Server
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    Server(Guid id, string name, string desc)
    {
        Id = id;
        Name = name;
        Description = desc;
    }
    public static Server Create(Guid id, string name, string desc)
    {
        return new Server(id, name, desc);
    }
}
