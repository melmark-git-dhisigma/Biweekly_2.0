using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class ClsTemplateSession : ISerializable
{

    public ClsTemplateSession()
    {

    }
    protected ClsTemplateSession(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");





        mTemplateId = info.GetInt32("mTemplateId");
        mPrompted = info.GetBoolean("mPrompted");
        nTemplateId = info.GetInt32("nTemplateId");
        mStudentId = info.GetInt32("mStudentId");
        mIsSave = info.GetBoolean("mIsSave");
        mIsNew = info.GetBoolean("mIsNew");
        msetId = info.GetInt32("msetId");
        mstepId = info.GetInt32("mstepId");
        mLessonPlanId = info.GetInt32("mLessonPlanId");
        mStdtIEPId = info.GetInt32("mStdtIEPId");
        bMode = info.GetBoolean("bMode");
        iSessinHdr = info.GetInt32("iSessinHdr");


    }
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("mTemplateId", mTemplateId);
        info.AddValue("mPrompted", mPrompted);
        info.AddValue("nTemplateId", nTemplateId);
        info.AddValue("mStudentId", mStudentId);
        info.AddValue("mIsSave", mIsSave);
        info.AddValue("mIsNew", mIsNew);
        info.AddValue("msetId", msetId);
        info.AddValue("mstepId", mstepId);
        info.AddValue("mLessonPlanId", mLessonPlanId);
        info.AddValue("mStdtIEPId", mStdtIEPId);
        info.AddValue("bMode", bMode);
        info.AddValue("iSessinHdr", iSessinHdr);
    }
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        GetObjectData(info, context);
    }

    private int mTemplateId = 0;
    public bool mPrompted = true;

    public int TemplateId
    {
        get { return mTemplateId; }
        set { mTemplateId = value; }
    }

    private int nTemplateId = 0;
    public bool nPrompted = true;

    public int NewTemplateId
    {
        get { return nTemplateId; }
        set { nTemplateId = value; }
    }


    private int mStudentId = 0;

    public int StudentId
    {
        get { return mStudentId; }
        set { mStudentId = value; }
    }

    private bool mIsSave = false;

    public bool IsSave
    {
        get { return mIsSave; }
        set { mIsSave = value; }
    }
    private bool mIsNew = false;

    public bool IsNew
    {
        get { return mIsNew; }
        set { mIsNew = value; }
    }
    private int msetId = 0;

    public int setId
    {
        get { return msetId; }
        set { msetId = value; }
    }
    private int mstepId = 0;

    public int stepId
    {
        get { return mstepId; }
        set { mstepId = value; }
    }
    private int mLessonPlanId = 0;

    public int LessonPlanId
    {
        get { return mLessonPlanId; }
        set { mLessonPlanId = value; }
    }

    private int mStdtIEPId = 0;

    public int StdtIEPId
    {
        get { return mStdtIEPId; }
        set { mStdtIEPId = value; }
    }

    private bool bMode = false;

    public bool bOpenMode
    {
        get { return bMode; }
        set { bMode = value; }
    }

    private int iSessinHdr = 0;

    public int iStdtSessHdr
    {
        get { return iSessinHdr; }
        set { iSessinHdr = value; }
    }
    public bool Prompted
    {
        get { return mPrompted; }
        set { mPrompted = value; }
    }


}


