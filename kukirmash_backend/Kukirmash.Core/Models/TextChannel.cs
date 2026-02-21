using System;

namespace Kukirmash.Core.Models;

public class TextChannel
{
    public Guid Id { get; set; }
    public string Name { get; private set; }

    private TextChannel( Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static TextChannel Create(Guid id, string name)
    {
        return new TextChannel(id, name);
    }
}
