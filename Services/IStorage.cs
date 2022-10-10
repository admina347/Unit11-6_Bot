using Unit11Bot.Models;

namespace Unit11Bot.Services;

public interface IStorage
{
    /// <summary>
    /// Получение сессии пользователя по идентификатору
    /// </summary>
    Session GetSession(long chatId);
}
