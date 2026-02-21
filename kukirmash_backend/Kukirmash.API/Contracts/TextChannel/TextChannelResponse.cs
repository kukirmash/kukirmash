using System;

namespace Kukirmash.API.Contracts.TextChannel;

public record TextChannelResponse
(
    Guid id,
    string name
);
