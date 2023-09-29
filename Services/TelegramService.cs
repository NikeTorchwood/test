using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using test.MenuStates;
using test.Repository;
using test.Repository.UserRepository;

namespace test.Services;

public class TelegramService
{
    private readonly ITelegramBotClient _bot;
    private readonly IUserRepository _userRepository;
    private TelegramBotMenuContext? _context;
    public TelegramService(ITelegramBotClient bot, SqlConnectionProvider sqlConnectionProvider)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        _userRepository = new UserStateRepository(sqlConnectionProvider) ??
                          throw new ArgumentNullException(nameof(sqlConnectionProvider));
    }

    public void StartListening()
    {
        _context = new TelegramBotMenuContext(_bot, _userRepository);
        _bot.StartReceiving(UpdateHandler, ErrorHandler);
    }

   
    private async Task ErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        Console.WriteLine($"{DateTime.Now}: {update.Message.Chat.Id}");
        var message = update.Message;
        if (message != null)
        {
            Task.Run(async() =>
            {
                await _context.ProcessMessage(update);
            });
        }

        return Task.CompletedTask;
    }
}



//проверка что это не групповой чат

//получение юзера
//var user = message.From;
//проверка доступа, если проходит - некст стейт, если нет в заглушку
//if (await CheckUserAccess(bot, user))
//{

//}
//else
//{

//}
//private async Task<bool> CheckUserAccess(ITelegramBotClient bot, BotUser user)
//{
//    var isAccessAllowed = false;
//    //получение списка чатов бота из бд
//    foreach (var chat in chats)
//    {
//        var userStatus = await bot.GetChatMemberAsync(chat, user.Id);
//        isAccessAllowed = userStatus.Status switch
//        {
//            ChatMemberStatus.Administrator => true,
//            ChatMemberStatus.Creator => true,
//            ChatMemberStatus.Member => true,
//            _ => isAccessAllowed
//        };
//    }
//    return isAccessAllowed;
//}