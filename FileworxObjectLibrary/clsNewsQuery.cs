using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileworxObjectClassLibrary
{
    public class clsNewsQuery
    {
        // Constants
        static string tableName = "T_NEWS";

        public List<clsNews> Run()
        {
            List<clsNews> allNews = new List<clsNews>();

            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"SELECT b1.ID, b1.C_DESCRIPTION, b1.C_CREATIONDATE, b1.C_MODIFICATIONDATE, b1.C_CREATORID, b2.C_NAME AS CREATORNAME , " +
                               $"b1.C_LASTMODIFIERID, b3.C_NAME AS LASTMODIFIERNAME, b1.C_NAME , b1.C_CLASSID, f1.C_BODY, {tableName}.C_CATEGORY " +
                               $"FROM {tableName} " +
                               $"INNER JOIN T_BUSINESSOBJECT b1 ON {tableName}.ID = b1.ID " +
                               $"INNER JOIN T_FILE f1 ON {tableName}.ID = f1.ID " +
                               $"Left JOIN T_BUSINESSOBJECT b2 ON b1.C_CREATORID = b2.ID " +
                               $"Left JOIN T_BUSINESSOBJECT b3 ON b1.C_LASTMODIFIERID = b3.ID ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clsNews news = new clsNews();

                            news.Id = new Guid(reader[0].ToString());

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                news.Description = (reader[1].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[2].ToString()))
                            {
                                news.CreationDate = DateTime.Parse(reader[2].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[3].ToString()))
                            {
                                news.ModificationDate = DateTime.Parse(reader[3].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[4].ToString()))
                            {
                                news.CreatorId = new Guid(reader[4].ToString());
                            }


                            if (!String.IsNullOrEmpty(reader[5].ToString()))
                            {
                                news.CreatorName = reader[5].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[6].ToString()))
                            {
                                news.LastModifierId = new Guid(reader[6].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[7].ToString()))
                            {
                                news.LastModifierName = reader[7].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[8].ToString()))
                            {
                                news.Name = reader[8].ToString();
                            }

                            int c = (int)(reader[9]);
                            news.Class = (clsBusinessObject.Type)c;

                            news.Body = reader[10].ToString();

                            news.Category = reader[11].ToString();

                            allNews.Add(news);
                        }
                    }
                }
            }
            return allNews;
        }
    }
}
