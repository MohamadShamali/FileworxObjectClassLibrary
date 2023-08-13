using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsUserQuery
    {
        // Constants
        static string tableName = "T_USER";

        public List<clsUser> Run()
        {
            List<clsUser> allUsers = new List<clsUser>();

            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"SELECT {tableName}.ID, C_DESCRIPTION, C_CREATIONDATE , C_MODIFICATIONDATE, C_CREATORID, C_LASTMODIFIERID, C_NAME, C_CLASSID , {tableName}.C_USERNAME, {tableName}.C_PASSWORD, {tableName}.ISADMIN " +
                               $"FROM {tableName} " +
                               $"INNER JOIN T_BUSINESSOBJECT ON {tableName}.ID = T_BUSINESSOBJECT.ID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clsUser user = new clsUser();

                            user.Id = new Guid(reader[0].ToString());

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                user.Description = (reader[1].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[2].ToString()))
                            {
                                user.CreationDate = DateTime.Parse(reader[2].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[3].ToString()))
                            {
                                user.ModificationDate = DateTime.Parse(reader[3].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[4].ToString()))
                            {
                                user.CreatorId = new Guid(reader[4].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[5].ToString()))
                            {
                                user.LastModifierId = new Guid(reader[5].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[6].ToString()))
                            {
                                user.Name = reader[6].ToString();
                            }

                            int c = (int)(reader[7]);
                            user.Class = (clsBusinessObject.Type) c;

                            user.Username = reader[8].ToString();

                            user.Password = reader[9].ToString();

                            user.IsAdmin = (bool) reader[10];

                            allUsers.Add(user);
                        }
                    }
                }
            }
            return allUsers;
        }
    }
}
