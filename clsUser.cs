using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsUser : clsBusinessObject
    {
        // Constants
        static string tableName = "T_USER";

        // Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public clsUser()
        {
            Class = Type.User;
        }

        public override void Insert()
        {
            base.Insert();

            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO {tableName}(ID, C_USERNAME, C_PASSWORD, ISADMIN) VALUES('{Id}', '{Username}', '{Password}', '{IsAdmin}')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public override void Update()
        {
            base.Update();
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"UPDATE {tableName} SET C_USERNAME= '{Username}', C_PASSWORD= '{Password}', ISADMIN= '{IsAdmin}' WHERE Id = '{Id}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public override void Read()
        {
            base.Read();
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"SELECT ID, C_USERNAME, C_PASSWORD, ISADMIN " +
                               $"FROM {tableName} " +
                               $"WHERE Id = '{Id}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Username = (reader[1].ToString());
                            Password = (reader[2].ToString());
                            IsAdmin = (bool) reader[3];
                        }
                    }
                }
            }
        }
    }
}
