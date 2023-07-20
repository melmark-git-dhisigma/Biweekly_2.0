using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JsonModel
/// </summary>
public class JsonModel
{
	public JsonModel()
	{
		//
		// TODO: Add constructor logic here
		//
        values = new List<Valuescl>();
	}

    public string key { get; set; }
    public string  color { get; set; }
    public List<Valuescl> values { get; set; }
}
public class Valuescl
{
    public string  label { get; set; }
    public double value { get; set; }
}

