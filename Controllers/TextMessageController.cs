using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Unit11Bot.Services;

namespace Unit11Bot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;
    string botAction;
    public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }
    public async Task Handle(Message message, CancellationToken ct)
    {
        try
        {
            botAction = _memoryStorage.GetSession(message.Chat.Id).BotAction;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        switch (message.Text)
        {
            case "/start":
                // Объект, представляющий кноки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($"Кол-во символов" , $"sLen"),
                        InlineKeyboardButton.WithCallbackData($"Сумма чисел" , $"nSum")
                    });
                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот считает количестов симоволов в строке.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Или вычисляет сумму чисел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                switch (botAction)
                {
                    case "sLen":
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                        break;
                    case "nSum":
                        //calc sum
                        string[] sNums = message.Text.Split(' ');
                        int[] nums = new int[sNums.Count()];

                        for (int i = 0; i < sNums.Length; i++)
                        {
                            //если число
                            if (ChekStr(sNums[i]))
                            {
                                nums[i] = Convert.ToInt32(sNums[i]);
                            }
                        }
                        Console.WriteLine("Сумма чисел: {0}", nums.Sum());
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {nums.Sum()}", cancellationToken: ct);
                        break;
                    default:
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте команду", cancellationToken: ct);
                        break;
                }
                break;
        }
    }
    //Проверка строки (исключить цифры)
    static bool ChekStr(string s)
    {
        foreach (var item in s)
        {
            if (char.IsDigit(item))
                return true; //если хоть один символ число, то выкидываешь true
        }
        return false; //если ни разу не выбило в цикле, значит, все символы - это буквы
    }
}