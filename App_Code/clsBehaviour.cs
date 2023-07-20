using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsBehaviour
/// </summary>
public class clsBehaviour
{
	public clsBehaviour()
	{
    }
		private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
 
    private int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
	
}