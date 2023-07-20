using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsIEP
/// </summary>
public class clsIEP
{
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlParameter par = null;
    int returnVal = 0;
    string strSql = "";
    DataTable Dt = null;
    DataTable DtCol = null;
    clsData objData = null;
    public clsIEP()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string GETIEPStatus(int IEPId,int StId,int ScId)
    {
        
        string value = "";
        if (ScId == 1)
        {
            value = "NE";
        }
        else if (ScId == 2)
        {
            value = "PA";
        }

        object status = "";
        objData = new clsData();
        if (value == "PA")
        {
            object pendstatus = objData.FetchValue("Select StatusId from StdtIEP_PE where StudentId =" + StId + "  And SchoolId=" + ScId + "  And StdtIEP_PEId=" + IEPId + " ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                status = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
        }
        else if (value == "NE")
        {
            object pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StudentId =" + StId + "  And SchoolId=" + ScId + "  And StdtIEPId=" + IEPId + " ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                status = objData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
        }

        return status.ToString();
    }
}