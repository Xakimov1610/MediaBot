using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace media_bot.Services
{
    public class BotHandlers
    {
        private readonly ILogger<BotHandlers> _logger;

        public BotHandlers(ILogger<BotHandlers> logger)
        {
            _logger = logger;
        }

        // Exception - Kutilmagan nimadur sodir bo'lsa Ogohlantirsh beradi, CancellationToken - 
        public Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ctoken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException => "Error occured with Telegram Client: {exception.Message}",
                _ => exception.Message
            };
            _logger.LogCritical(errorMessage);
            return Task.CompletedTask;
        }

        // o'zgarishlarni tutib olish uchun
        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ctoken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecived(client, update.Message),
                _ => UnknownUpdateHandler(client, update)
            };
            try
            {
                await handler;
            }
            catch(Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        private Task UnknownUpdateHandler(ITelegramBotClient client, Update update)
        {
            throw new Exception("Bu turdagi yangilanish mumkunmas");
        }

        private async Task BotOnMessageRecived(ITelegramBotClient client, Message? message)
        {
            if(message.Text == "/start")
            {
                await client.SendTextMessageAsync(
                    message.Chat.Id,
                    "Salom, bu bot siz izlagan rasm yoki videoni topib beradi."
                );
            }
            if(message.Text == "salom")
            {
                await client.SendTextMessageAsync(
                    message.Chat.Id,
                    "Gar saloming bo'lmasa internetingni o'chirar edim bo'tam"
                );
            }
        }
    }
}