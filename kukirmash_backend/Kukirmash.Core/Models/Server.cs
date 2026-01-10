namespace Kukirmash.Core.Models;

public class Server
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? IconPath { get; private set; }
    public bool IsPrivate { get; private set; }

    private Server(Guid id, string name, string? desc, string? iconPath, bool isPrivate)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = iconPath;
        IsPrivate = isPrivate;
    }
    public static Server Create(Guid id, string name, string? desc, string? iconPath, bool isPrivate)
    {
        return new Server(id, name, desc, iconPath, isPrivate);
    }
}
