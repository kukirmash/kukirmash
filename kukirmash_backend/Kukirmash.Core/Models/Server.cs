namespace Kukirmash.Core.Models;

public class Server
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string? IconPath { get; private set; }

    Server(Guid id, string name, string desc, string? iconPath)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = iconPath;
    }
    public static Server Create(Guid id, string name, string desc, string? iconPath)
    {
        return new Server(id, name, desc, iconPath);
    }
}
