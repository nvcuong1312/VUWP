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
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
            {
                db.Open();

                String tableDataBox = "CREATE TABLE IF NOT " +
                    "EXISTS DataBox (ID NVARCHAR(5) PRIMARY KEY, " +
                    "NameBox NVARCHAR(2048) NULL," +
                    "NameSubBox NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableDataBox, db);

                createTable.ExecuteReader();                
            }

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
            {
                db.Open();                

                String tableBookmark = "CREATE TABLE IF NOT " +
                    "EXISTS Bookmark (ID NVARCHAR(10), " +
                    "Title NVARCHAR(2048) NULL," +
                    "Page NVARCHAR(10) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableBookmark, db);
                createTable.ExecuteReader();
            }
            //AddData("10", "Cuong", "haha");
            //DeleteData("10");
            //DeleteData("207");
        }

        public static void DeleteDataBox(string ID)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
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

        public static void AddDataBox(string ID, string NameBox, string NameSubBox)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
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

        public static Dictionary<string, List<string>> GetDataBox()
        {
            Dictionary<string, List<string>> entries = new Dictionary<string, List<string>>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
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

        public static void DeleteDataBookmark(string ID, string Page)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Bookmark WHERE ID = @ID AND Page = @Page;";
                insertCommand.Parameters.AddWithValue("@ID", ID);
                insertCommand.Parameters.AddWithValue("@Page", Page);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static void AddDataBookmark(string ID, string Title, string Page)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Bookmark VALUES (@ID, @Title, @Page);";
                insertCommand.Parameters.AddWithValue("@ID", ID);
                insertCommand.Parameters.AddWithValue("@Title", Title);
                insertCommand.Parameters.AddWithValue("@Page", Page);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static Dictionary<string, string> GetDataBookmark()
        {
            Dictionary<string, string> entries = new Dictionary<string, string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteVozforumsUWP.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from Bookmark", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    var ID = query.GetString(0);
                    var Title = query.GetString(1);
                    var Page = query.GetString(2);
                    var Key = ID + "_" + Page;
                    if (!entries.ContainsKey(Key))
                    {
                        entries.Add(Key, Title);
                    }                    
                }

                db.Close();
            }

            return entries;
        }
    }
}
