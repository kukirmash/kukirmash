namespace Kukirmash.API.Contracts.Server;

public record ServerResponse
(
    Guid id,
    string Name,
    string? Description,
    string? iconPath
);

