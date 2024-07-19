using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.IO;
namespace TelegramBotInn
{
    class clsTelegramBot
    {
        private enum BotCommands
        {
            start,
            help,
            hello,
            inn,
            last,
            okved,
            ergul,
            none
        }
        private static BotCommands LastCommand = BotCommands.none;
        public async void Start(ITelegramBotClient Bot, Message Command)
        {
            string MessageChat = "Доброго времени суток! \nЯ телеграмм бот InfoCompany!" +
                                 "\nЯ могу предоставить информацию о компаниях по указанным ИНН!" +
                                 "\nВоспользуйтесь командой /help, чтобы узнать о доступных командах!";

            await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
            LastCommand = BotCommands.start;
        }
        public async void Help(ITelegramBotClient Bot, Message Command)
        {
            string MessageChat = "Набор возможных команд: \n" +
                                 "\n1. /start - Начать общение с ботом;" +
                                 "\n\n2. /help - Вывести справку о доступных командах;" +
                                 "\n\n3. /hello - Вывести ваше имя и фамилию, ваш email и ссылку на github;" +
                                 "\n\n4. /inn - Получить наименования и адреса компаний по ИНН;" +
                                 "\n\n5. /last - Повторить последнее действие бота;" +
                                 "\n\n6. /okved - Вывести коды (ОКВЕД) и виды деятельности компании по ИНН;" +
                                 "\n\n7. /egrul - Получить выписку из ЕГРЮЛ по ИНН;";


            await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
            LastCommand = BotCommands.help;
        }
        public async void Hello(ITelegramBotClient Bot, Message Command)
        {
            string MessageChat = "Меня зовут Овсяников Георгий!" +
                                 "\n\nМоя почта: what-skype-ok@mail.ru" +
                                 "\n\nСсылка на GitHub: https://github.com/KurisuMak1se/INNTelegramBot";


            await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
            LastCommand = BotCommands.hello;
        }
        public async void Inn(ITelegramBotClient Bot, Message Command)
        {
            clsDataBase DB = new clsDataBase();
            List<string[]> colRowsResult = new List<string[]>();
            string MessageChat;


            if (!mdlData.flgInputData)
            {
                MessageChat = "Введите ИНН компаний через запятую";


                await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                mdlData.flgInputData = true;
            }
            else
            {
                colRowsResult = DB.Inn(CheckInputData(Bot, Command));
                if (colRowsResult.Count != 0)
                {
                    MessageChat = "Информация о найденных компаниях представлена ниже: \n\n";
                    for (int i = 0; i < colRowsResult.Count; i++)
                    {
                        MessageChat += $"Наименование компании: {colRowsResult[i][0]} \n" +
                                        $"Адресс компании: {colRowsResult[i][1]} \n" +
                                        $"ИНН компании: {colRowsResult[i][2]} \n\n";
                    }
                    
                    await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }
                else
                {
                    MessageChat = "К сожалению, информация об указанных компаниях отсутствует";


                    await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }

                mdlData.flgInputData = false;
            }

            LastCommand = BotCommands.inn;
        }
        public void Last(ITelegramBotClient Bot, Message Command)
        {
            ChoiceCommand(Bot, Command);
        }
        public async void Okved(ITelegramBotClient Bot, Message Command)
        {
            clsDataBase DB = new clsDataBase();
            List<string[]> colRowsResult = new List<string[]>();
            string MessageChat;


            if (!mdlData.flgInputData)
            {
                MessageChat = "Введите ИНН компаний через запятую";

                await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                mdlData.flgInputData = true;
            }
            else
            {
                colRowsResult = DB.Okved(CheckInputData(Bot, Command));
                if (colRowsResult.Count != 0)
                {
                    MessageChat = "Информация о найденных компаниях представлена ниже: \n\n";
                    for (int i = 0; i < colRowsResult.Count; i++)
                    {
                        MessageChat += $"ОКВЕД компании: {colRowsResult[i][0]} \n" +
                                        $"Тип деятельности компании: {colRowsResult[i][1]} \n" +
                                        $"ИНН компании: {colRowsResult[i][2]} \n\n";
                    }


                    await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }
                else
                {
                    MessageChat = "К сожалению, информация об указанных компаниях отсутствует";

                    await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }

                mdlData.flgInputData = false;
            }
            LastCommand = BotCommands.okved;
        }
        public async void Egrul(ITelegramBotClient Bot, Message Command)
        {
            string[] ArrDirectory = Environment.CurrentDirectory.Split('\\');
            string PathProject = "";
            clsDataBase DB = new clsDataBase();
            List<string[]> colRowsResult = new List<string[]>();
            string MessageChat;

            if (!mdlData.flgInputData)
            {
                MessageChat = "Введите ИНН компаний через запятую";

                await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                mdlData.flgInputData = true;
            }
            else
            {
                colRowsResult = DB.Egrul(CheckInputData(Bot, Command));
                for (int i = 0; i < ArrDirectory.Length - 2; i++)
                {
                    if (i != ArrDirectory.Length - 3)
                    {
                        PathProject += ArrDirectory[i] + "\\";
                    }
                    else
                    {
                        PathProject += ArrDirectory[i];
                    }
                }
                if (colRowsResult.Count != 0)
                {
                    for (int i = 0; i < colRowsResult.Count; i++)
                    {
                        try
                        {
                            await Bot.SendDocumentAsync(Command.Chat, InputFile.FromStream(new FileStream(PathProject + colRowsResult[i][1],
                                                                FileMode.Open), $"EGRUL ИНН {colRowsResult[i][0]}  компании"));
                        }
                        catch
                        {
                            MessageChat = "Произошла ошибка со стороны сервера!\nПопробуйте воспользоваться командой позже!";

                            await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                        }

                    }
                }
                else
                {
                    MessageChat = "К сожалению, информация об указанных компаниях отсутствует";

                    await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }

                mdlData.flgInputData = false;
            }

            LastCommand = BotCommands.ergul;
            
        }
        private async void ErrorMessage(ITelegramBotClient Bot, Message Command)
        {
            string MessageChat = "Ошибка!" +
                                 "\n\nЗаписанной команды не существует!" +
                                 "\n\nВы можете просмотреть набор доступных команд с помощью /help.";


            await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
        }
        private string CheckInputData(ITelegramBotClient Bot, Message Command)
        {
            string[] ArrInn = Command.Text.Split(',');
            string InputData = "'";
            string MessageChat;
            for (int i = 0; i < ArrInn.Length; i++)
            {
                if (!UInt64.TryParse(ArrInn[i], out _) || ArrInn[i].Trim(' ').Length != 10)
                {
                    MessageChat = "Ошибка!" +
                                  $"\n\n{i + 1} ИНН по счёту был ввёден некорректно!";


                    Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                }
                else
                {
                    if (i != ArrInn.Length - 1)
                    {
                        InputData += ArrInn[i].Trim(' ') + "', '";
                    }
                    else
                    {
                        InputData += ArrInn[i].Trim(' ');
                    }
                }
            }
            InputData += "'";
            return InputData;
        }
        public async void ChoiceCommand(ITelegramBotClient Bot, Message Command)
        {
            string MessageChat;
            switch (LastCommand)
            {
                case BotCommands.start:
                    {
                        Start(Bot, Command);
                        break;
                    }
                case BotCommands.help:
                    {
                        Help(Bot, Command);
                        break;
                    }
                case BotCommands.hello:
                    {
                        Hello(Bot, Command);
                        break;
                    }
                case BotCommands.inn:
                    {
                        Inn(Bot, Command);
                        break;
                    }
                case BotCommands.okved:
                    {
                        Okved(Bot, Command);
                        break;
                    }
                case BotCommands.ergul:
                    {
                        Egrul(Bot, Command);
                        break;
                    }
                default:
                    {
                        MessageChat = "Ошибка!" +
                                      "\n\nЕщё не была введена ни одна команда!" +
                                      "\n\nВоспользуетесь другой командой из списка /help прежде, чем использовать /last снова";


                        await Bot.SendTextMessageAsync(Command.Chat, MessageChat);
                        break;
                    }
            }
        }
        public void IdentificationCommand(ITelegramBotClient Bot, Message Command)
        {
            switch (Command.Text)
            {
                case "/start":
                    {
                        Start(Bot, Command);
                        break;
                    }
                case "/help":
                    {
                        Help(Bot, Command);
                        break;
                    }
                case "/hello":
                    {
                        Hello(Bot, Command);
                        break;
                    }
                case "/inn":
                    {
                        Inn(Bot, Command);
                        break;
                    }
                case "/last":
                    {
                        Last(Bot, Command);
                        break;
                    }
                case "/okved":
                    {
                        Okved(Bot, Command);
                        break;
                    }
                case "/egrul":
                    {
                        Egrul(Bot, Command);
                        break;
                    }
                default:
                    {
                        ErrorMessage(Bot, Command);
                        break;
                    }
            }
        }
    }
}
