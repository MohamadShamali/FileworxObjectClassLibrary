using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsBusinessObject
    {
        // Constants
        static string tableName = "T_BUSINESSOBJECT";
        public enum Type { User = 1, News = 2, Photo = 3 }
        
        // Properties
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; } = null;
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public Guid? LastModifierId { get; set; } = null;
        public string LastModifierName { get; set; } = String.Empty;
        public string Name { get; set; }
        public Type Class { get; set; }

        public clsBusinessObject()
        {
            CreatorId = new Guid("ffd7c672-aa84-47b1-a9a3-c7875a503708"); // to remove
            LastModifierId = new Guid("ffd7c672-aa84-47b1-a9a3-c7875a503708"); // to remove
        }

        public virtual void Insert()
        {
            CreationDate = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO {tableName}(ID, C_DESCRIPTION, C_CREATIONDATE, C_CREATORID, C_NAME, C_CLASSID)" +
                                $"VALUES('{Id}', '{Description}', '{CreationDate}', '{CreatorId}', '{Name}', {(int) Class});";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        public virtual void Delete()
        {
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                string query = $"DELETE FROM {tableName} WHERE Id = '{Id}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public virtual void Update()
        {
            ModificationDate = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"UPDATE {tableName} SET C_DESCRIPTION = '{Description}', C_CREATIONDATE = '{CreationDate}'," +
                    $"C_MODIFICATIONDATE = '{ModificationDate}', C_CREATORID= '{CreatorId}', C_LASTMODIFIERID= '{LastModifierId}', " +
                    $"C_NAME= '{Name}'  WHERE Id = '{Id}'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public virtual void Read()
        {
            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();
                                        //0
                string query = $"SELECT b1.C_DESCRIPTION, b1.C_CREATIONDATE, b1.C_MODIFICATIONDATE, b1.C_CREATORID, b2.C_NAME AS CREATORNAME , " +
                               $"b1.C_LASTMODIFIERID, b3.C_NAME AS LASTMODIFIERNAME, b1.C_NAME , b1.C_CLASSID " +
                               $"FROM {tableName} b1 " +
                               $"Left JOIN {tableName} b2 ON b1.C_CREATORID = b2.ID " +
                               $"Left JOIN {tableName} b3 ON b1.C_LASTMODIFIERID = b3.ID "+
                               $"WHERE b1.ID= '{Id}';";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                            {
                                Description = (reader[0].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                CreationDate = DateTime.Parse(reader[1].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[2].ToString()))
                            {
                                ModificationDate = DateTime.Parse(reader[2].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[3].ToString()))
                            {
                                CreatorId = new Guid(reader[3].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[4].ToString()))
                            {
                                CreatorName = reader[4].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[5].ToString()))
                            {
                                LastModifierId = new Guid(reader[5].ToString());
                            }


                            if (!String.IsNullOrEmpty(reader[6].ToString()))
                            {
                                LastModifierName = reader[6].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[7].ToString()))
                            {
                                Name = reader[7].ToString();
                            }

                            int c = (int)(reader[8]);
                            Class = (Type)c;
                        }
                    }
                }
            }
        }

    }
}
