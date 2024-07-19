using System.Collections.Generic;
using Npgsql;
using System.Data;

namespace TelegramBotInn
{
    class clsDataBase
    {

        public List<string[]> Inn(string Command)
        {
            NpgsqlDataReader ReaderCommand;
            List<string[]> colRowsResult = new List<string[]>();
            NpgsqlConnection ConnectionDB = new NpgsqlConnection(mdlData.CONNECTION);
            ConnectionDB.Open();
            NpgsqlCommand CommandDB = ConnectionDB.CreateCommand();


            CommandDB.CommandType = CommandType.Text;
            CommandDB.CommandText = "SELECT" +
                                    " \"Name\" " +
                                    ", \"Address\" " +
                                    ", \"INN\"" +
                                    " FROM \"Company\" " +
                                    $" WHERE \"INN\" IN({Command});";

            ReaderCommand = CommandDB.ExecuteReader();
            while (ReaderCommand.Read())
            {
                colRowsResult.Add(new string[] { ReaderCommand.GetString(0)
                                               , ReaderCommand.GetString(1)
                                               , ReaderCommand.GetString(2)});
            }
            ReaderCommand.Close();
            ConnectionDB.Close();
            return colRowsResult;
        }
        public List<string[]> Okved(string Command)
        {
            NpgsqlDataReader ReaderCommand;
            List<string[]> colRowsResult = new List<string[]>();
            NpgsqlConnection ConnectionDB = new NpgsqlConnection(mdlData.CONNECTION);
            ConnectionDB.Open();
            NpgsqlCommand CommandDB = ConnectionDB.CreateCommand();


            CommandDB.CommandType = CommandType.Text;
            CommandDB.CommandText = "SELECT" +
                                    " \"OKVED\" " +
                                    ", \"TypeActivity\" " +
                                    ", \"INN\"" +
                                    " FROM \"Company\" " +
                                    $" WHERE \"INN\" IN({Command})" +
                                    $"ORDER BY \"TypeActivity\" desc;";

            ReaderCommand = CommandDB.ExecuteReader();
            while (ReaderCommand.Read())
            {
                colRowsResult.Add(new string[] { ReaderCommand.GetString(0)
                                               , ReaderCommand.GetString(1)
                                               , ReaderCommand.GetString(2)});
            }
            ReaderCommand.Close();
            ConnectionDB.Close();
            return colRowsResult;
        }
        public List<string[]> Egrul(string Command)
        {
            NpgsqlDataReader ReaderCommand;
            List<string[]> colRowsResult = new List<string[]>();
            NpgsqlConnection ConnectionDB = new NpgsqlConnection(mdlData.CONNECTION);

            ConnectionDB.Open();
            NpgsqlCommand CommandDB = ConnectionDB.CreateCommand();
            CommandDB.CommandType = CommandType.Text;
            CommandDB.CommandText = "SELECT" +
                                    " \"INN\"" +
                                    ", \"Egrul\"" +
                                    " FROM \"Company\" " +
                                    $" WHERE \"INN\" IN({Command});";

            ReaderCommand = CommandDB.ExecuteReader();
            while (ReaderCommand.Read())
            {
                colRowsResult.Add(new string[] { ReaderCommand.GetString(0)
                                               , ReaderCommand.GetString(1)});
            }
            ReaderCommand.Close();
            ConnectionDB.Close();
            return colRowsResult;
        }
    }
}
