using Telegram.Bot;
using test.Repository;
using test.Services;

namespace test;

public class Program
{
    private static async Task Main(string[] args)
    {
        //todo: Найти способ преобразования текущего State в чтото понятное типо Enum
        //Console.WriteLine(TypeDescriptor.GetClassName(typeof(StartState)));
        //Console.WriteLine(TypeDescriptor.GetClassName(typeof(ChooseStoreMenu)));
        //Console.WriteLine(TypeDescriptor.GetClassName(typeof(DownloadFileMenu)));
        var token = "6206880800:AAEJhQglpNQApcq0w0wmJR8IgnI_QXmMKvM";
        var connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ntwdc\\source\\repos\\test\\StoreBot.mdf;Integrated Security=True";
        var sqlProvider = new SqlConnectionProvider(connectionString);
        var bot = new TelegramBotClient(token);
        var telegramService = new TelegramService(bot, sqlProvider);
        await telegramService.StartListening();

        Console.ReadKey();
    }
}
//-995662460