using Bot.Data;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = Bot.Entities.User;

namespace Bot.Services;

public class UserServices
{
    private readonly object _object = new object();
    private readonly BotDbContext _botDbContext;
    private readonly ILogger<UserServices> _logger;

    public UserServices(BotDbContext botDbContext, ILogger<UserServices> logger)
    {
        _botDbContext = botDbContext;
        _logger = logger;
    }

    public async Task AddUserAsunc(User user)
    {
        await _botDbContext.Users.AddAsync(user);
        await _botDbContext.SaveChangesAsync();
    }

    public async Task<bool> Exists(long userId)
    {
        var any = await _botDbContext.Users.AnyAsync(user => user.UserId == userId);
        return any;
    }

    public async Task<string?> GetLanguageCode(long? userId)
    {
        var user = await GetUserAsync(userId);
        return user.LanguageCode ?? "en-Us";
    }

    public async Task UpdateLanguageCode(Update update, string languageCode)
    {
        var user = await GetUserAsync(update.CallbackQuery.From.Id);

        user.LanguageCode = languageCode;
        _botDbContext.Users.Update(user);
        await _botDbContext.SaveChangesAsync();

        _logger.LogInformation("Language set:{languageCode}", languageCode);
    }

    private async Task<Entities.User> GetUserAsync(long? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(_botDbContext.Users);

        var user = await _botDbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);
        return user;
    }

    public async Task<User> ConvertToEntity(Update update)
    {
        User user = update.Type switch
        {
            UpdateType.Message => await MessageToUser(update.Message),
            UpdateType.CallbackQuery => await CallbackQueryToUser(update.CallbackQuery)
        };
        return user;
    }

    private async Task<User> CallbackQueryToUser(CallbackQuery? callbackQuery)
    {
        return new User
        {
            UserId = callbackQuery.From.Id,
            FirstName = callbackQuery.From.FirstName,
            LastName = callbackQuery.From.LastName,
            ChatId = callbackQuery.Message.Chat.Id,
            Username = callbackQuery.From.Username,
            CreatedAt = DateTimeOffset.Now,
            IsBot = callbackQuery.From.IsBot,
            LastInteractionAt = DateTimeOffset.Now,
            LanguageCode = callbackQuery.From.LanguageCode
        };
    }

    private async Task<User> MessageToUser(Message message)
    {
        return new User
        {
            UserId = message.From.Id,
            FirstName = message.From.FirstName,
            LastName = message.From.LastName,
            ChatId = message.Chat.Id,
            Username = message.From.Username,
            CreatedAt = DateTimeOffset.Now,
            IsBot = message.From.IsBot,
            LastInteractionAt = DateTimeOffset.Now
        };
    }
}
