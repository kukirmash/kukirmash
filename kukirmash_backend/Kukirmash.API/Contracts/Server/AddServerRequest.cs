using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.Server;

public class AddServerRequest
{
    // Обязательное поле
    [Required] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IFormFile? Icon { get; set; }
    [Required] public bool IsPrivate { get; set; } = false;
}

