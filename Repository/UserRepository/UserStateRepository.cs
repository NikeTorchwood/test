using Microsoft.Data.SqlClient;
using System.ComponentModel;
using test.MenuStates;

namespace test.Repository.UserRepository;

public class UserStateRepository : IUserRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public UserStateRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    private static int GetStatesType(IStateMenu state)
    {
        var result = StatesMenu.StartMenu;
        var name = TypeDescriptor.GetClassName(state.GetType());
        result = name switch
        {
            "test.MenuStates.States.StartMenu" => StatesMenu.StartMenu,
            "test.MenuStates.States.ChooseStoreMenu" => StatesMenu.ChooseStoreMenu,
            "test.MenuStates.States.DownloadFileMenu" => StatesMenu.DownloadMenu,
            "test.MenuStates.States.DirectionsControlState" => StatesMenu.DirectionsControlState,
            _ => result
        };
        return (int)result;
    }
    public StatesMenu GetState(long userId)
    {
        using var connection = (SqlConnection)_connectionProvider.GetConnection();
        const string query = "SELECT State FROM UserRepository WHERE UserId = @UserId";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        connection.Open();
        var result = command.ExecuteScalar();
        if (result == null) return 0;
        Console.WriteLine($"received: userid - {userId}, userstate - {result};");
        return (StatesMenu)result;
    }
    public async Task SaveState(long userId, IStateMenu state)
    {
        await using var connection = (SqlConnection)_connectionProvider.GetConnection();
        const string query = $@"
            MERGE INTO UserRepository AS Target
            USING (SELECT @UserId AS [UserId], @State AS [State]) AS Source
            ON (Target.[UserId] = Source.[UserId])
            WHEN MATCHED THEN
                UPDATE SET [State] = Source.[State]
            WHEN NOT MATCHED THEN
                INSERT ([UserId], [State])
                VALUES (Source.[UserId], Source.[State]);
        ";
        await using var command = new SqlCommand(query, connection);
        var resultState = GetStatesType(state);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@State", resultState);
        Console.WriteLine($"saved: userid - {userId}, userState - {resultState}");
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}