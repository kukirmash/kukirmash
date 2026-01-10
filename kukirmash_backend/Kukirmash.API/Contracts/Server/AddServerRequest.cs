using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.Server;

public class AddServerRequest
{
    // Обязательное поле
    [Required]
    public string Name { get; set; } = string.Empty;
    // Необязательное поле (биндер просто пропустит его, если ключа нет)
    public string? Description { get; set; }
    // Необязательное поле
    public IFormFile? Icon { get; set; }
}

// public record AddServerRequest
// (
//     [Required] string Name,

//     string? Description = null,

//     IFormFile? Icon = null
// );
