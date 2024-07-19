using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotInn
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var Client = new TelegramBotClient(mdlData.TOKEN);
                Client.StartReceiving(OnUpdate, async (bot, ex, ct) => Console.WriteLine(ex));
                Console.WriteLine("Телеграм бот успешно запушен!\nНажмите на любую клавишу, чтобы выключить телеграм бота.");
            }
            catch
            {
                Console.WriteLine("Произошла ошибка на сервере!\n Попробуйте воспользоваться телеграмм ботом позже");
            }
            Console.ReadKey(true);

        }

        static async Task OnUpdate(ITelegramBotClient Bot, Update update, CancellationToken ct)
        {
            if (update.Message is null || update.Message.Text is null) return;
            clsTelegramBot TgBot = new clsTelegramBot();


            if(mdlData.flgInputData)
            {
                TgBot.ChoiceCommand(Bot, update.Message);
            }
            else
            {
                TgBot.IdentificationCommand(Bot, update.Message);
            }
        }
    }
}
