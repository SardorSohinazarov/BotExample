using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Bot.Services
{
    public class BotBackgroundServices : BackgroundService
    {
        private readonly TelegramBotClient _client;
        private readonly ILogger<BotBackgroundServices> _logger;
        private readonly IUpdateHandler _handler;

        public BotBackgroundServices(
            ILogger<BotBackgroundServices> logger,
            TelegramBotClient client,
            IUpdateHandler handler)
        {
            _client = client;
            _logger = logger;
            _handler = handler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bot = await _client.GetMeAsync(stoppingToken);

            _logger.LogInformation("Bot started succesfully {bot.Username}", bot.Username);

            _client.StartReceiving(
                _handler.HandleUpdateAsync,
                _handler.HandlePollingErrorAsync,
                new ReceiverOptions() { ThrowPendingUpdates = true },
                stoppingToken
                );
        }
    }
}
