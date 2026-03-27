using System;

namespace Kukirmash.API.Contracts.TextChannel;

public record GetTextChannelResponse
(
    Guid id,
    string name
);
