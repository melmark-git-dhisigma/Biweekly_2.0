using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsCalnderSess
/// </summary>
public class clsCalnderSess
{
	public clsCalnderSess()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private DataTable dtStudList;
    public DataTable dt_StudList
    {
        get { return dtStudList; }
        set { dtStudList = value; }
    }

    private SelectedDatesCollection days4weeks;
    public SelectedDatesCollection days_4weeks
    {
        get { return days4weeks; }
        set { days4weeks = value; }
    }
    private string crrntdate = "";
    public string CrrntDate
    {
        get { return crrntdate; }
        set { crrntdate = value; }
    }
}