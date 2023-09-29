using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace test.MenuStates.States;

public class ChooseStoreMenu : IStateMenu
{
    private readonly ITelegramBotClient _bot;

    public ChooseStoreMenu(ITelegramBotClient bot)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
    }
    public async Task ProcessMessage(Update update,
        TelegramBotMenuContext context)
    {
        switch (update.Message.Text)
        {
            case "Загрузить отчет":
                await context.SetState(new DownloadFileMenu(_bot), update);
                break;
            case "Главное меню":
                await context.SetState(new StartState(_bot), update);
                break;
        }
    }

    public async Task SendStateMessage(Update update)
    {
        await _bot.SendTextMessageAsync(update.Message.Chat.Id, GetStateTitle(), replyMarkup: GetKeyboard());
    }

    public string GetStateTitle()
    {
        return "state = ChooseStoreMenu";
    }

    public IReplyMarkup GetKeyboard()
    {
        return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>()
                {
                    //new KeyboardButton("Печатать отчет"),
                    new KeyboardButton("Главное меню")
                },
                //new List<KeyboardButton>(){
                //    new KeyboardButton("Инструкция")
                //},
                new List<KeyboardButton>(){
                    new KeyboardButton("Загрузить отчет")
                }

            }
        );
    }
}