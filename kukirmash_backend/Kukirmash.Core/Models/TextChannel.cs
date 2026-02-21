using System;

namespace Kukirmash.Core.Models;

public class TextChannel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid ServerId { get; private set; }

    private TextChannel(Guid id, string name, Guid serverId)
    {
        Id = id;
        Name = name;
        ServerId = serverId;
    }

    public static TextChannel Create(Guid id, string name, Guid serverId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя канала не может быть пустым", nameof(name));

        if (serverId == Guid.Empty)
            throw new ArgumentException("Канал должен принадлежать серверу", nameof(serverId));

        if (name.Length > 31)
            throw new ArgumentException("Имя канала слишком длинное", nameof(name));

        return new TextChannel(id, name, serverId);
    }
}
