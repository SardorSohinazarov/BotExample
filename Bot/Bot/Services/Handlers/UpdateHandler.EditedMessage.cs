using Telegram.Bot.Types;
using Telegram.Bot;

namespace Bot.Services
{
    public partial class UpdateHandler
    {
        private async Task EditMessageAsync(ITelegramBotClient botClient, Message? editedMessage, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(editedMessage);

            var from = editedMessage.From;

            _logger.LogInformation("Recived edited message from : {from.Message}", from?.FirstName);
        }
    }
}
