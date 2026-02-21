using Kukirmash.Core.Models;

namespace Kukirmash.Application.Interfaces.Repositories;

public interface ITextMessageRepository
{
    Task<TextMessage> Add(TextMessage message);


}
