using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace test.MenuStates.States
{
    public class DirectionsControlState : IStateMenu
    {
        private const string Button1 = "Посмотреть список KPI";
        private const string Button2 = "Посмотреть список исключений";
        private const string Button3 = "Изменить список KPI";
        private const string Button4 = "Изменить список исключений";
        private const string Button5 = "Главное меню";
        private readonly ITelegramBotClient _bot;
        public DirectionsControlState(ITelegramBotClient bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }
        public async Task ProcessMessage(Update update, TelegramBotMenuContext context)
        {
            switch (update.Message.Text)
            {
                case Button1:
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "method Show KPI");
                    break;
                case Button2:
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "method Show Exclusions");
                    break;
                case Button3:
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Изменить список KPI");
                    //await context.SetState(new ChangeDirectionList(_bot), update);
                    break;
                case Button4:
                    await _bot.SendTextMessageAsync(update.Message.Chat.Id, "Изменить список исключений");
                    //await context.SetState(new ChangeExclusionList(_bot), update);
                    break;
                case Button5:
                    await context.SetState(new StartState(_bot), update);
                    break;
            }
        }

        public async Task SendStateMessage(Update update)
        {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id, GetStateTitle(), replyMarkup: GetKeyboard());
        }
        private string GetStateTitle()
        {
            return "Меню управления KPI";
        }

        public IReplyMarkup GetKeyboard()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>()
                {
                    new List<KeyboardButton>()
                    {
                        new KeyboardButton(Button1),
                        new KeyboardButton(Button2)
                    },
                    new List<KeyboardButton>(){
                        new KeyboardButton(Button3),
                        new KeyboardButton( Button4)
                    },
                    new List<KeyboardButton>(){
                        new KeyboardButton(Button5)
                    }

                }
            );
        }
    }
}
