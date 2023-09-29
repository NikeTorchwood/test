using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace test.MenuStates.States
{
    public class DownloadService
    {
        private readonly ITelegramBotClient _bot;
        public DownloadService(ITelegramBotClient bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public async Task StartDownload(Update update)
        {

            var sw = new Stopwatch();
            sw.Restart();
            await _bot.SendTextMessageAsync(update.Message.Chat.Id,
                "Обновляю данные, дождись скачивания данных...",
                replyMarkup: new ReplyKeyboardRemove());
            var fileId = update.Message.Document.FileId;
            var fileInfo = await _bot.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
            var destinationFilePath = $"{Environment.CurrentDirectory}\\economic.xlsx";
            Console.WriteLine(destinationFilePath);
            var sw1 = new Stopwatch();
            sw1.Restart();
            try
            {
                var fileStream = new FileStream(destinationFilePath, FileMode.OpenOrCreate);
                await _bot.DownloadFileAsync(
                    filePath,
                    fileStream);
                sw1.Stop();
                await _bot.SendTextMessageAsync(update.Message.Chat.Id,
                    $"Скачивание файла произошло успешно. Время скачивания {sw1.Elapsed}");
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
