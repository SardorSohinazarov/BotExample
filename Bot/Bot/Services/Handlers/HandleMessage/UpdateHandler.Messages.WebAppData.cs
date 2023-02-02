using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Services;

public partial class UpdateHandler
{
    private async Task HandleWebAppDataAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendDefaultTextMessage(botClient, message, cancellationToken);
    }
}