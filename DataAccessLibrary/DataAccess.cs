using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS DataBox (ID NVARCHAR(5) PRIMARY KEY, " +
                    "NameBox NVARCHAR(2048) NULL," +
                    "NameSubBox NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
            //AddData("10", "Cuong", "haha");
            //DeleteData("10");
            //DeleteData("207");
        }

        public static void DeleteData(string ID)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM DataBox WHERE ID = @ID;";
                insertCommand.Parameters.AddWithValue("@ID", ID);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static void AddData(string ID, string NameBox, string NameSubBox)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO DataBox VALUES (@ID, @NameBox, @NameSubBox);";
                insertCommand.Parameters.AddWithValue("@ID", ID);
                insertCommand.Parameters.AddWithValue("@NameBox", NameBox);
                insertCommand.Parameters.AddWithValue("@NameSubBox", NameSubBox);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static Dictionary<string, List<string>> GetData()
        {
            Dictionary<string, List<string>> entries = new Dictionary<string, List<string>>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from DataBox", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(
                        query.GetString(0), 
                        new List<string>()
                        {
                            query.GetString(1),
                            query.GetString(2)
                        });
                }

                db.Close();
            }

            return entries;
        }
    }
}
