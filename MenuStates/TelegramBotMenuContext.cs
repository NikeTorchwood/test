using Telegram.Bot;
using Telegram.Bot.Types;
using test.MenuStates.States;
using test.Repository.UserRepository;

namespace test.MenuStates;

public class TelegramBotMenuContext
{
    private readonly ITelegramBotClient _bot;
    private IStateMenu _currentState;
    private readonly IUserRepository _userRepository;
    public TelegramBotMenuContext(ITelegramBotClient bot, IUserRepository userRepository)
    {
        _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    public async Task SetState(IStateMenu state, Update update)
    {
        await _userRepository.SaveState(update.Message.Chat.Id, state);
        _currentState = state;
    }
    public async Task ProcessMessage(Update update)
    {
        var user = update.Message.From.Id;
        var state = _userRepository.GetState(user);
        _currentState = GetState(state);
        await _currentState.ProcessMessage(update, this);
        await _currentState.SendStateMessage(update);
    }

    private IStateMenu GetState(StatesMenu state)
    {
        return state switch
        {
            StatesMenu.None => new StartState(_bot),
            StatesMenu.StartMenu => new StartState(_bot),
            StatesMenu.DownloadMenu => new DownloadFileMenu(_bot),
            StatesMenu.ChooseStoreMenu => new ChooseStoreMenu(_bot),
            StatesMenu.DirectionsControlState => new DirectionsControlState(_bot),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}