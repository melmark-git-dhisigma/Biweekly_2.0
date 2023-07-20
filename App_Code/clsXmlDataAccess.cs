using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;
using System.Data;
using System.IO;
using System.Xml;
using System.Web.UI;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ClassAssess
/// </summary>
public class ClassAssess
{
    #region Declarations

    private string assessName;
    private string assessDesc;
    private string assessXML;
    private string note;
    private string[] goals;
    static int colID = 0;
    clsSession oSession = null;

    #endregion
    public ClassAssess()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Select all the section nodes from the Xmldocument and return
    /// </summary>
    /// <param name="i"></param>
    /// <param name="doc"></param>
    /// <param name="assess"></param>
    /// <returns></returns>
    public XPathNodeIterator ExecuteCode(int index, XmlDocument doc, string[] assess)
    {
        string file;

        file = "Assessmnts/" + assess[index];

        doc = new XmlDocument();
        doc.Load(file);
        XPathNavigator nav = doc.CreateNavigator();

        XPathNodeIterator iterator = nav.Select("/Sections/Section");
        return iterator;
    }

    /// <summary>
    /// Save the Assessment
    /// </summary>
    /// <param name="UsrId"></param>
    /// <returns></returns>
    public int Save(string sqlQry, int UsrId, int schoolID, SqlConnection con, SqlTransaction trans)
    {
        int nRetVal = 0;

        //DataClass oData = new DataClass();

        ClassAssess oAssess = new ClassAssess();
        byte[] blobData = null;

        blobData = oAssess.SaveAsBlob(AssessXML);



        System.Data.SqlClient.SqlCommand oCmd = new SqlCommand();

        oCmd.Connection = con;
        oCmd.Transaction = trans;
        oCmd.CommandType = System.Data.CommandType.Text;
        oCmd.CommandText = sqlQry;


        oCmd.Parameters.AddWithValue("@School", schoolID);
        oCmd.Parameters.AddWithValue("@Name", AssessName);
        oCmd.Parameters.AddWithValue("@Desc", AssessDesc);
        oCmd.Parameters.AddWithValue("@XML", blobData);

        oCmd.Parameters.AddWithValue("@User", UsrId);

        object retrnVal = oCmd.ExecuteScalar();


        nRetVal = Convert.ToInt32(retrnVal);

        //

        return nRetVal;
    }
    public int SaveGoal(string sqlQry, int schoolID, int UsrId, int assessID, int goalID, string desc, SqlConnection con, SqlTransaction trans)
    {
        int nRetVal = 0;

        DataClass oData = new DataClass();




        System.Data.SqlClient.SqlCommand oCmd = new SqlCommand();

        oCmd.Connection = oData.Connect();
        oCmd.CommandType = System.Data.CommandType.Text;
        oCmd.CommandText = sqlQry;

        oCmd.Parameters.AddWithValue("@school", schoolID);
        oCmd.Parameters.AddWithValue("@asmnt", assessID);
        oCmd.Parameters.AddWithValue("@goal", goalID);
        oCmd.Parameters.AddWithValue("@desc", desc);
        oCmd.Parameters.AddWithValue("@user", UsrId);

        nRetVal = oCmd.ExecuteNonQuery();

        return nRetVal;
    }
    /// <summary>
    /// Converts the assessment into byte array
    /// </summary>
    /// <param name="AssessXML"></param>
    /// <returns></returns>
    public byte[] SaveAsBlob(string AssessXML)
    {
        byte[] byteArray = null;

        using (FileStream fs = new FileStream
            (AssessXML, FileMode.Open, FileAccess.Read, FileShare.Read))
        {

            byteArray = new byte[fs.Length];

            int iBytesRead = fs.Read(byteArray, 0, (int)fs.Length);
        }
        return byteArray;
    }
    /// <summary>
    /// Load the xmldocument from the byte array
    /// </summary>
    /// <param name="XMLName"></param>
    /// <returns></returns>
    public XmlDocument LoadXmlfromBlob(string sqlQry)
    {
        XmlDocument oDoc = new XmlDocument();
        byte[] buffer = (byte[])SelectBlobData(sqlQry);
        if (buffer != null)
        {

            using (System.IO.MemoryStream oByteStream = new System.IO.MemoryStream(buffer)) //To Load the xml taken from the database into a XmlDocument object
            {
                using (System.Xml.XmlTextReader oRD = new System.Xml.XmlTextReader(oByteStream))
                {
                    oDoc.Load(oRD);
                }
            }
        }
        return oDoc;
    }
    /// <summary>
    /// Loads the byte array of xml from the database
    /// </summary>
    /// <param name="XMLid"></param>
    /// <returns></returns>
    public byte[] SelectBlobData(string sqlQry)
    {

        SqlCommand oCmd;

        DataClass oData = new DataClass();
        oCmd = new SqlCommand();

        oCmd.Connection = oData.Connect();
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = sqlQry;

        object blobData = oCmd.ExecuteScalar();
        oData.CloseConnection();

        return (byte[])blobData;
    }
    /// <summary>
    /// Fills the assessment into the Dataset
    /// </summary>
    /// <returns></returns>
    //public DataSet ShowAssessments()
    //{
    //    return Db.SqlQuery("SELECT AsmntTemplateID, AsmntTemplateName FROM AsmntTemplate ORDER BY AsmntTemplateName");
    //}

    #region Properties
    public string AssessName
    {
        get
        {
            return assessName;
        }
        set
        {
            assessName = value;
        }
    }


    public string AssessDesc
    {
        get
        {
            return assessDesc;
        }
        set
        {
            assessDesc = value;
        }
    }

    public string AssessXML
    {
        get
        {
            return assessXML;
        }
        set
        {
            assessXML = value;
        }
    }

    public string[] Goals
    {
        get
        {
            return goals;
        }
        set
        {
            goals = value;
        }
    }
    #endregion
}
/// <summary>
/// Summary description for ClassFormAssess
/// </summary>
public class ClassFormAssess
{
    ClassDatatable oDt = new ClassDatatable();
    static int Value = 0;
    public ClassFormAssess()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Loads the Xml/document from byte array
    /// </summary>
    /// <param name="StudXMLId"></param>
    /// <returns></returns>
    //public XmlDocument LoadXmlfromBlob(string sqlQry)
    //{
    //    XmlDocument oDoc = new XmlDocument();
    //    byte[] buffer = (byte[])SelectBlobData(sqlQry);
    //    if (buffer != null)
    //    {

    //        using (System.IO.MemoryStream oByteStream = new System.IO.MemoryStream(buffer)) //To Load the xml taken from the database into a XmlDocument object
    //        {
    //            using (System.Xml.XmlTextReader oRD = new System.Xml.XmlTextReader(oByteStream))
    //            {
    //                oDoc.Load(oRD);
    //            }
    //        }
    //    }
    //    return oDoc;
    //}
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="StudXMLId"></param>
    ///// <returns></returns>
    //public byte[] SelectBlobData(string sqlQry)
    //{
    //    object blobData = Db.SqlAction(sqlQry);
    //    return (byte[])blobData;
    //}
    /// <summary>
    /// Converts the XmlNodelist into Datatable based on the node contains child or not
    /// </summary>
    /// <param name="xnl"></param>
    /// <param name="containsQues"></param>
    /// <returns></returns>
    public DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl, string containsQues)
    {
        DataTable dt = new DataTable();
        int TempColumn = 0;

        foreach (XmlNode node in xnl)
        {
            if (node.Attributes["ContainsQuestions"] != null)
            {
                if (node.Attributes["ContainsQuestions"].Value == containsQues)
                {
                    if (TempColumn == 0)
                    {
                        for (int i = 0; i < node.Attributes.Count; i++)
                        {
                            dt = oDt.CreateColumn(node.Attributes[i].Name, dt);
                        }
                        TempColumn = 1;
                    }
                    string[] columnNames = new string[node.Attributes.Count];
                    string[] Values = new string[node.Attributes.Count];
                    for (int j = 0; j < node.Attributes.Count; j++)
                    {
                        columnNames[j] = node.Attributes[j].Name;
                        Values[j] = node.Attributes[j].Value;
                    }
                    dt = oDt.CreateAssessmntsTable(columnNames, dt, Values);
                }
            }

        }
        return dt;
    }


    /// <summary>
    /// Converts the Xmlnodelist into datatable
    /// </summary>
    /// <param name="xnl"></param>
    /// <returns></returns>
    public DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
    {
        DataTable dt2 = new DataTable();
        int TempColumn2 = 0;

        foreach (XmlNode node in xnl)
        {

            if (TempColumn2 == 0)
            {
                dt2 = oDt.CreateColumn("ID", dt2);
                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    dt2 = oDt.CreateColumn(node.Attributes[i].Name, dt2);
                }
                TempColumn2 = 1;
            }

            string[] columnNames = new string[node.Attributes.Count + 1];
            string[] Values = new string[node.Attributes.Count + 1];
            columnNames[0] = "ID";
            Values[0] = Value.ToString();
            Value++;
            for (int j = 0; j < node.Attributes.Count; j++)
            {
                columnNames[j + 1] = node.Attributes[j].Name;
                Values[j + 1] = node.Attributes[j].Value;
            }
            dt2 = oDt.CreateAssessmntsTable(columnNames, dt2, Values);
        }
        return dt2;
    }
}
/// <summary>
/// Summary description for ClassGoal
/// </summary>
public class ClassGoal
{
    public static int Value = 0;
    public ClassGoal()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    public string GoalName(string goal)
    {
        string goalName = "";
        if (goal == "Basic Learning")
            goalName = "BasicLearnerInd";
        if (goal == "Self Help")
            goalName = "SelfHelpInd";
        if (goal == "Academic")
            goalName = "AcademicScr";
        if (goal == "Language Skills")
            goalName = "LanguageInd";
        if (goal == "Daily Living")
            goalName = "DailyLivingScr";
        if (goal == "Communication")
            goalName = "CommunicationInd";
        if (goal == "Employment")
            goalName = "EmploymentScr";
        if (goal == "Social")
            goalName = "SocialInd";
        if (goal == "Motor")
            goalName = "MotorInd";
        if (goal == "Academic Skills")
            goalName = "AcademicInd";
        if (goal == "")
            goalName = "";
        if (goal == "")
            goalName = "";
        if (goal == "")
            goalName = "";
        return goalName;
    }

    //public XmlDocument LoadXmlfromBlob(string sqlQry)
    //{
    //    XmlDocument oDoc = new XmlDocument();
    //    byte[] buffer = (byte[])SelectBlobData(sqlQry);
    //    if (buffer != null)
    //    {

    //        using (System.IO.MemoryStream oByteStream = new System.IO.MemoryStream(buffer)) //To Load the xml taken from the database into a XmlDocument object
    //        {
    //            using (System.Xml.XmlTextReader oRD = new System.Xml.XmlTextReader(oByteStream))
    //            {
    //                oDoc.Load(oRD);
    //            }
    //        }
    //    }
    //    return oDoc;
    //}

    //public byte[] SelectBlobData(string sqlQry)
    //{
    //    object blobData = Db.SqlAction(string.Format("SELECT StdtAsmntXML FROM StdtAsmnt WHERE StdtAsmntId={0}"));
    //    return (byte[])blobData;
    //}

    public DataSet SelectAssessmnts(string goal)
    {
        string columnName = GoalName(goal);
        DataSet dsAssess = new DataSet();
        DataClass oData = new DataClass();

        dsAssess = oData.ExecuteDataSet("SELECT AsmntTemplateId, AsmntTemplateName, AsmntXML FROM AsmntTemplate WHERE " + columnName + "='A' ORDER BY AsmntTemplateName");
        return dsAssess;
    }
    //public DataSet SelectAssessmnts(int AssessID)
    //{
    //    return Db.SqlQuery(String.Format("SELECT StdtAsmntXML, AsmntTemplateName, StdtAsmntXML FROM StdtAsmnt WHERE StdtAsmntId={0}", AssessID));
    //}

    public DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl, string skill)
    {
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtAssess = new DataTable();
        int TempColumn = 0;

        foreach (XmlNode node in xnl)
        {

            if (TempColumn == 0)
            {
                for (int index = 0; index < node.Attributes.Count; index++)
                {
                    dtAssess = oDt.CreateColumn(node.Attributes[index].Name, dtAssess);
                }
                TempColumn = 1;
            }
            if (node.Attributes["Skill"].Value == skill)
            {
                string[] columnNames = new string[node.Attributes.Count];
                string[] Values = new string[node.Attributes.Count];

                for (int index = 0; index < node.Attributes.Count; index++)
                {
                    columnNames[index] = node.Attributes[index].Name;
                    Values[index] = node.Attributes[index].Value;
                }
                dtAssess = oDt.CreateAssessmntsTable(columnNames, dtAssess, Values);
            }
        }
        return dtAssess;
    }

    public DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
    {
        try
        {
            ClassDatatable oDt = new ClassDatatable();
            DataTable dtAssess = new DataTable();
            int TempColumn = 0;

            foreach (XmlNode node in xnl)
            {

                if (TempColumn == 0)
                {
                    dtAssess = oDt.CreateColumn("ID", dtAssess);
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        if (node.Attributes[i].Name != "Box")
                            dtAssess = oDt.CreateColumn(node.Attributes[i].Name, dtAssess);
                    }
                    dtAssess = oDt.CreateColumn("Box", dtAssess);
                    TempColumn = 1;
                }
                string[] columnNames = new string[node.Attributes.Count + 1];
                string[] Values = new string[node.Attributes.Count + 1];
                columnNames[0] = "ID";
                Values[0] = Value.ToString();
                Value++;
                if (node.Attributes["ContainsQuestions"] != null)
                {
                    if (node.Attributes["ContainsQuestions"].Value == "False")
                    {
                        for (int j = 0; j < node.Attributes.Count; j++)
                        {
                            columnNames[j + 1] = node.Attributes[j].Name;
                            Values[j + 1] = node.Attributes[j].Value;
                        }
                        dtAssess = oDt.CreateAssessmntsTable(columnNames, dtAssess, Values);
                    }
                    else
                    {

                        for (int j = 0; j < node.Attributes.Count; j++)
                        {
                            columnNames[j + 1] = node.Attributes[j].Name;
                            Values[j + 1] = node.Attributes[j].Value;
                        }

                        dtAssess = oDt.CreateAssessmntsTable(columnNames, dtAssess, Values);
                        if (node.ChildNodes.Count>0)
                        {
                       
                            XmlNodeList xQuest = node.ChildNodes.Item(0).ChildNodes;
                            for (int quesCount = 0; quesCount < xQuest.Count; quesCount++)
                            {
                                dtAssess = oDt.CreateAssessmntsTable(new string[] { "name", "Code", "ID", "Box" }, dtAssess, new string[] { ">> " + xQuest.Item(quesCount).Attributes["name"].Value, xQuest.Item(quesCount).Attributes["Code"].Value, Value.ToString(), xQuest.Item(quesCount).Attributes["Box"].Value });
                                Value++;
                            }

                        }
                    }
                }
            }
            return dtAssess;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        
    }
}
/// <summary>
/// Summary description for ClassStudntAssess
/// </summary>
public class ClassStudntAssess
{
    #region Declarations

    private int studentID;
    private int assessyearID;
    private string note;
    private string studAssessName;
    private string assessTempName;
    private string assessType;
    private byte[] blobData;
    clsSession oSession = null;

    #endregion
    public ClassStudntAssess()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    public int Save(string sqlQry, int AssessID, int loginID, int schoolID, int status)
    {
        int nRetVal = 0;
        //nRetVal = Db.SaveStudAssess(AssessID, this.studentID, this.assessyearID, this.studAssessName, this.assessTempName, this.blobData);

        //nRetVal = Load(nRetVal);
        DataClass oData = new DataClass();
        YearID = oData.ExecuteScalar("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");

        System.Data.SqlClient.SqlCommand oCmd = new SqlCommand();
        if (AssessID == 0)
        {

            oCmd.Connection = oData.Connect();
            oCmd.CommandType = System.Data.CommandType.Text;
            oCmd.CommandText = sqlQry;

            oCmd.Parameters.AddWithValue("@StId", StudID);
            oCmd.Parameters.AddWithValue("@YearId", YearID);
            oCmd.Parameters.AddWithValue("@AsmntName", assessTempName);
            oCmd.Parameters.AddWithValue("@AssessTempName", StudAssessName);
            oCmd.Parameters.AddWithValue("@Type", AssessType);
            oCmd.Parameters.AddWithValue("@XML", blobData);
            oCmd.Parameters.AddWithValue("@User", loginID);
            oCmd.Parameters.AddWithValue("@School", schoolID);
            oCmd.Parameters.AddWithValue("@AssgnUserId", 1);
            oCmd.Parameters.AddWithValue("@AssmntId", AsmntID);
            //oCmd.Parameters.AddWithValue("@AssmntGrpId", 1);
            oCmd.Parameters.AddWithValue("@AssmntStatusId", status);
            //oCmd.Parameters.AddWithValue("@IncScr", 600);
            oCmd.Parameters.AddWithValue("@ModUsr", loginID);

            nRetVal = Convert.ToInt32(oCmd.ExecuteScalar());

        }
        else
        {
            oCmd.Parameters.Clear();
            oCmd.Connection = oData.Connect();
            oCmd.CommandType = System.Data.CommandType.Text;
            oCmd.CommandText = sqlQry;

            oCmd.Parameters.AddWithValue("@XML", blobData);
            oCmd.Parameters.AddWithValue("@AssessID", AssessID);
            oCmd.Parameters.AddWithValue("@AsmntName", StudAssessName);
            oCmd.Parameters.AddWithValue("@Status", status);
            //oCmd.Parameters.AddWithValue("@ModDate", DateTime.Now);
            oCmd.Parameters.AddWithValue("@ModId", loginID);
            oCmd.Parameters.AddWithValue("@Note", Note);
            oCmd.ExecuteScalar();

        }
        oData.CloseConnection();
        //
        return nRetVal;
    }

    #region Properties
    public int StudID
    {
        get
        {
            return studentID;
        }
        set
        {
            studentID = value;
        }
    }
    public string Note
    {
        get
        {
            return note;
        }
        set
        {
            note = value;
        }
    }

    public int YearID
    {
        get
        {
            return assessyearID;
        }
        set
        {
            assessyearID = value;
        }
    }
    private int assessID = 0;
    public int AsmntID
    {
        get
        {
            return assessID;
        }
        set
        {
            assessID = value;
        }
    }
    public string StudAssessName
    {
        get
        {
            return studAssessName;
        }
        set
        {
            studAssessName = value;
        }
    }

    public string AssessTempName
    {
        get
        {
            return assessTempName;
        }
        set
        {
            assessTempName = value;
        }
    }

    public string AssessType
    {
        get
        {
            return assessType;
        }
        set
        {
            assessType = value;
        }
    }

    public byte[] BlobData
    {
        get
        {
            return blobData;
        }
        set
        {
            blobData = value;
        }
    }
    #endregion
}

