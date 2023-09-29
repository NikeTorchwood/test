using Telegram.Bot;
using Telegram.Bot.Types;

namespace test.MenuStates.States;

public class ChangeDirectionList : IStateMenu
{
    public ChangeDirectionList(ITelegramBotClient bot)
    {
        throw new NotImplementedException();
    }

    public Task ProcessMessage(Update update, TelegramBotMenuContext context)
    {
        throw new NotImplementedException();
    }

    public Task SendStateMessage(Update update)
    {
        throw new NotImplementedException();
    }
}