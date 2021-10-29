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
using SqlKata.Compilers;

namespace InkspireAPI
{
    public class Utils
    {
        MySqlCompiler compiler = new MySqlCompiler();
        QueryFactory queryFactory;
        MySqlConnection connection;

        public Utils()
        {
            connection = new MySqlConnection(getConStrSQL());
            queryFactory = new QueryFactory(connection, compiler);
        }

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

        public bool GuidIsUnique(string db, string stringID, string guid)
        {
            if (queryFactory.Query(db).Where(stringID, guid).Limit(1).Get().Count() == 0)
            {
                return true;
            }
            return false;
        }
    }
}
