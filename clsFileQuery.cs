using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileworxObjectClassLibrary
{
    public class clsFileQuery
    {
        //Constats
        static string tableName = "T_FILE";
        public enum ClassIds { News = 2, Photos = 3 }

        // Properties
        public ClassIds[] QClasses { get; set; } = { ClassIds.News, ClassIds.Photos };

        public List<clsFile> Run()
        {
            List<clsFile> allFiles = new List<clsFile>();

            string condition1 = "b1.C_CLASSID = 0 OR ";
            string condition2 = "b1.C_CLASSID = 0 ";

            if (QClasses.Contains(ClassIds.News)) 
            {
                condition1 = "b1.C_CLASSID = 2 OR ";
            }

            if (QClasses.Contains(ClassIds.Photos))
            {
                condition2 = "b1.C_CLASSID = 3 ";
            }

            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"SELECT b1.ID, b1.C_DESCRIPTION, b1.C_CREATIONDATE, b1.C_MODIFICATIONDATE, b1.C_CREATORID, b2.C_NAME AS CREATORNAME , " +
                               $"b1.C_LASTMODIFIERID, b3.C_NAME AS LASTMODIFIERNAME, b1.C_NAME , b1.C_CLASSID, {tableName}.C_BODY " +
                               $"FROM {tableName} " +
                               $"INNER JOIN T_BUSINESSOBJECT b1 ON {tableName}.ID = b1.ID " +
                               $"Left JOIN T_BUSINESSOBJECT b2 ON b1.C_CREATORID = b2.ID " +
                               $"Left JOIN T_BUSINESSOBJECT b3 ON b1.C_LASTMODIFIERID = b3.ID " +
                               $"WHERE " + condition1 + condition2;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clsFile file = new clsFile();

                            file.Id = new Guid(reader[0].ToString());

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                file.Description = (reader[1].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[2].ToString()))
                            {
                                file.CreationDate = DateTime.Parse(reader[2].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[3].ToString()))
                            {
                                file.ModificationDate = DateTime.Parse(reader[3].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[4].ToString()))
                            {
                                file.CreatorId = new Guid(reader[4].ToString());
                            }


                            if (!String.IsNullOrEmpty(reader[5].ToString()))
                            {
                                file.CreatorName = reader[5].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[6].ToString()))
                            {
                                file.LastModifierId = new Guid(reader[6].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[7].ToString()))
                            {
                                file.LastModifierName = reader[7].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[8].ToString()))
                            {
                                file.Name = reader[8].ToString();
                            }

                            int c = (int)(reader[9]);
                            file.Class = (clsBusinessObject.Type)c;

                            file.Body = reader[10].ToString();

                            allFiles.Add(file);
                        }
                    }
                }
            }

            return allFiles;
        }



    }
}
