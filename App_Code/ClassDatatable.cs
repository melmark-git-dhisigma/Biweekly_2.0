using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.XPath;

/// <summary>
/// Summary description for ClassDatatable
/// </summary>
public class ClassDatatable
{
	public ClassDatatable()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// Function to create DataColumn
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dtNew"></param>
    /// <returns></returns>
    public DataTable CreateColumn(string columnName, DataTable dtNew)
    {
        DataColumn dcNewColumn = new DataColumn(columnName, System.Type.GetType("System.String"));
        dtNew.Columns.Add(dcNewColumn);
        return dtNew;
    }
    /// <summary>
    /// Function to create the Datatable
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dtNew"></param>
    /// <param name="Assessmnts"></param>
    /// <returns></returns>
    public DataTable CreateAssessmntsTable(string[] columnName, DataTable dtNew, string[] Assessmnts)
    {
        DataRow dr = dtNew.NewRow();
        for (int i = 0; i < columnName.Length; i++)
        {
            dr[columnName[i]] = Assessmnts[i];
        }
        dtNew.Rows.Add(dr);

        return dtNew;
    }

    public string GetClassType(int Classid)
    {
        clsData objdata = new clsData();
        string ClassTypeQuery = "SELECT CASE WHEN ResidenceInd=1 THEN 'Residence' ELSE 'Day' END AS ClassType FROM [Class] WHERE ClassId='" + Classid + "'";
        object result= objdata.FetchValue(ClassTypeQuery);
        return result.ToString();
    }
    
}