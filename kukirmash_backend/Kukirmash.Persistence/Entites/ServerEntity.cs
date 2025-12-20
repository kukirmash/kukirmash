namespace Kukirmash.Persistence.Entites;

public class ServerEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public UserEntity Creator { get; set; } = null!;
    public List<UserEntity> Users { get; set; } = [];
}
