using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsTempFrame
/// </summary>
public class clsTempFrame
{
    public clsTempFrame()
    {

    }
    private int mSessHdrId = 0;
    private int mTempId = 0;
    private int mNextSetId = 0;
    private string mNextSetName = "";
    private string mColTypeCode = "";
    private string mCustom = "";
    private int mSchoolid = 0;
    private int mLogin = 0;
    private int mStudentid = 0;
    private bool mBStatusFlag = false;
    private int mIOASessHdrId = 0;
    private int mISessHeaderId = 0;
    private bool mBSetMoveUp = true;
    private bool mBSetMoveBack = true;
    private int mLessonPlanId = 0;
    private int[] mcolumnids = new int[10];
    public int LessonPlanId
    {
        get
        {
            return mLessonPlanId;
        }
        set
        {
            mLessonPlanId = value;
        }
    }


    public int[] Columnids
    {
        get
        {
            return mcolumnids;
        }
        set
        {
            mcolumnids = value;
        }
    }

    public int SessHdrId
    {
        get
        {
            return mSessHdrId;
        }
        set
        {
            mSessHdrId = value;
        }
    }
    public int TempId
    {
        get
        {
            return mTempId;
        }
        set
        {
            mTempId = value;
        }
    }
    public int NextSetId
    {
        get
        {
            return mNextSetId;
        }
        set
        {
            mNextSetId = value;
        }
    }
    public string NextSetName
    {
        get
        {
            return mNextSetName;
        }
        set
        {
            mNextSetName = value;
        }
    }
    public string ColTypeCode
    {
        get
        {
            return mColTypeCode;
        }
        set
        {
            mColTypeCode = value;
        }
    }
    public string Custom
    {
        get
        {
            return mCustom;
        }
        set
        {
            mCustom = value;
        }
    }
    public int Schoolid
    {
        get
        {
            return mSchoolid;
        }
        set
        {
            mSchoolid = value;
        }
    }
    public int Login
    {
        get
        {
            return mLogin;
        }
        set
        {
            mLogin = value;
        }
    }
    public int Studentid
    {
        get
        {
            return mStudentid;
        }
        set
        {
            mStudentid = value;
        }
    }
    public bool BStatusFlag
    {
        get
        {
            return mBStatusFlag;
        }
        set
        {
            mBStatusFlag = value;
        }
    }
    public int IOASessHdrId
    {
        get
        {
            return mIOASessHdrId;
        }
        set
        {
            mIOASessHdrId = value;
        }
    }
    public int ISessHeaderId
    {
        get
        {
            return mISessHeaderId;
        }
        set
        {
            mISessHeaderId = value;
        }
    }
    public bool BSetMoveUp
    {
        get
        {
            return mBSetMoveUp;
        }
        set
        {
            mBSetMoveUp = value;
        }
    }
    public bool BSetMoveBack
    {
        get
        {
            return mBSetMoveBack;
        }
        set
        {
            mBSetMoveBack = value;
        }
    }
}