using System.Diagnostics;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace test.MenuStates.States;

public class DownloadFileMenu : IStateMenu
{
    private readonly ITelegramBotClient _bot;
    private readonly DownloadService _downloadService;
    private const string Button1 = "Главное Меню";

    public DownloadFileMenu(ITelegramBotClient bot)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        _downloadService = new DownloadService(_bot);
    }

    public async Task ProcessMessage(Update update,
        TelegramBotMenuContext context)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                switch (update.Message.Type)
                {
                    case MessageType.Document:
                        await _downloadService.StartDownload(update);
                        await context.SetState(new StartState(_bot), update);
                        break;
                    case MessageType.Text:
                        switch (update.Message.Text)
                        {
                            case Button1:
                                await context.SetState(new StartState(_bot), update);
                                break;
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }
    public async Task SendStateMessage(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message.Chat.Id, GetStateTitle(), replyMarkup: GetKeyboard());
    }

    public static string GetStateTitle()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Меню загрузки файла:");
        sb.AppendLine("Отправь файл в сообщении, сейчас реализована поддержка только рейтинга выполнения планов");
        sb.AppendLine("!Важно! Если ты отправишь что-то кроме детализации продаж - то скорей всего бот сломается");
        return sb.ToString();
    }

    public static IReplyMarkup GetKeyboard()
    {
        return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new()
            {
                new KeyboardButton(Button1)
            }
        }
        );
    }
    
}