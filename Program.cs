using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Unit11Bot.Controllers;
using Unit11Bot.Services;

namespace Unit11Bot;
public class Program
{
    public static async Task Main()
    {
        //Console.OutputEncoding = Encoding.Unicode;    //On debian bulsee not work

        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем

        Console.WriteLine("Сервис запущен");
        // Запускаем сервис
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        // Подключаем контроллеры сообщений и кнопок
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<VoiceMessageController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();
        //Хранилище
        services.AddSingleton<IStorage, MemoryStorage>();
        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5742466775:AAGPvRT2JKdy_rgXxkgeytKRoS6Ajpf_zdI"));
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }
}