namespace Kukirmash.API.Contracts.TextMessage;

public record GetTextMessageResponse
(
    Guid id,
    string text,
    DateTime createdDateTimeUtc
);
