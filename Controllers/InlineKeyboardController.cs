using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Unit11Bot.Services;

namespace Unit11Bot.Controllers;

public class InlineKeyboardController
{
    private readonly IStorage _memoryStorage;
    private readonly ITelegramBotClient _telegramClient;

    public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery?.Data == null)
            return;

        // Обновление пользовательской сессии новыми данными
        _memoryStorage.GetSession(callbackQuery.From.Id).BotAction = callbackQuery.Data;
        // Генерим информационное сообщение
        string cmdText = callbackQuery.Data switch
        {
            "sLen" => "Кол-во символов",
            "nSum" => "Сумма",
            _ => String.Empty
        };

        // Отправляем в ответ уведомление о выборе
        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
            $"<b>Выбрать функцию - {cmdText}.{Environment.NewLine}</b>" +
            $"{Environment.NewLine}Можно в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
    }
}
