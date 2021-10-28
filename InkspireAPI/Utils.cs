using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Execution;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI
{
    public class Utils
    {
        public string getConStrSQL()
        {
            string connectionString = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "",
                Database = "Inkspire"
            }.ConnectionString;

            Console.WriteLine(connectionString);
            return connectionString;
        }

        public JsonResult JsonResult(string query)
        {
            DataTable table = new DataTable();
            MySqlDataReader reader;
            using (MySqlConnection con = new MySqlConnection(getConStrSQL()))
            {
                con.Open();
                using (MySqlCommand command = new MySqlCommand(query, con))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }
            return new JsonResult(table);
        }

        public string Error(string errorText)
        {
            return $"ERROR: {errorText}";
        }
    }
}
