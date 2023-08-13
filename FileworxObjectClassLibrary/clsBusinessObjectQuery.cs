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
    public class clsBusinessObjectQuery
    {
        // Constants
        static string tableName = "T_BUSINESSOBJECT";
        public enum ClassIds { Users = 1, News = 2, Photos = 3 }

        // Properties
        public ClassIds[] QClasses { get; set; } = {ClassIds.Users , ClassIds.News , ClassIds.Photos};
        

        public List<clsBusinessObject> Run()
        {
            List<clsBusinessObject> allBusinessObjects = new List<clsBusinessObject>();

            string condition1 = "b1.C_CLASSID = 0 OR ";
            string condition2 = "b1.C_CLASSID = 0 OR ";
            string condition3 = "b1.C_CLASSID = 0 ";

            if (QClasses.Contains(ClassIds.Users))
            {
                condition1 = "b1.C_CLASSID = 1 OR ";
            }

            if (QClasses.Contains(ClassIds.News))
            {
                condition2 = "b1.C_CLASSID = 2 OR ";
            }

            if (QClasses.Contains(ClassIds.Photos))
            {
                condition3 = "b1.C_CLASSID = 3 ";
            }

            using (SqlConnection connection = new SqlConnection(EditBeforRun.connectionString))
            {
                connection.Open();

                string query = $"SELECT b1.ID, b1.C_DESCRIPTION, b1.C_CREATIONDATE, b1.C_MODIFICATIONDATE, b1.C_CREATORID, b2.C_NAME AS CREATORNAME , " +
                               $"b1.C_LASTMODIFIERID, b3.C_NAME AS LASTMODIFIERNAME, b1.C_NAME , b1.C_CLASSID " +
                               $"FROM {tableName} b1 " +
                               $"Left JOIN {tableName} b2 ON b1.C_CREATORID = b2.ID " +
                               $"Left JOIN {tableName} b3 ON b1.C_LASTMODIFIERID = b3.ID " +
                               $"WHERE " + condition1 + condition2 + condition3;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clsBusinessObject businessObject = new clsBusinessObject();

                            businessObject.Id = new Guid(reader[0].ToString());

                            if (!String.IsNullOrEmpty(reader[1].ToString()))
                            {
                                businessObject.Description = (reader[1].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[2].ToString()))
                            {
                                businessObject.CreationDate = DateTime.Parse(reader[2].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[3].ToString()))
                            {
                                businessObject.ModificationDate = DateTime.Parse(reader[3].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[4].ToString()))
                            {
                                businessObject.CreatorId = new Guid(reader[4].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[5].ToString()))
                            {
                                businessObject.CreatorName = reader[5].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[6].ToString()))
                            {
                                businessObject.LastModifierId = new Guid(reader[6].ToString());
                            }

                            if (!String.IsNullOrEmpty(reader[7].ToString()))
                            {
                                businessObject.LastModifierName = reader[7].ToString();
                            }

                            if (!String.IsNullOrEmpty(reader[8].ToString()))
                            {
                                businessObject.Name = reader[8].ToString();
                            }

                            int c = (int)(reader[9]);
                            businessObject.Class = (clsBusinessObject.Type) c;

                            allBusinessObjects.Add(businessObject);
                        }
                    }
                }
            }

            return allBusinessObjects;
        }


    }
}
