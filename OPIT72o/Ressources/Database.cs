using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OPIT72o.Ressources
{
    class Database
    {
        private const string ConnectionString = "Server=localhost; database=OPIT; UID=root; password=br-91no";

        private readonly MySqlConnection MySQLConnection;
        public readonly List<dynamic> Data;

        public string Error { get; private set; }

        public Database()
        {
            this.Data = new List<dynamic>();

            try
            {
                MySQLConnection = new MySqlConnection(ConnectionString);
            }
            catch (MySqlException e)
            {
                this.Error = e.Message;
            }

        }

        public bool Select(string query)
        {
            try
            {
                MySQLConnection.Open();
                Data.Clear();

                MySqlCommand mySqlCommand = MySQLConnection.CreateCommand();
                mySqlCommand.CommandText = query;

                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    var expando = new ExpandoObject();
                    var expandoDictionary = expando as IDictionary<String, object>;

                    for (int intIndex = 0; intIndex < reader.FieldCount; intIndex++)
                    {
                        expandoDictionary[reader.GetName(intIndex)] = reader[intIndex].ToString();
                    }

                    Data.Add(expando);
                }

                MySQLConnection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                this.Error = e.Message;
                MySQLConnection.Close();

                return false;
            }
            finally
            {
                MySQLConnection.Close();
            }
        }

        public bool SaveOrUpdate(string query)
        {
            try
            {
                MySQLConnection.Open();

                MySqlCommand mySQLCommand = MySQLConnection.CreateCommand();
                mySQLCommand.CommandText = query;

                int intUpdatedRows = mySQLCommand.ExecuteNonQuery();
                MySQLConnection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                this.Error = e.Message;

                MySQLConnection.Close();
                return false;
            }
        }
    }
}
