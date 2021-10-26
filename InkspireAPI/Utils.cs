using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InkspireAPI
{
    public class Utils
    {
        public string getConStrSQL()
        {

            string connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                InitialCatalog = "UserDB",
                DataSource = ".",
                IntegratedSecurity = true,
            }.ConnectionString;

            return connectionString;
        }

        public JsonResult JsonResult(string query, IConfiguration configuration)
        {
            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("UserCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(getConStrSQL()))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
