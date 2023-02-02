using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services;

public partial class UpdateHandler
{
    private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if(message.Text == "/start")
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Uzbek tili", callbackData: "uz-Latn-Uz"),
                    InlineKeyboardButton.WithCallbackData(text: "Enlish language", callbackData: "en-Us"),
                }
            });
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: _stringLocalizer["chooseLanguage"],
                parseMode: ParseMode.Html,
                replyMarkup: inlineKeyboard,
                cancellationToken:cancellationToken
                );
        }
        else
        {
            await SendDefaultTextMessage(botClient, message, cancellationToken);
        }
    }
}