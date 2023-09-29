using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace test.MenuStates.States;

public class StartState : IStateMenu
{
    private readonly ITelegramBotClient _bot;
    private const string Button1 = "Печать инструкции";
    private const string Button2 = "Выбрать отслеживаемый магазин";
    private const string Button3 = "Попасть в менеджер отчетов";
    private const string Button4 = "Попасть в менеджер KPI";

    public StartState(ITelegramBotClient bot)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
    }
    public async Task ProcessMessage(Update update,
        TelegramBotMenuContext context)
    {
        switch (update.Message.Type)
        {
            case MessageType.Text:
                switch (update.Message.Text)
                {
                    case Button3:
                        await context.SetState(new DownloadFileMenu(_bot), update);
                        break;
                    case Button2:
                        await context.SetState(new ChooseStoreMenu(_bot), update);
                        break;
                    case Button1:
                        await PrintInstructions(update);
                        break;
                    case Button4:
                        await context.SetState(new DirectionsControlState(_bot), update);
                        break;
                }
                break;
            case MessageType.Document:
                await _bot.SendTextMessageAsync(update.Message.Chat.Id,
                    "Возможно ты отправил отчет? Посмотри внимательно на кнопки или нажми инструкцию.");
                break;
        }
    }

    private async Task PrintInstructions(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message.Chat.Id, GetInstruction(), replyMarkup: GetKeyboard());
    }

    private static string GetInstruction()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Инструкция:");
        sb.AppendLine("Привет, здесь инструкция по использованию");
        sb.AppendLine(
            "Бот может печатать отчет по магазину, основывается он только на тех данных, которые мы туда загрузили.");
        sb.AppendLine("Если у тебя он печатает отчет не по твоему магазину - выбери заново свой магазин и повтори.");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("Немного о кнопочках");
        sb.AppendLine("=> Печать отчета: Печатает отчет по выбранному магазину;");
        sb.AppendLine(
            "=> Выбрать магазин: После загрузки файла берет актуальный список магазинов и выбирает магазин, который можешь выбрать;");
        sb.AppendLine(
            "=> Загрузить отчет: Меню куда мы скидываем детализацию. !Важно! Если ты отправишь что-то кроме детализации продаж - то скорей всего бот сломается");
        return sb.ToString();
    }

    public async Task SendStateMessage(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message.Chat.Id, GetStateTitle(), replyMarkup: GetKeyboard());
    }
    private static string GetStateTitle()
    {
        return "Главное меню бота";
    }

    public static IReplyMarkup GetKeyboard()
    {
        return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    new KeyboardButton(Button1),
                    new KeyboardButton(Button2)
                },
                //new List<KeyboardButton>(){
                //},
                new List<KeyboardButton>(){
                    new KeyboardButton(Button3),
                    new KeyboardButton(Button4),
                }

            }
        );
    }
}
