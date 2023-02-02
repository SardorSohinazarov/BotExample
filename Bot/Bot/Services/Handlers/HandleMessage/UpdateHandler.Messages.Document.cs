using Telegram.Bot.Types;
using Telegram.Bot;

namespace Bot.Services;

public partial class UpdateHandler
{
    private async Task HandleDocumentAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendDefaultTextMessage(botClient, message, cancellationToken);
    }
}