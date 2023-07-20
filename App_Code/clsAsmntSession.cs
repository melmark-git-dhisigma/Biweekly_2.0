using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

[Serializable]
public class clsAsmntSession: ISerializable
{
	public clsAsmntSession()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    protected clsAsmntSession(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        dtLP = (DataTable)info.GetValue("dtLP", typeof(object));
        dtTemp = (DataTable)info.GetValue("dtTemp", typeof(object));
        dtSkill = (DataTable)info.GetValue("dtSkill", typeof(object));
        dtInactive = (DataTable)info.GetValue("dtInactive", typeof(object));

    }
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("dtLP", dtLP);
        info.AddValue("dtTemp", dtTemp);
        info.AddValue("dtSkill", dtSkill);
        info.AddValue("dtInactive", dtInactive);
        
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        GetObjectData(info, context);
    }



   
    private DataTable dtLP;
    public DataTable dt_LP
    {
        get { return dtLP; }
        set { dtLP = value; }
    }
    private DataTable dtTemp;
    public DataTable dt_Temp
    {
        get { return dtTemp; }
        set { dtTemp = value; }
    }
    private DataTable dtSkill;
    public DataTable dt_Skill
    {
        get { return dtSkill; }
        set { dtSkill = value; }
    }
    private DataTable dtInactive;
    public DataTable dt_Inactive
    {
        get { return dtInactive; }
        set { dtInactive = value; }
    }
}