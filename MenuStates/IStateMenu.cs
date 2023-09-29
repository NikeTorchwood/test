using Telegram.Bot.Types;

namespace test.MenuStates;

public interface IStateMenu
{
    public Task ProcessMessage(Update update, TelegramBotMenuContext context);
    Task SendStateMessage(Update update);
}