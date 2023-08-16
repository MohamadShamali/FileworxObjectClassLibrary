using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsFile: clsBusinessObject
    {
        // Constants
        static string tableName = "T_FILE";

        // Properties
        public string Body { get; set; }

        public override void Insert()
        {
            CreationDate = DateTime.Now;
            string escapedDescription = Description.Replace("'", "''");
            string escapedName = Name.Replace("'", "''");
            string escapedBody = Body.Replace("'", "''");
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO T_BUSINESSOBJECT (ID, C_DESCRIPTION, C_CREATIONDATE, C_CREATORID, C_NAME, C_CLASSID)" +
                               $"VALUES('{Id}', '{Description}', '{CreationDate}', '{CreatorId}', '{Name}', {(int) Class});"+
                               $"INSERT INTO T_FILE (ID, C_BODY) VALUES('{Id}', '{Body}');";
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
                               $"C_NAME= '{Name}'  WHERE Id = '{Id}';"+
                               $"UPDATE T_FILE SET C_BODY = '{Body}' WHERE Id = '{Id}';";

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
                string query = $"SELECT ID, C_BODY " +
                               $"FROM {tableName} " +
                               $"WHERE Id = '{Id}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                Body = (reader[1].ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
