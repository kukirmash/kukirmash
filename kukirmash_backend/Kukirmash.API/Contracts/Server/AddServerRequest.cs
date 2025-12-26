using System.ComponentModel.DataAnnotations;

namespace Kukirmash.API.Contracts.Server;

public record AddServerRequest
(
    [Required] string Name,
    [Required] string Description
);
