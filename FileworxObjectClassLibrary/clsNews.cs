using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsNews : clsFile
    {
        // Constants
        static string tableName = "T_NEWS";

        // Properties
        public string Category { get; set; }

        public clsNews()
        {
            Class = Type.News;
        }

        public override void Insert()
        {
            CreationDate = DateTime.Now;
            Description = Description.Replace("'", "''");
            Name = Name.Replace("'", "''");
            Body = Body.Replace("'", "''");
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO T_BUSINESSOBJECT (ID, C_DESCRIPTION, C_CREATIONDATE, C_CREATORID, C_NAME, C_CLASSID)" +
                               $"VALUES('{Id}', '{Description}', '{CreationDate}', '{CreatorId}', '{Name}', {(int)Class});" +
                               $"INSERT INTO T_FILE (ID, C_BODY) VALUES('{Id}', '{Body}');"+
                               $"INSERT INTO T_NEWS (ID, C_CATEGORY) VALUES('{Id}', '{Category}')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public override void Update()
        {   


            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"UPDATE T_BUSINESSOBJECT SET C_DESCRIPTION = '{Description}', C_CREATIONDATE = '{CreationDate}'," +
                               $"C_MODIFICATIONDATE = '{ModificationDate}', C_CREATORID= '{CreatorId}', C_LASTMODIFIERID= '{LastModifierId}', " +
                               $"C_NAME= '{Name}'  WHERE Id = '{Id}';" +
                               $"UPDATE T_FILE SET C_BODY = '{Body}' WHERE Id = '{Id}';"+
                               $"UPDATE T_NEWS SET C_CATEGORY = '{Category}' WHERE Id = '{Id}'";

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
                string query = $"SELECT ID, C_CATEGORY " +
                               $"FROM {tableName} " +
                               $"WHERE Id = '{Id}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Category = (reader[1].ToString());
                        }
                    }
                }
            }
        }
    }
}
