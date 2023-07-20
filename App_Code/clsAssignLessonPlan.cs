using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
public class clsAssignLessonPlan
{
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlParameter par = null;
    int returnVal = 0;
    string strSql = "";
    DataTable Dt = null;
    DataTable DtCol = null;
    clsData objData = null;

    public clsAssignLessonPlan()
    {

    }

    public int SaveTemplateDetails1(int SchoolId, int StudentId, int TemplateId, string LessonName, int CreatedBy)
    {
        objData = new clsData();
        string db1ConnectionString = objData.ConnectionString;

        const String sqlSELECT = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,DSTemplateName,CreatedBy,CreatedOn)";
        //const String  sqlINSERT ="SELECT SchoolId,@StudentId ,LessonPlanId,DSTemplateName,@CreatedBy,getdate() FROM DSTempHdr WHERE DSTempHdrId = @TemplateId;";
        String sqlINSERT = "SELECT SchoolId,StudentId ,LessonPlanId,DSTemplateName,CreatedBy,getdate() FROM DSTempHdr WHERE DSTempHdrId = " + TemplateId + ";";
        //const String sqlSELECT = "SELECT COL1,COl2,Col3 FROM TableA WHERE COL1=@COL1;";
        //const String sqlINSERT = "INSERT INTO TableA (COl2,Col3)VALUES (@Col2,@Col3)";
        //                       + "SELECT CAST(scope_identity() AS int)";

        SqlConnection Con1 = objData.Open();
        SqlConnection Con2 = objData.Open();
        //con1.Open();
        //   con2.Open();
        using (var SELECTCommand = new SqlCommand(sqlINSERT, Con1))
        {
            //SELECTCommand.Parameters.AddWithValue("@TemplateId", TemplateId);
            //SELECTCommand.Parameters.AddWithValue("@StudentId", StudentId);
            //SELECTCommand.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            using (SqlDataReader reader = SELECTCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    int newID;
                    using (var INSERTCommand = new SqlCommand(sqlSELECT, Con2))
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            INSERTCommand.Parameters.AddWithValue("@" + reader.GetName(i), reader[i]);
                        }
                        newID = (int)INSERTCommand.ExecuteScalar();
                    }
                }
            }
        }


        //objData = new clsData();
        //strSql = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,DSTemplateName,CreatedBy,CreatedOn)";
        //strSql += " SELECT 	SchoolId," + StudentId + ",LessonPlanId,DSTemplateName," + CreatedBy + ",getdate() FROM DSTempHdr WHERE DSTempHdrId = " + TemplateId + " ";


        //string SELECTQuery = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,DSTemplateName,CreatedBy,CreatedOn)";
        //string INSERTQuery = string.Format(" SELECT 	SchoolId," + StudentId + ",LessonPlanId,DSTemplateName," + CreatedBy + ",getdate() FROM DSTempHdr WHERE DSTempHdrId = " + TemplateId + " ", SELECTQuery);



        //objData.Execute(INSERTQuery);
        return 0;
    }

    public int SaveTemplateDetails11(int SchoolId, int StudentId, int LessonId, string LessonName, int CreatedBy)
    {
        try
        {
            objData = new clsData();
            if (objData != null)
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pSchoolId, pStudentId, pLessonId, pLessonName, pCreatedBy;

                pSchoolId = new SqlParameter("@SchoolId", SqlDbType.Int);
                cmd.Parameters.Add(pSchoolId);
                pSchoolId.Value = SchoolId;

                pStudentId = new SqlParameter("@StudentId", SqlDbType.Int);
                cmd.Parameters.Add(pStudentId);
                pStudentId.Value = StudentId;

                pLessonId = new SqlParameter("@LessonPlanId", SqlDbType.Int);
                cmd.Parameters.Add(pLessonId);
                pLessonId.Value = LessonId;

                pLessonName = new SqlParameter("@LessonName", SqlDbType.VarChar, 250);
                cmd.Parameters.Add(pLessonName);
                pLessonName.Value = LessonName;

                pCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.Int);
                cmd.Parameters.Add(pCreatedBy);
                pCreatedBy.Value = CreatedBy;


                con = objData.Open();
                cmd.Connection = con;
                cmd.CommandText = "TemplateStudentLevel_Create";
                returnVal = cmd.ExecuteNonQuery();
            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            return 0;

        }
        finally
        {
            con.Close();
        }
        return returnVal;
    }

    public int CopyTemplate(int templateid, int loginid, int visualLessonId)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = null;
        objData = new clsData();
        int oldSetId = 0;
        int parentSetId = 0;
        string strQuery = "";
        try
        {
            Con = objData.Open();
            Trans = Con.BeginTransaction();
            strQuery = "INSERT INTO DSTempHdr SELECT [SchoolId],[StudentId],[LessonPlanId],[TeachingProcId],[DSTemplateName],[DSTemplateDesc],[VerBeginDate],[VerEndDate],[CurrVerInd]," +
                       "[MultiSetsInd],[MultiStepInd],[SkillType],[NbrOfTrials],[ChainType],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd]," +
                       "(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),[IsVisualTool]," + visualLessonId + ",[Baseline],[Objective],[BaselineProc],[BaselineStart],[BaselineEnd],[CorrRespDef],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef]," +
                       "[CorrectionProc],[ReinforcementProc],[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst]," + loginid + ",GETDATE()," + loginid + ",GETDATE() FROM DSTempHdr WHERE DSTempHdrId='" + templateid + "'";

            int TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));



            DataTable dtpromt = new DataTable();
            dtpromt = objData.ReturnDataTable("SELECT DSTempPromptId FROM DSTempPrompt WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtpromt.Rows.Count > 0)
            {
                foreach (DataRow row in dtpromt.Rows)
                {
                    strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) ";
                    strQuery += "SELECT " + TId + ",PromptId,PromptOrder,ActiveInd," + loginid + ",CreatedOn FROM DSTempPrompt WHERE DSTempPromptId=" + Convert.ToInt32(row["DSTempPromptId"]) + "";
                    int PromptId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                }
            }

            DataTable dtset = new DataTable();
            dtset = objData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtset.Rows.Count > 0)
            {
                foreach (DataRow row in dtset.Rows)
                {
                    strQuery = "INSERT INTO DSTempSet(SchoolId,DSTempHdrId,PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn,DistractorSamples,DistractorSamplesCount) ";
                    strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + loginid + ",getdate(),DistractorSamples,DistractorSamplesCount FROM DSTempSet WHERE DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
                    int SetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                }
            }
            DataTable dtstep = new DataTable();
            dtstep = objData.ReturnDataTable("SELECT DSTempStepId FROM DSTempStep WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtstep.Rows.Count > 0)
            {
                foreach (DataRow row in dtstep.Rows)
                {
                    strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                    strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()	FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " ";
                    int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                }
            }


            DataTable dtsetcol = new DataTable();
            dtsetcol = objData.ReturnDataTable("SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtsetcol.Rows.Count > 0)
            {
                foreach (DataRow row in dtsetcol.Rows)
                {
                    strQuery = "INSERT INTO DSTempSetCol(SchoolId, DSTempHdrId,ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp	,InCorrStdResp,IncMisTrialInd,MisTrialDesc,ActiveInd,CreatedBy,CreatedOn) ";
                    strQuery += "SELECT SchoolId, " + TId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,ActiveInd," + loginid + ",CreatedOn FROM DSTempSetCol WHERE DSTempSetColId = " + Convert.ToInt32(row["DSTempSetColId"]) + " ";
                    int setColNewId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    DataTable dtsetcolcalc = new DataTable();
                    dtsetcolcalc = objData.ReturnDataTable("SELECT DSTempSetColCalcId FROM DSTempSetColCalc WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + "", Con, Trans, false);
                    if (dtsetcolcalc.Rows.Count > 0)
                    {
                        foreach (DataRow rowc in dtsetcolcalc.Rows)
                        {
                            strQuery = "INSERT INTO DSTempSetColCalc(SchoolId,DSTempSetColId,CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn) " +
                                        "SELECT SchoolId," + setColNewId + ",CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd," + loginid + ",getdate() FROM DSTempSetColCalc WHERE DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + "";
                            int setColCalId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) "; //--- [New Criteria] May 2020 ---//
                            strQuery += "SELECT  " + TId + ",SchoolId," + setColNewId + "," + setColCalId + ",RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ISNULL(ConsequetiveAvgInd,0) AS ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + " And DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + " "; //--- [New Criteria] May 2020 ---//
                            int lastId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        }
                    }

                }
            }
            objData.CommitTransation(Trans, con);
            return TId;
        }
        catch (Exception Ex)
        {
            objData.RollBackTransation(Trans, con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            return 0;
        }
    }

    public void DSTempSetStatusInsert(int templateid, int loginid)
    {
        try
        {
            objData = new clsData();
            DataTable dtTempType = objData.ReturnDataTable("SELECT SkillType,ChainType,(SELECT [LookupName] FROM [dbo].[LookUp] WHERE [LookupId]=PromptTypeId) PrmptName FROM DSTempHdr WHERE DSTempHdrId='" + templateid + "'", false);
            string PromptType = Convert.ToString(dtTempType.Rows[0]["PrmptName"]);
            DataTable DTSet = objData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId='" + templateid + "' AND ActiveInd='A'", false);
            DataTable DTStep = objData.ReturnDataTable("SELECT DSTempStepId FROM DSTempStep WHERE DSTempHdrId='" + templateid + "' AND ActiveInd='A' ", false);
            if (dtTempType.Rows[0]["SkillType"] == "Discrete")
            {                
                foreach (DataRow row in DTSet.Rows)
                {
                    string InsertQuery="";
                    if (PromptType == "")
                    {
                        InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + "," + loginid + ",GETDATE())";
                    }
                    else
                    {
                        if (PromptType == "Most-to-Least")
                        {
                            InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=(SELECT MAX(PromptOrder) FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + ") )," + loginid + ",GETDATE())";
                        }
                        else 
                        {
                            InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=0)," + loginid + ",GETDATE())";
                        }
                        
                    }
                    objData.Execute(InsertQuery);
                }

            }
            else if (dtTempType.Rows[0]["SkillType"] == "Chained")
            {
                foreach (DataRow row in DTSet.Rows)
                {
                    string InsertQuery = "";
                    if (dtTempType.Rows[0]["ChainType"] == "Backward chain")
                    {

                        if (PromptType == "")
                        {
                            InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + "," + loginid + ",GETDATE())";
                        }
                        else
                        {
                            if (PromptType == "Most-to-Least")
                            {
                                InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=(SELECT MAX(PromptOrder) FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + ") )," + loginid + ",GETDATE())";
                            }
                            else
                            {
                                InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=0)," + loginid + ",GETDATE())";
                            }

                        }
                    }
                    else
                    {
                        if (PromptType == "")
                        {
                            InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + "," + loginid + ",GETDATE())";
                        }
                        else
                        {
                            if (PromptType == "Most-to-Least")
                            {
                                InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=(SELECT MAX(PromptOrder) FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + ") )," + loginid + ",GETDATE())";
                            }
                            else
                            {
                                InsertQuery = "INSERT INTO DSTempSetStatus(DSTempHdrId,CurrentSetId,CurrentStepId,CurrentPromptId,CreatedBy,CreatedOn) VALUES (" + templateid + "," + row["DSTempSetId"] + ",(SELECT PromptId FROM [dbo].[DSTempPrompt] WHERE DSTempHdrId=" + templateid + " AND PromptOrder=0)," + loginid + ",GETDATE())";
                            }

                        }
                    }
                    objData.Execute(InsertQuery);
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

    public int CopyCustomtemplate(int templateid, int loginid, int visualLessonId,int SLpId=0)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = null;
        objData = new clsData();
        string strQuery = "";
        int oldSetId = 0;
        int parentSetId = 0;
        try
        {
            Con = objData.Open();
            Trans = Con.BeginTransaction();
            strQuery = "SELECT LessonPlanId,StudentId from DSTempHdr WHERE DSTempHdrId=" + templateid;
            DataTable dt = new DataTable();
            dt = objData.ReturnDataTable(strQuery, Con, Trans, false);
            strQuery = "SELECT MAX(VerNbr) from DSTempHdr WHERE LessonPlanId=" + Convert.ToInt32(dt.Rows[0]["LessonPlanId"]) + " AND StudentId=" + Convert.ToInt32(dt.Rows[0]["StudentId"]) + " AND [StatusId]<>(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Deleted')";
            string version = objData.FetchValueTrans(strQuery, Trans, Con).ToString();
            version = checkversion(version);
            strQuery = "select StdtLessonPlanid from DSTempHdr where DSTempHdrId=" + templateid;
            int stdtLpId = Convert.ToInt32(objData.FetchValueTrans(strQuery, Trans, Con));
            if (SLpId != 0)
            {
                stdtLpId = SLpId;
            }
            //strQuery = "Update DSTempHdr set [StatusId]=(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Expired')  WHERE DSTempHdrId= " + templateid;
            //int expiredId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

            strQuery = "INSERT INTO DSTempHdr ([SchoolId],[StudentId],[LessonPlanId],[StdtLessonplanId],[TeachingProcId],[DSTemplateName],[NoofTimesTried],[NoofTimesTriedPer]," +
                       "[DSTemplateDesc],[VerBeginDate],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd],[SkillType],[MatchToSampleType],[NbrOfTrials]," +
                       "[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd],[StatusId],[IsVisualTool]," +
                       "[VTLessonId],[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd],[CorrRespDef]," +
                       "[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
                       "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
                       "[TeacherPrepare],[StudentPrepare],[StudResponse],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]," +
                       "[FrameandStrand],[LessonPlanGoal],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[LessonOrder],[deletessn],[LessonSDate],[LessonEDate]) SELECT [SchoolId],[StudentId],[LessonPlanId]," + stdtLpId + "," +
                       "[TeachingProcId],[DSTemplateName],[NoofTimesTried],[NoofTimesTriedPer],[DSTemplateDesc],[VerBeginDate],[VerEndDate],[CurrVerInd],[MultiSetsInd],[MultiStepInd]," +
                       "[SkillType],[MatchToSampleType],[NbrOfTrials],[ChainType],[TotalTaskFormat],[TotalTaskType],[TaskOther],[MatchToSampleRecOrExp],[PromptTypeId],[TotNbrOfSessions],[SessionFreq],[NbrOfSession],[CompCurrInd]," +
                       "(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),[IsVisualTool]," +
                       "'" + visualLessonId + "',[Baseline],[Objective],[GeneralProcedure],[BaselineProc],[BaselineStart],[BaselineEnd]," +
                       "[CorrRespDef],[CorrectResponse],[StudCorrRespDef],[IncorrRespDef],[StudIncorrRespDef],[CorrectionProc],[ReinforcementProc]," +
                       "[TeacherRespReadness],[StudentReadCrita],[MajorSetting],[MinorSetting],[LessonDefInst],[Mistrial],[MistrialResponse]," +
                       "[TeacherPrepare],[StudentPrepare],[StudResponse]," + loginid + ",GETDATE()," + loginid + ",GETDATE(),[FrameandStrand],[LessonPlanGoal]" +
                       ",[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[ApprNoteLessonInfo],[ApprNoteTypeInstruction],[ApprNoteMeasurement],[ApprNoteSet],[ApprNoteStep],[ApprNotePrompt],[ApprNoteLessonProc],[LessonOrder],[deletessn],[LessonSDate],[LessonEDate] FROM DSTempHdr WHERE DSTempHdrId='" + templateid + "'";

            int TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
            strQuery = "UPDATE DSTempHdr SET VerNbr='" + version + "' WHERE DSTempHdrId=" + TId;
            objData.ExecuteWithTrans(strQuery, Con, Trans);

            DataTable dtpromt = new DataTable();
            dtpromt = objData.ReturnDataTable("SELECT DSTempPromptId FROM DSTempPrompt WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtpromt != null)
            {
                if (dtpromt.Rows.Count > 0)
                {
                    foreach (DataRow row in dtpromt.Rows)
                    {
                        strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT " + TId + ",PromptId,PromptOrder,ActiveInd," + loginid + ",CreatedOn FROM DSTempPrompt WHERE DSTempPromptId=" + Convert.ToInt32(row["DSTempPromptId"]) + "";
                        int PromptId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            DataTable dtset = new DataTable();
            Hashtable ht = new Hashtable();
            dtset = objData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtset != null)
            {
                if (dtset.Rows.Count > 0)
                {
                    foreach (DataRow row in dtset.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSet(SchoolId,DSTempHdrId,PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn,DistractorSamples,DistractorSamplesCount) ";
                        strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + loginid + ",getdate(),DistractorSamples,DistractorSamplesCount FROM DSTempSet WHERE DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
                        int SetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        if (!ht.ContainsKey(row["DSTempSetId"]))
                        {
                            ht.Add(row["DSTempSetId"], SetId);
                        }
                    }
                }
            }
            string teachingProc = "";
            string sqlStr = "";
            sqlStr = "SELECT DH.LessonPlanId,ISNULL(LU.LookupName,'') AS TeachingProc,ISNULL(LUp.LookupName,'') as PromptProc ,SkillType,ISNULL(NbrOfTrials,0) as NbrOfTrials," +
                    "LP.LessonPlanName,ISNULL(LP.Materials,'') as Mat,ISNULL(ChainType,'') AS ChainType,DH.IsVisualTool,ISNULL(DH.VTLessonId,0) as VTLessonId,ISNULL(ModificationInd,0) as ModificationInd FROM DSTempHdr DH JOIN LessonPlan LP ON LP.LessonPlanId=DH.LessonPlanId LEFT " +
                    "JOIN LookUp LU ON TeachingProcId=LU.LookUpId INNER JOIN Lookup LUp ON LUp.LookupId=PromptTypeId WHERE DSTempHdrId=" + templateid;
            DataTable dtTmpHdrDtls = objData.ReturnDataTable(sqlStr, false);
            if (dtTmpHdrDtls != null)
            {
                if (dtTmpHdrDtls.Rows.Count > 0)
                {
                    teachingProc = dtTmpHdrDtls.Rows[0]["TeachingProc"].ToString();
                }
            }
            if (teachingProc == "Match-to-Sample")
            {
                DataTable dtstep = new DataTable();
                dtstep = objData.ReturnDataTable("SELECT DSTempStepId,DSTempSetId FROM DSTempStep WHERE DSTempHdrId=" + templateid + " AND IsDynamic=0", Con, Trans, false);
                if (dtstep.Rows.Count > 0)
                {
                    foreach (DataRow row in dtstep.Rows)
                    {
                        oldSetId = Convert.ToInt32(row["DSTempSetId"]);
                        if (oldSetId != 0)
                        {
                            parentSetId = SetUpdateCopy(oldSetId, TId, Trans, Con);
                        }
                        strQuery =
                        strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                        strQuery += "SELECT SchoolId," + TId + "," + parentSetId + ",PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()	FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " ";
                        int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                    }
                }
            }
            else
            {
                int oldParentSetId = 0;
                DataTable dtParentStep = new DataTable();
                // strQuery = "INSERT INTO DSTempParentStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn) ";
                strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn"
                    + " FROM DSTempParentStep WHERE DSTempHdrId = " + templateid;
                dtParentStep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                //  int DSTempParentStepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                // DataTable dt
                if (dtParentStep != null)
                {
                    if (dtParentStep.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtParentStep.Rows)
                        {
                            string newsetids = "";
                            foreach (string setid in row["SetIds"].ToString().Split(','))
                            {
                                if (setid != "")
                                {
                                    if (ht.ContainsKey(Convert.ToInt32(setid)))
                                    {
                                        newsetids += ht[Convert.ToInt32(setid)] + ",";
                                    }
                                }
                            }
                            oldParentSetId = Convert.ToInt32(row["DSTempParentStepId"]);
                            strQuery = "INSERT INTO DSTempParentStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn) "
                                        + "SELECT  SchoolId," + TId + ",StepCd,StepName,DSTempSetId,SortOrder,'" + newsetids + "',SetNames,ActiveInd," + loginid + ",getdate()"
                                        + " FROM DSTempParentStep WHERE DSTempHdrId = " + templateid + " AND DSTempParentStepId=" + oldParentSetId;
                            parentSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            DataTable dtstep = new DataTable();

                            strQuery = "SELECT  SchoolId,PrevStepId,SortOrder,PreDefinedInd,CustomById,VTStepId,DSTempSetId,StepCd,StepName,ActiveInd,"
                                    + "DSTempParentStepId FROM DSTempStep WHERE DSTempParentStepId=" + oldParentSetId + " AND DSTempHdrId = " + templateid;
                            dtstep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                            if (dtstep.Rows.Count > 0)
                            {
                                foreach (DataRow rows in dtstep.Rows)
                                {
                                    oldSetId = Convert.ToInt32(rows["DSTempSetId"]);

                                    strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                    strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + loginid + ",ActiveInd,GETDATE()"
                                        + "	FROM DSTempStep WHERE DSTempSetId = " + oldSetId + " AND DSTempParentStepId=" + oldParentSetId;
                                    int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    strQuery = "SELECT DSTempSetId FROM DSTempStep WHERE DSTempStepId=" + StepId;
                                    int NewSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    if (ht.ContainsKey(Convert.ToInt32(NewSetId)))
                                    {
                                        newsetids = ht[Convert.ToInt32(NewSetId)].ToString();
                                        strQuery = "UPDATE DSTempStep SET DSTempSetId=" + Convert.ToInt32(ht[Convert.ToInt32(NewSetId)]) + ",DSTempParentStepId=" + parentSetId + ""
                                            + " WHERE DSTempStepId=" + StepId;
                                        int updateId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                    }

                                }
                            }

                        }
                    }
                }
            }

            DataTable dtsetcol = new DataTable();
            dtsetcol = objData.ReturnDataTable("SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId=" + templateid + "", Con, Trans, false);
            if (dtsetcol != null)
            {
                if (dtsetcol.Rows.Count > 0)
                {
                    foreach (DataRow row in dtsetcol.Rows)
                    {
                        strQuery = "INSERT INTO DSTempSetCol(SchoolId, DSTempHdrId,ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp	,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd,CreatedBy,CreatedOn,MoveUpstat) ";
                        strQuery += "SELECT SchoolId, " + TId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,CalcuType,CalcuData,ActiveInd," + loginid + ",CreatedOn,MoveUpstat FROM DSTempSetCol WHERE DSTempSetColId = " + Convert.ToInt32(row["DSTempSetColId"]) + " ";
                        int setColNewId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        DataTable dtsetcolcalc = new DataTable();
                        dtsetcolcalc = objData.ReturnDataTable("SELECT DSTempSetColCalcId FROM DSTempSetColCalc WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + "", Con, Trans, false);
                        if (dtsetcolcalc.Rows.Count > 0)
                        {
                            foreach (DataRow rowc in dtsetcolcalc.Rows)
                            {
                                strQuery = "INSERT INTO DSTempSetColCalc(SchoolId,DSTempSetColId,CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn,IncludeInGraph) " +
                                            "SELECT SchoolId," + setColNewId + ",CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd," + loginid + ",getdate(),IncludeInGraph FROM DSTempSetColCalc WHERE DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + "";
                                int setColCalId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                                strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) "; //--- [New Criteria] May 2020 ---//
                                strQuery += "SELECT  " + TId + ",SchoolId," + setColNewId + "," + setColCalId + ",RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ISNULL(ConsequetiveAvgInd,0) AS ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=" + Convert.ToInt32(row["DSTempSetColId"]) + " And DSTempSetColCalcId=" + Convert.ToInt32(rowc["DSTempSetColCalcId"]) + " "; //--- [New Criteria] May 2020 ---//
                                int lastId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                            }
                        }

                    }
                    strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) "; //--- [New Criteria] May 2020 ---//
                    strQuery += "SELECT  " + TId + ",SchoolId,0,0,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ISNULL(ConsequetiveAvgInd,0) AS ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd," //--- [New Criteria] May 2020 ---//
                        + "LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE" +
                        " DSTempSetColId=0 And DSTempSetColCalcId=0 AND DSTempHdrId=" + templateid;
                    int lastModRuleId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                }
            }
            objData.CommitTransation(Trans, Con);
            return TId;
        }
        catch (Exception Ex)
        {
            objData.RollBackTransation(Trans, Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            return 0;
        }
    }

    private string checkversion(string version)
    {
        if (version != null && version != "")
        {
            string[] tempver = version.Split('.');
            int first = Convert.ToInt32(tempver[0]);
            int second = Convert.ToInt32(tempver[1]);
            if (second == 9)
            {
                first = first + 1;
                second = 0;
            }
            else
                second = second + 1;
            version = first.ToString() + "." + second.ToString();
            return version;
        }
        else
            return version = "1.1";

    }


    public int SetUpdateCopy(int oldsetId, int newheaderId, SqlTransaction Trans, SqlConnection Con)
    {
        objData = new clsData();
        string strquerry = "";
        string setCd = "";
        int sortorder = 0;
        int parentSetId = 0;
        strquerry = "SELECT SetCd,SetName,SortOrder FROM DSTempSet WHERE DSTempSetId = " + oldsetId + " AND ActiveInd = 'A'";
        DataTable dtList = objData.ReturnDataTable(strquerry, Con, Trans, false);
        if (dtList != null)
        {
            if (dtList.Rows.Count > 0)
            {
                setCd = dtList.Rows[0]["SetCd"].ToString();
                sortorder = Convert.ToInt32(dtList.Rows[0]["SortOrder"]);

                strquerry = "SELECT DSTempSetId FROM DSTempSet WHERE DsTempHdrId = " + newheaderId + " AND SetCd = '" + clsGeneral.convertQuotes(setCd) + "' AND SortOrder = " + sortorder;

                DataTable dtNew = objData.ReturnDataTable(strquerry, Con, Trans, false);

                if (dtNew != null)
                {
                    if (dtNew.Rows.Count > 0)
                    {
                        parentSetId = Convert.ToInt32(dtNew.Rows[0]["DSTempSetId"]);
                    }
                }

            }
        }
        return parentSetId;

    }
    public int SaveTemplateDetails(int SchoolId, int StudentId, int LessonId, string LessonName, int CreatedBy, int StdtLesPlanId, SqlConnection Con, SqlTransaction Trans)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        //SqlTransaction Trans = null;
        //SqlConnection Con = new SqlConnection();
        objData = new clsData();
        int OldTempId = 0;
        int TId = 0;
        int oldSetId = 0;
        int parentSetId = 0;
        string strQuery = "";
        try
        {
            //Con = objData.Open();
            //Trans = Con.BeginTransaction();
            strQuery = "SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LessonId + " and StudentId IS NULL ";
            bool flag = objData.IFExistsWithTranss(strQuery, Trans, Con);
            if (flag == true)
            {

                strQuery = "SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LessonId + " AND StudentId=" + StudentId + " AND (StatusId<>(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Expired') AND StatusId<>(SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Deleted')) ";
                TId = Convert.ToInt32(objData.FetchValueTrans(strQuery, Trans, Con));

                if (TId > 0)
                {
                    strQuery = "UPDATE DSTempHdr SET PrevStatus='ST-'+CONVERT(VARCHAR(50),(SELECT StatusId FROM DSTempHdr WHERE DSTempHdrId=" + TId + ")) WHERE DSTempHdrId=" + TId;
                    objData.ExecuteWithTrans(strQuery, Con, Trans);
                    strQuery = "UPDATE DSTempHdr SET StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress') WHERE DSTempHdrId=" + TId;
                    objData.ExecuteWithTrans(strQuery, Con, Trans);
                }
                else if (TId == 0)
                {
                    strQuery = "SELECT MAX(DSTempHdrId) FROM dbo.DSTempHdr WHERE LessonPlanId=" + LessonId + " and StudentId IS NULL ";
                    OldTempId = Convert.ToInt32(objData.FetchValueTrans(strQuery, Trans, Con));


                    strQuery = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,StdtLessonplanId,DSTemplateName,BaselineStart,BaselineEnd,VTLessonId,[CompCurrInd],MultiStepInd," +
                               "MultiSetsInd,CurrVerInd,VerBeginDate,VerEndDate,DSTemplateDesc,StatusId,TeachingProcId,SkillType,MatchToSampleType,MatchToSampleRecOrExp,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool," +
                               "Baseline,Objective,GeneralProcedure,BaselineProc,CorrRespDef,StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness," +
                               "StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse,CreatedBy,CreatedOn,LessonPlanGoal,[FrameandStrand],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[LessonOrder]) SELECT " + SchoolId + "," + StudentId + "," + LessonId + "," +
                               "'" + StdtLesPlanId + "','" + LessonName + "',BaselineStart,BaselineEnd,VTLessonId,[CompCurrInd],MultiStepInd,MultiSetsInd,CurrVerInd,VerBeginDate,VerEndDate," +
                               "DSTemplateDesc,(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress'),TeachingProcId,SkillType,MatchToSampleType,MatchToSampleRecOrExp,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,GeneralProcedure,BaselineProc,CorrRespDef," +
                               "StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting," +
                               "LessonDefInst,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse,CreatedBy,getdate(),LessonPlanGoal,(SELECT [FrameandStrand] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") FrameandStrand,(SELECT [SpecStandard] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecStandard,(SELECT [SpecEntryPoint]  FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecEntryPoint,(SELECT [PreReq] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") PreReq,(SELECT [Materials] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") Materials,(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + StudentId + ") LessonOrder FROM DSTempHdr WHERE DSTempHdrId = " + OldTempId;

                    TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                    strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) ";
                    strQuery += "SELECT " + TId + ",PromptId,PromptOrder,ActiveInd," + CreatedBy + ",CreatedOn FROM DSTempPrompt WHERE DSTempHdrId=" + OldTempId + "";
                    int PromptId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                    DataTable dtset = new DataTable();
                    Hashtable ht = new Hashtable();
                    dtset = objData.ReturnDataTable("SELECT DSTempSetId FROM DSTempSet WHERE DSTempHdrId=" + OldTempId + "", Con, Trans, false);
                    if (dtset != null)
                    {
                        if (dtset.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtset.Rows)
                            {
                                strQuery = "INSERT INTO DSTempSet(SchoolId,DSTempHdrId,PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn,DistractorSamples,DistractorSamplesCount) ";
                                strQuery += "SELECT  SchoolId," + TId + ",PrevSetId,SetCd,SetName,Samples,SortOrder,ActiveInd," + oSession.LoginId + ",getdate(),DistractorSamples,DistractorSamplesCount FROM DSTempSet WHERE DSTempSetId = " + Convert.ToInt32(row["DSTempSetId"]) + " ";
                                int SetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                if (!ht.ContainsKey(row["DSTempSetId"]))
                                {
                                    ht.Add(row["DSTempSetId"], SetId);
                                }
                            }
                        }
                    }
                    string teachingProc = "";
                    string sqlStr = "";
                    sqlStr = "SELECT DH.LessonPlanId,ISNULL(LU.LookupName,'') AS TeachingProc,ISNULL(LUp.LookupName,'') as PromptProc ,SkillType,ISNULL(NbrOfTrials,0) as NbrOfTrials," +
                            "LP.LessonPlanName,ISNULL(LP.Materials,'') as Mat,ISNULL(ChainType,'') AS ChainType,DH.IsVisualTool,ISNULL(DH.VTLessonId,0) as VTLessonId,ISNULL(ModificationInd,0) as ModificationInd FROM DSTempHdr DH JOIN LessonPlan LP ON LP.LessonPlanId=DH.LessonPlanId LEFT " +
                            "JOIN LookUp LU ON TeachingProcId=LU.LookUpId INNER JOIN Lookup LUp ON LUp.LookupId=PromptTypeId WHERE DSTempHdrId=" + OldTempId;
                    DataTable dtTmpHdrDtls = objData.ReturnDataTable(sqlStr, false);
                    if (dtTmpHdrDtls != null)
                    {
                        if (dtTmpHdrDtls.Rows.Count > 0)
                        {
                            teachingProc = dtTmpHdrDtls.Rows[0]["TeachingProc"].ToString();
                        }
                    }
                    if (teachingProc == "Match-to-Sample")
                    {
                        DataTable dtstep = new DataTable();
                        dtstep = objData.ReturnDataTable("SELECT DSTempStepId,DSTempSetId FROM DSTempStep WHERE DSTempHdrId=" + OldTempId + "", Con, Trans, false);
                        if (dtstep.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtstep.Rows)
                            {
                                oldSetId = Convert.ToInt32(row["DSTempSetId"]);
                                if (oldSetId != 0)
                                {
                                    parentSetId = SetUpdateCopy(oldSetId, TId, Trans, Con);
                                }
                                strQuery =
                                strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                strQuery += "SELECT SchoolId," + TId + "," + parentSetId + ",PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + oSession.LoginId + ",ActiveInd,GETDATE()	FROM DSTempStep WHERE DSTempStepId = " + Convert.ToInt32(row["DSTempStepId"]) + " ";
                                int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                            }
                        }
                    }
                    else
                    {
                        int oldParentSetId = 0;
                        DataTable dtParentStep = new DataTable();
                        // strQuery = "INSERT INTO DSTempParentStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery = "SELECT  DSTempParentStepId,SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn"
                            + " FROM DSTempParentStep WHERE DSTempHdrId = " + OldTempId;
                        dtParentStep = objData.ReturnDataTable(strQuery, Con, Trans, false);
                        int DSTempParentStepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        // DataTable dt
                        if (dtParentStep.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtParentStep.Rows)
                            {
                                string newsetids = "";
                                foreach (string setid in row["SetIds"].ToString().Split(','))
                                {
                                    if (setid != "")
                                    {
                                        if (ht.ContainsKey(Convert.ToInt32(setid)))
                                        {
                                            newsetids += ht[Convert.ToInt32(setid)] + ",";
                                        }
                                    }
                                }
                                oldParentSetId = Convert.ToInt32(row["DSTempParentStepId"]);
                                strQuery = "INSERT INTO DSTempParentStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,SetIds,SetNames,ActiveInd,CreatedBy,CreatedOn) "
                                            + "SELECT  SchoolId," + TId + ",StepCd,StepName,DSTempSetId,SortOrder,'" + newsetids + "',SetNames,ActiveInd," + oSession.LoginId + ",getdate()"
                                            + " FROM DSTempParentStep WHERE DSTempHdrId = " + OldTempId + " AND DSTempParentStepId=" + oldParentSetId;
                                parentSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                                DataTable dtstep = new DataTable();

                                strQuery = "SELECT  SchoolId,PrevStepId,SortOrder,PreDefinedInd,CustomById,VTStepId,DSTempSetId,StepCd,StepName,ActiveInd,"
                                        + "DSTempParentStepId FROM DSTempStep WHERE DSTempParentStepId=" + oldParentSetId + " AND DSTempHdrId = " + OldTempId;
                                dtstep = objData.ReturnDataTableWithTransaction(strQuery, Con, Trans, false);
                                if (dtstep.Rows.Count > 0)
                                {
                                    foreach (DataRow rows in dtstep.Rows)
                                    {
                                        oldSetId = Convert.ToInt32(rows["DSTempSetId"]);

                                        strQuery = "INSERT INTO DSTempStep(SchoolId,DSTempHdrId,DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder,CreatedBy,ActiveInd,CreatedOn) ";
                                        strQuery += "SELECT SchoolId," + TId + ",DSTempSetId,PrevStepId,DSTempParentStepId,StepCd,StepName,SortOrder," + oSession.LoginId + ",ActiveInd,GETDATE()"
                                            + "	FROM DSTempStep WHERE DSTempSetId = " + oldSetId + " AND DSTempParentStepId=" + oldParentSetId;
                                        int StepId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                        strQuery = "SELECT DSTempSetId FROM DSTempStep WHERE DSTempStepId=" + StepId;
                                        int NewSetId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                        if (ht.ContainsKey(Convert.ToInt32(NewSetId)))
                                        {
                                            newsetids = ht[Convert.ToInt32(NewSetId)].ToString();
                                            strQuery = "UPDATE DSTempStep SET DSTempSetId=" + Convert.ToInt32(ht[Convert.ToInt32(NewSetId)]) + ",DSTempParentStepId=" + parentSetId + ""
                                                + " WHERE DSTempStepId=" + StepId;
                                            int updateId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                                        }

                                    }
                                }

                            }
                        }
                    }



                    generateSetCol(SchoolId, StudentId, OldTempId, TId, CreatedBy, Trans, Con);




                }
                //else
                //{
                //    strQuery = "UPDATE DSTempHdr SET StatusId=(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress') WHERE DSTempHdrId=" + TId;
                //    objData.ExecuteWithScopeandConnection(strQuery, Con, Trans);
                //    strQuery = "DELETE FROM DSTempPrompt WHERE DSTempHdrId=" + TId;
                //    objData.ExecuteWithTrans(strQuery, Con, Trans);
                //    strQuery = "DELETE FROM DSTempSet WHERE DSTempHdrId=" + TId;
                //    objData.ExecuteWithTrans(strQuery, Con, Trans);
                //    strQuery = "DELETE FROM DSTempStep WHERE DSTempHdrId=" + TId;
                //    objData.ExecuteWithTrans(strQuery, Con, Trans);
                //    strQuery = "SELECT * FROM DSTempSetCol WHERE DSTempHdrId=" + TId;
                //    DataTable dtSetCol = objData.ReturnDataTableWithTransaction(strQuery, Trans, false);
                //    if (dtSetCol != null)
                //    {
                //        foreach (DataRow dr in dtSetCol.Rows)
                //        {
                //            strQuery = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId=" + dr["DSTempSetColId"].ToString();
                //            objData.ExecuteWithTrans(strQuery, Con, Trans);
                //            strQuery = "DELETE FROM DSTempRule WHERE DSTempSetColId=" + dr["DSTempSetColId"].ToString();
                //            objData.ExecuteWithTrans(strQuery, Con, Trans);
                //        }
                //    }
                //    strQuery = "DELETE FROM DSTempSetCol WHERE DSTempHdrId=" + TId;
                //    objData.ExecuteWithTrans(strQuery, Con, Trans);
                //}


            }
            else
            {
                string basProce = "";
                string gObject = "";
                if (HttpContext.Current.Session["Baseline"] != null && HttpContext.Current.Session["Objective"] != null)
                {
                    basProce = HttpContext.Current.Session["Baseline"].ToString();
                    gObject = HttpContext.Current.Session["Objective"].ToString();

                    basProce = clsGeneral.convertQuotes(basProce);
                    gObject = clsGeneral.convertQuotes(gObject);
                }
                strQuery = "INSERT INTO DSTempHdr(SchoolId,LessonPlanId,DSTemplateName,Baseline,Objective,StatusId,isDynamic,CreatedBy,CreatedOn) SELECT " + SchoolId + "," + LessonId + ",'" + LessonName + "','" + basProce + "','" + gObject + "',LookupId,1," + CreatedBy + ",getdate() FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress' ";
                TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                if(SchoolId==1)
                    strQuery = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,StdtLessonplanId,DSTemplateName,StatusId,TeachingProcId,SkillType,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,BaselineProc,GeneralProcedure,BaselineEnd,CorrRespDef,  " +
                    "StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,isDynamic,CreatedBy,CreatedOn,[FrameandStrand],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[LessonOrder],[LessonSDate],[LessonEDate]) SELECT " + SchoolId + "," + StudentId + "," + LessonId + ",'" + StdtLesPlanId + "','" + LessonName + "',StatusId,TeachingProcId,SkillType,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,GeneralProcedure,BaselineProc,BaselineEnd,CorrRespDef," +
                    "StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,isDynamic,1,getdate(),(SELECT [FrameandStrand] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") FrameandStrand,(SELECT [SpecStandard] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecStandard,(SELECT [SpecEntryPoint]  FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecEntryPoint,(SELECT [PreReq] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") PreReq,(SELECT [Materials] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") Materials,(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + StudentId + ") LessonOrder, " +
                    "(Select DISTINCT EffStartDate  from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.StudentId = " + StudentId + " AND StdtIEP.StatusId=65) LessonSDate," +
                    " (Select DISTINCT EffEndDate  from StDtLessonPlan inner join StDtIEP on StDtLessonPlan.StDtIEPId=StDtIEP.StDtIEPId where StDtLessonPlan.StudentId = " + StudentId + " AND StdtIEP.StatusId=65) LessonEdate FROM DSTempHdr WHERE DSTempHdrId = " + TId;
                if(SchoolId==2)
                     strQuery = "INSERT INTO DSTempHdr(SchoolId,StudentId,LessonPlanId,StdtLessonplanId,DSTemplateName,StatusId,TeachingProcId,SkillType,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,BaselineProc,GeneralProcedure,BaselineEnd,CorrRespDef,  " +
                    "StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,isDynamic,CreatedBy,CreatedOn,[FrameandStrand],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials],[LessonOrder],[LessonSDate],[LessonEDate]) SELECT " + SchoolId + "," + StudentId + "," + LessonId + ",'" + StdtLesPlanId + "','" + LessonName + "',StatusId,TeachingProcId,SkillType,NbrOfTrials,ChainType,TotalTaskFormat,PromptTypeId,IsVisualTool,Baseline,Objective,GeneralProcedure,BaselineProc,BaselineEnd,CorrRespDef," +
                    "StudCorrRespDef,IncorrRespDef,StudIncorrRespDef,CorrectionProc,ReinforcementProc,TeacherRespReadness,StudentReadCrita,MajorSetting,MinorSetting,LessonDefInst,isDynamic,1,getdate(),(SELECT [FrameandStrand] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") FrameandStrand,(SELECT [SpecStandard] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecStandard,(SELECT [SpecEntryPoint]  FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") SpecEntryPoint,(SELECT [PreReq] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") PreReq,(SELECT [Materials] FROM LessonPlan WHERE LessonPlanId=" + LessonId + ") Materials,(select isnull( max(LessonOrder)+1,1) from dstemphdr where studentid=" + StudentId + ") LessonOrder, " +
                    "(Select DISTINCT EffStartDate  from StDtLessonPlan inner join stdtIep_PE on StDtLessonPlan.StDtIEPId=stdtIep_PE.StdtIEP_PEId where StDtLessonPlan.StudentId ="+  StudentId + " AND StdtIEP_PE.StatusId=65) LessonSDate," +
                    " (Select DISTINCT EffEndDate  from StDtLessonPlan inner join stdtIep_PE on StDtLessonPlan.StDtIEPId=stdtIep_PE.StdtIEP_PEId where StDtLessonPlan.StudentId =" + StudentId + " AND StdtIEP_PE.StatusId=65) LessonEdate FROM DSTempHdr WHERE DSTempHdrId = " + TId;
               

                TId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

            }


            OldTempId = TId;





        }
        catch (Exception Ex)
        {
            //objData.RollBackTransation(Trans, Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
            throw Ex;
            return 0;
        }
        finally
        {

        }
        return TId;
    }
    private void generateSetCol(int SchoolId, int StudentId, int TempId, int CurrentTempId, int CreatedBy, SqlTransaction Trans, SqlConnection Con)
    {
        Dt = new DataTable();
        DataTable DtSetCol = new DataTable();
        objData = new clsData();
        string strFields = " (";
        string strValues = " VALUES (";
        string strQuery = "";
        int setColId = 0;
        int setColCalId = 0;
        int setColNewId = 0;
        int Id = 0;

        try
        {
            Dt = objData.ReturnDataTable("SELECT * FROM DSTempSetCol WHERE DSTempHdrId=" + TempId + "", Con, Trans, false);
            if (Dt != null)
            {

                if (Dt.Rows.Count > 0)
                {

                    strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) "; //--- [New Criteria] May 2020 ---//
                    strQuery += "SELECT  " + CurrentTempId + ",SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ISNULL(ConsequetiveAvgInd,0) AS ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=0 And DSTempSetColCalcId=0 And CriteriaType='MODIFICATION' And DSTempHdrId=" + TempId + " "; //--- [New Criteria] May 2020 ---//
                    int modId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        setColId = Convert.ToInt32(Dr["DSTempSetColId"]);
                        strQuery = "INSERT INTO DSTempSetCol(SchoolId, DSTempHdrId,ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp	,InCorrStdResp,IncMisTrialInd,MisTrialDesc,ActiveInd,CreatedBy,CreatedOn) ";
                        strQuery += "SELECT " + SchoolId + ", " + CurrentTempId + ",ColName,ColTypeCd,CorrRespType,CorrResp,CorrRespDesc	,InCorrRespDesc,CorrStdtResp,InCorrStdResp,IncMisTrialInd,MisTrialDesc,ActiveInd," + CreatedBy + ",CreatedOn FROM DSTempSetCol WHERE DSTempSetColId = " + setColId + " ";
                        setColNewId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));


                        strQuery = " SELECT DSTempSetColCalcId, " + SchoolId + ", " + setColNewId + ",CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn FROM DSTempSetColCalc   WHERE DSTempSetColId=" + setColId + "";
                        DtSetCol = objData.ReturnDataTable(strQuery, Con, Trans, false);
                        int ColCount = DtSetCol.Columns.Count;
                        int i = 0;
                        foreach (DataRow DrSC in DtSetCol.Rows)
                        {
                            strQuery = "";
                            strValues = " VALUES (";
                            Id = Convert.ToInt32(DrSC["DSTempSetColCalcId"]);
                            while (i < ColCount)
                            {
                                if (i == 0) { i++; continue; };
                                if (i == 2) { i++; strValues += "'" + setColNewId + "',"; continue; }
                                if (i == 8) { i++; strValues += "'" + CreatedBy + "',"; continue; }
                                if (i == 9) { i++; strValues += "getdate(),"; continue; }
                                strValues += "'" + DrSC[i].ToString() + "',";
                                i++;
                            }
                            i = 0;
                            strValues = strValues.Substring(0, strValues.Length - 1) + ")";
                            strQuery = "INSERT INTO DSTempSetColCalc(SchoolId,DSTempSetColId,CalcType,CalcLabel,CalcFormula,CalcRptLabel,ActiveInd,CreatedBy,CreatedOn) " + strValues;

                            setColCalId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));

                            strQuery = "INSERT INTO DSTempRule(DSTempHdrId,SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn) "; //--- [New Criteria] May 2020 ---//
                            strQuery += "SELECT  " + CurrentTempId + ",SchoolId," + setColNewId + "," + setColCalId + ",RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,ISNULL(ConsequetiveAvgInd,0) AS ConsequetiveAvgInd,MultiTeacherReqInd,IOAReqInd,LogicalCombType,ActiveInd,IsComment,IsNA,ModificationComment,ModificationRule,CreatedBy,CreatedOn FROM DSTempRule WHERE DSTempSetColId=" + setColId + " And DSTempSetColCalcId=" + Id + " "; //--- [New Criteria] May 2020 ---//
                            int lastId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, Con, Trans));
                        }


                    }

                }

            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }



    }


    public void Delete_Reorder_setStep(int Id, string Type)
    {
        objData = new clsData();
        try
        {
            using (cmd = new SqlCommand())
            {
                SqlConnection con = objData.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Delete_Reorder_setStep";
                SqlParameter pId, pType;

                pId = new SqlParameter("@id", SqlDbType.Int);
                cmd.Parameters.Add(pId);
                pId.Value = Id;

                pType = new SqlParameter("@type", SqlDbType.VarChar);
                cmd.Parameters.Add(pType);
                pType.Value = Type;

                object content = cmd.ExecuteScalar();
                objData.Close(con);
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
    }


}