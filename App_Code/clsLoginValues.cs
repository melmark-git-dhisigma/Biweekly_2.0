using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class clsLoginValues : ISerializable
{
    public clsLoginValues()
    {

    }
    protected clsLoginValues(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        mDomainName = info.GetString("mDomainName");
        mIsActiveLogin = info.GetString("mIsActiveLogin");
        ar = (ArrayList)info.GetValue("ar", typeof(object));
    }
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("mDomainName", mDomainName);
        info.AddValue("ar", ar);
        info.AddValue("mIsActiveLogin", mIsActiveLogin);
    }
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        GetObjectData(info, context);
    }



    private ArrayList ar = null;

    private string mDomainName = "";
    private string mUsername = "";
    private string mPassword = "";
    private string mIsActiveLogin = "";



    public string IsActiveLogin
    {
        get { return mIsActiveLogin; }
        set { mIsActiveLogin = value; }
    }

    public string DomainName
    {
        get { return mDomainName; }
        set { mDomainName = value; }
    }

    public ArrayList LoginVals
    {
        get { return ar; }
        set { ar = value; }
    }
}