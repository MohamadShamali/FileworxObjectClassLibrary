using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public enum LogInValidationResult
    {
        WrongUser = 0,
        WrongPassword = 1,
        Valid = 2
    }

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
            CreationDate = DateTime.Now;
            try
            {
                using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
                {
                    connection.Open();
                    string query = $"INSERT INTO T_BUSINESSOBJECT (ID, C_DESCRIPTION, C_CREATIONDATE, C_CREATORID, C_NAME, C_CLASSID)" +
                                   $"VALUES('{Id}', '{Description}', '{CreationDate}', '{CreatorId}', '{Name}', {(int)Class});"+
                                   $"INSERT INTO T_USER(ID, C_USERNAME, C_PASSWORD, ISADMIN) VALUES('{Id}', '{Username}', '{Password}', '{IsAdmin}');";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Error number for unique constraint violation
                {
                    throw new InvalidOperationException ("Duplicated username", ex);
                }

                else
                {
                    throw new InvalidOperationException("An error occurred while processing.", ex);
                }
            }

        }

        public override void Update()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
                {
                    connection.Open();

                    string query = $"UPDATE T_BUSINESSOBJECT SET C_DESCRIPTION = '{Description}', C_CREATIONDATE = '{CreationDate}'," +
                                   $"C_MODIFICATIONDATE = '{ModificationDate}', C_CREATORID= '{CreatorId}', C_LASTMODIFIERID= '{LastModifierId}', " +
                                   $"C_NAME= '{Name}'  WHERE Id = '{Id}';" +
                                   $"UPDATE T_USER SET C_USERNAME= '{Username}', C_PASSWORD= '{Password}', ISADMIN= '{IsAdmin}' WHERE Id = '{Id}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Error number for unique constraint violation
                {
                    throw new InvalidOperationException("Duplicated username", ex);
                }

                else
                {
                    throw new InvalidOperationException("An error occurred while processing.", ex);
                }
            }
        }

        public override void Read()
        {
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"SELECT ID, C_USERNAME, C_PASSWORD, ISADMIN " +
                               $"FROM {tableName} " +
                               $"WHERE ID = '{Id}' OR C_USERNAME = '{Username}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = new Guid(reader[0].ToString());
                            Username = (reader[1].ToString());
                            Password = (reader[2].ToString());
                            IsAdmin = (bool)reader[3];
                        }
                    }
                }
            }
            base.Read();
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
