using Bot.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Services
{
    public partial class UpdateHandler : IUpdateHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UpdateHandler> _logger;
        private UserServices _userServices;
        private IStringLocalizer _stringLocalizer;
        public UpdateHandler(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<UpdateHandler> logger,
            IStringLocalizer<UpdateHandler> stringLocalizer)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }


        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Error occured with Telegram bot : {exception.Message}", exception.Message);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _userServices = scope.ServiceProvider.GetRequiredService<UserServices>();

            if (UpdateType.CallbackQuery == update.Type)
                await UpdateLanguageCodeFromCallBackAsync(botClient, update, cancellationToken);
            else
            {
                var user =  await _userServices.ConvertToEntity(update);

                if(!(await _userServices.Exists(user.UserId)))
                {
                    await _userServices.AddUserAsunc(user);
                }
                var culture = await GetUserCulture(update);
   

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                _stringLocalizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<Localizer>>();

                var handler = update.Type switch
                {
                    UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                    UpdateType.EditedMessage => EditMessageAsync(botClient, update.EditedMessage, cancellationToken),
                    UpdateType.CallbackQuery => HandleCallbackQuery(botClient, update, cancellationToken),
                    _ => HandleUnknownUpdate(botClient, update, cancellationToken)
                };
            }

        }

        private async Task UpdateLanguageCodeFromCallBackAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await _userServices.UpdateLanguageCode(update, update.CallbackQuery.Data);
            return;
        }

        private async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await _userServices.UpdateLanguageCode(update, update.CallbackQuery.Data);
        }

        private Task HandleUnknownUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update type {update.type} recived", update.Type);
            return Task.CompletedTask;
        }

        private async Task<CultureInfo> GetUserCulture(Update update)
        {
            var from = update.Type switch
            {
                UpdateType.Message => update.Message.From,
                UpdateType.EditedMessage => update.EditedMessage.From,
                UpdateType.CallbackQuery => update.CallbackQuery.From,
                UpdateType.InlineQuery => update.InlineQuery.From,
                UpdateType.ShippingQuery => update.ShippingQuery.From,
                UpdateType.PreCheckoutQuery => update.PreCheckoutQuery.From,
                UpdateType.ChosenInlineResult => update.ChosenInlineResult.From,
                _ => update.Message.From
            };

            var result = await _userServices.ConvertToEntity(update);

            _logger.LogInformation("New user added:{from.FirstName}", from.FirstName);

            var languageCode = await _userServices.GetLanguageCode(from.Id);

            _logger.LogInformation("Language set to : {languageCode}", languageCode);

            if (languageCode == null)
                return new CultureInfo("en-Us");
            return new CultureInfo(languageCode);
        }
    }
}
