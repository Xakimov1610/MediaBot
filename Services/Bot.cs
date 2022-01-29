using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace media_bot.Services;
public class Bot : BackgroundService
{
    private readonly TelegramBotClient _client;
    private ILogger<Bot> _logger;

    //IHosted Service Bot ran bolishi uchun  


    // Program cs da yaratgan clientimizni  qo'shdik , Logger - hatoliklarni ko'rsatish uchun , Botning vazifalari yozish uchun Handlers qo'shdik
    public Bot(TelegramBotClient client, ILogger<Bot> logger, BotHandlers handlers)
    {
        _client = client;
        _logger = logger;
        _client.StartReceiving(new DefaultUpdateHandler(handlers.HandleUpdateAsync, handlers.HandleErrorAsync));
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await _client.GetMeAsync();
        _logger.LogInformation($"{me.Username} has connected successfully");
    }
}