namespace Kukirmash.API.Contracts.Server;

public class AddServerRequest
{
    // Обязательное поле
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IFormFile? Icon { get; set; }
    public bool IsPrivate { get; set; } = false;
}

