using test.MenuStates;

namespace test.Repository.UserRepository;

public interface IUserRepository
{
    StatesMenu GetState(long userId);
    Task SaveState(long userId, IStateMenu state);
}