using System;
using System.Collections;
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
        public enum LogInValidationResult
        {
            WrongUser =0,
            WrongPassword =1,
            Valid =2
        }

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
                            IsAdmin = (bool)reader[3];
                        }
                    }
                }
            }
        }

        public LogInValidationResult ValidateLogin()
        {
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"SELECT C_PASSWORD " +
                               $"FROM T_USER " +
                               $"WHERE C_USERNAME='{Username}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = (reader[0].ToString());

                            if (storedPassword == Password)
                            {
                                return LogInValidationResult.Valid;
                            }

                            else
                            {
                                return LogInValidationResult.WrongPassword;
                            }

                        }
                        else
                        {
                            return LogInValidationResult.WrongUser;
                        }
                    }
                }
            }
        }
    }
}
