using Telegram.Bot.Types;
using Telegram.Bot;

namespace Bot.Services;

public partial class UpdateHandler
{
    private async Task HandlePollAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendDefaultTextMessage(botClient, message, cancellationToken);
    }
}