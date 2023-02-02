using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading;

namespace Bot.Services
{
    public partial class UpdateHandler
    {
        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);

            var from = message.From;

            _logger.LogInformation("Recived message from : {from.Message}", from?.FirstName);

            var handler = message.Type switch
            {
                MessageType.Text => HandleTextMessageAsync(botClient, message, cancellationToken),
                MessageType.Location => HandleLocationAsync(botClient, message, cancellationToken),
                MessageType.Contact => HandleContactAsync(botClient, message, cancellationToken),
                MessageType.Audio => HandleAudioAsync(botClient, message, cancellationToken),
                MessageType.Sticker => HandlerStrikerAsync(botClient, message , cancellationToken),
                MessageType.Photo => HandlePhotoAsync(botClient, message , cancellationToken),
                MessageType.Dice => HandleDiceAsync(botClient, message , cancellationToken),
                MessageType.Document => HandleDocumentAsync(botClient, message, cancellationToken),
                MessageType.Game => HandleGameAsync(botClient, message, cancellationToken),
                MessageType.Invoice => HandleInvoiceAsync(botClient, message, cancellationToken),
                MessageType.Poll => HandlePollAsync(botClient, message, cancellationToken),
                MessageType.Voice => HandleVoiceAsync(botClient, message, cancellationToken), 
                MessageType.VideoNote => HandleVideoNote(botClient, message, cancellationToken),
                MessageType.WebAppData => HandleWebAppDataAsync(botClient, message, cancellationToken),
                _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
            };

            await handler;
        }

        private async Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task SendDefaultTextMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                text: $"{_stringLocalizer["greeting"]} {message.From.FirstName}    (<i>{message.Type}</i>) {_stringLocalizer["invalidformat"]}",
                parseMode: ParseMode.Html,
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
                );
        }
    }
}
