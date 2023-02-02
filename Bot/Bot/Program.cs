using Bot.Data;
using Bot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContextPool<BotDbContext>(option =>
//    option.UseSqlServer(builder.Configuration.GetConnectionString("BotDb")));

builder.Services.AddDbContext<BotDbContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("BotDb")),
         ServiceLifetime.Transient);


builder.Services.AddDbContext<BotDbContext>(ServiceLifetime.Transient);

var token = builder.Configuration.GetValue("BotToken", string.Empty);

#region Localization
builder.Services.AddLocalization();

var localizationOptions = new RequestLocalizationOptions();

var supportedCultures = new[] {
    new CultureInfo("uz-Latn-UZ"),
    new CultureInfo("en-Us")
};

localizationOptions.SupportedCultures = supportedCultures;
localizationOptions.SupportedUICultures = supportedCultures;
localizationOptions.SetDefaultCulture("en-Us");
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
#endregion

builder.Services.AddSingleton(new TelegramBotClient(token));

builder.Services.AddSingleton< IUpdateHandler , UpdateHandler>();

builder.Services.AddHostedService<BotBackgroundServices>();

builder.Services.AddScoped<UserServices>();

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

app.Run();
