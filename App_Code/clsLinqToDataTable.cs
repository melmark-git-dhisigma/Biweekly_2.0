using System;

using System.Data;

using System.Configuration;

using System.Linq;

using System.Web;

using System.Web.Security;

using System.Web.UI;

using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;

using System.Xml.Linq;

using System.Reflection;


static public class  clsLinqToDataTable
{
	public static DataTable ConvertToDataTable<T>(this System.Collections.Generic.IEnumerable<T> varList, CreateRowDelegate<T> fn)

        {

            DataTable dataTable = new DataTable();



            // Variable for column names.

            PropertyInfo[] tableColumns = null;



            // To check whether more than one elements there in varList.

            foreach (T rec in varList)

            {

                // Use reflection to get column names, to create table.

                if (tableColumns == null)

                {

                    tableColumns = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in tableColumns)

                    {

                        Type columnType = pi.PropertyType;

                        if ((columnType.IsGenericType) && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)))

                        {

                            columnType = columnType.GetGenericArguments()[0];

                        }

                        dataTable.Columns.Add(new DataColumn(pi.Name, columnType));

                    }

                }



                // Copying the IEnumerable value to DataRow and then added into DataTable.

                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo pi in tableColumns)

                {

                    dataRow[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);

                }

                dataTable.Rows.Add(dataRow);

            }

            return (dataTable);

        }



        public delegate object[] CreateRowDelegate<T>(T t);

    }

