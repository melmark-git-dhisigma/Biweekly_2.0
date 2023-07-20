/*
 * File name : /DynamicGridWithTemplateColumn/ DataClass.cs
 * Class file used for database connectivity.
 * Created By: Arun .M
 * Date:21 -06-2012
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


/// <summary>
/// Summary description for DataClass
/// </summary>
public class DataClass
{


    public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());

    public SqlParameter @studentId = new SqlParameter();
    public SqlParameter @ClassId = new SqlParameter();
    public SqlParameter @schoolId = new SqlParameter();
    public SqlParameter @lessonplan_id = new SqlParameter();

    /// <summary>
    /// Template Parameters
    /// </summary>

    public SqlParameter @temp_id = new SqlParameter();
    public SqlParameter @temp_name = new SqlParameter();
    public SqlParameter @temp_desc = new SqlParameter();
    public SqlParameter @skill_type = new SqlParameter();
    public SqlParameter @chain_type = new SqlParameter();
    public SqlParameter @prompt_type = new SqlParameter();
    public SqlParameter @compCurrInd = new SqlParameter();


    /// <summary>
    /// Set Parameters
    /// </summary>

    public SqlParameter @setName = new SqlParameter();
    public SqlParameter @setId = new SqlParameter();
    public SqlParameter @setDesc = new SqlParameter();
    public SqlParameter @sortOrdr = new SqlParameter();


    /// <summary>
    /// Session Parameters
    /// </summary>

    public SqlParameter @sess_Status = new SqlParameter();
    public SqlParameter @IOASessId = new SqlParameter();
    public SqlParameter @sess_nbr = new SqlParameter();
    public SqlParameter @startTime = new SqlParameter();
    public SqlParameter @endTime = new SqlParameter();
    public SqlParameter @assign_Ind = new SqlParameter();
    public SqlParameter @curSet_Id = new SqlParameter();
    public SqlParameter @curStep_Id = new SqlParameter();
    public SqlParameter @curProm_Id = new SqlParameter();
    public SqlParameter @SessStus_Cd = new SqlParameter();
    public SqlParameter @IOA_Ind = new SqlParameter();
    public SqlParameter @IOA_UserId = new SqlParameter();


    /// <summary>
    /// Session Details Parameters
    /// </summary>
    /// 

    public SqlParameter @row_number = new SqlParameter();
    public SqlParameter @StdtSessionHdrId = new SqlParameter();
    public SqlParameter @TrialNbr = new SqlParameter();
    public SqlParameter @TrialName = new SqlParameter();
    public SqlParameter @SessionStatusCd = new SqlParameter();
    public SqlParameter @Comments = new SqlParameter();
    public SqlParameter @Duration = new SqlParameter();
    public SqlParameter @StdtSessionStepId = new SqlParameter();
    public SqlParameter @DSTempSetColId = new SqlParameter();
    public SqlParameter @DSTempSetColCalcId = new SqlParameter();
    public SqlParameter @StepVal = new SqlParameter();


    /// <summary>
    /// Column Parameters and Step Parameters
    /// </summary>

    public SqlParameter @colum_id = new SqlParameter();
    public SqlParameter @colum_type = new SqlParameter();
    public SqlParameter @step_name = new SqlParameter();
    public SqlParameter @step_desc = new SqlParameter();

    /// <summary>
    /// Score Parameters
    /// </summary>

    public SqlParameter @scoreSheetId = new SqlParameter();
    public SqlParameter @answer = new SqlParameter();
    public SqlParameter @PercAccuracy = new SqlParameter();
    public SqlParameter @PercIndependant = new SqlParameter();
    public SqlParameter @PercPromptr = new SqlParameter();
    public SqlParameter @AvgDuration = new SqlParameter();
    public SqlParameter @TotoDuration = new SqlParameter();
    public SqlParameter @Frequency = new SqlParameter();

    public SqlParameter @goalId = new SqlParameter();
    public SqlParameter @assmntYr = new SqlParameter();
    public SqlParameter @stat = new SqlParameter();
    public SqlParameter @by = new SqlParameter();
    public SqlParameter @on = new SqlParameter();




    public DataClass()
    {
        //
        // TODO: Add constructor logic here
        con.Close();
        //
    }
    public SqlConnection Connect()
    {
        if (con.State == ConnectionState.Closed) con.Open();
        return con;
    }
    public SqlConnection CloseConnection()
    {
        if (con.State == ConnectionState.Open) con.Close();
        return con;
    }




    /// <summary>
    /// Function to Execute query for creating a new Template and Insert vales.
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public int ExecuteInsertTemplateQuery(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {

            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@studentId", SqlDbType.Int);
            cmd.Parameters["@studentId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@lessonplan_id", SqlDbType.Int);
            cmd.Parameters["@lessonplan_id"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@temp_name", SqlDbType.VarChar);
            cmd.Parameters["@temp_name"].Value = argument[3].ToString();

            cmd.Parameters.Add("@temp_desc", SqlDbType.VarChar);
            cmd.Parameters["@temp_desc"].Value = argument[4].ToString();

            cmd.Parameters.Add("@skill_type", SqlDbType.VarChar);
            cmd.Parameters["@skill_type"].Value = argument[5].ToString();

            cmd.Parameters.Add("@chain_type", SqlDbType.VarChar);
            cmd.Parameters["@chain_type"].Value = argument[6].ToString();

            cmd.Parameters.Add("@prompt_type", SqlDbType.Int);
            cmd.Parameters["@prompt_type"].Value = Convert.ToInt32(argument[7]);

            cmd.Parameters.Add("@compCurrInd", SqlDbType.VarChar);
            cmd.Parameters["@compCurrInd"].Value = argument[8].ToString();

            cmd.Parameters.Add("@create_by", SqlDbType.VarChar);
            cmd.Parameters["@create_by"].Value = argument[9].ToString();

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[10]);

            id = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function for Execute query to creating a colum for template and insert its values
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public int ExecuteInsertTemplateColumnQuery(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@temp_id", SqlDbType.Int);
            cmd.Parameters["@temp_id"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@colum_type", SqlDbType.VarChar);
            cmd.Parameters["@colum_type"].Value = argument[2].ToString();

            cmd.Parameters.Add("@create_by", SqlDbType.VarChar);
            cmd.Parameters["@create_by"].Value = argument[3].ToString();

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[4]);

            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function excute query to create Sets and insert the questions 
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public int ExecuteInsertTemplateSetsQuery(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@temp_id", SqlDbType.Int);
            cmd.Parameters["@temp_id"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@setName", SqlDbType.VarChar);
            cmd.Parameters["@setName"].Value = argument[2].ToString();

            cmd.Parameters.Add("@setDesc", SqlDbType.VarChar);
            cmd.Parameters["@setDesc"].Value = argument[3].ToString();

            cmd.Parameters.Add("@sort_order", SqlDbType.Int);
            cmd.Parameters["@sort_order"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@create_by", SqlDbType.VarChar);
            cmd.Parameters["@create_by"].Value = argument[5].ToString();

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[6]);

            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }




    /// <summary>
    /// Function excute query to create a set of steps or questions and insert the questions 
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public int ExecuteInsertTemplateStepsQuery(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@temp_id", SqlDbType.Int);
            cmd.Parameters["@temp_id"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@step_name", SqlDbType.VarChar);
            cmd.Parameters["@step_name"].Value = argument[2].ToString();

            cmd.Parameters.Add("@step_desc", SqlDbType.VarChar);
            cmd.Parameters["@step_desc"].Value = argument[3].ToString();

            cmd.Parameters.Add("@sort_order", SqlDbType.Int);
            cmd.Parameters["@sort_order"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@create_by", SqlDbType.VarChar);
            cmd.Parameters["@create_by"].Value = argument[5].ToString();

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[6]);

            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function to Execute query for creating a new Session and Insert vales on StdtSessionHdr.
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    public int ExecuteInsertSessionQuery(SqlCommand cmd, String[] argument)
    {
        // SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {

            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@studentId", SqlDbType.Int);
            cmd.Parameters["@studentId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@temp_id", SqlDbType.Int);
            cmd.Parameters["@temp_id"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@ClassId", SqlDbType.Int);
            cmd.Parameters["@ClassId"].Value = Convert.ToInt32(argument[3]);

            cmd.Parameters.Add("@lessonplan_id", SqlDbType.Int);
            cmd.Parameters["@lessonplan_id"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@IOASessId", SqlDbType.Int);
            cmd.Parameters["@IOASessId"].Value = Convert.ToInt32(argument[5]);

            cmd.Parameters.Add("@sess_nbr", SqlDbType.Int);
            cmd.Parameters["@sess_nbr"].Value = Convert.ToInt32(argument[6]);

            cmd.Parameters.Add("@startTime", SqlDbType.DateTime);
            cmd.Parameters["@startTime"].Value = Convert.ToDateTime(argument[7]);

            cmd.Parameters.Add("@endTime", SqlDbType.DateTime);
            cmd.Parameters["@endTime"].Value = Convert.ToDateTime(argument[8]);

            cmd.Parameters.Add("@assign_Ind", SqlDbType.Int);
            cmd.Parameters["@assign_Ind"].Value = Convert.ToInt32(argument[9]);

            cmd.Parameters.Add("@curSet_Id", SqlDbType.Int);
            cmd.Parameters["@curSet_Id"].Value = Convert.ToInt32(argument[10]);

            cmd.Parameters.Add("@curStep_Id", SqlDbType.Int);
            cmd.Parameters["@curStep_Id"].Value = Convert.ToInt32(argument[11]);

            cmd.Parameters.Add("@curProm_Id", SqlDbType.Int);
            cmd.Parameters["@curProm_Id"].Value = Convert.ToInt32(argument[12]);

            cmd.Parameters.Add("@SessStus_Cd", SqlDbType.VarChar);
            cmd.Parameters["@SessStus_Cd"].Value = argument[13].ToString();

            cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
            cmd.Parameters["@Comments"].Value = argument[14].ToString();

            cmd.Parameters.Add("@IOA_Ind", SqlDbType.VarChar);
            cmd.Parameters["@IOA_Ind"].Value = argument[15].ToString();

            cmd.Parameters.Add("@IOA_UserId", SqlDbType.Int);
            cmd.Parameters["@IOA_UserId"].Value = Convert.ToInt32(argument[16]);

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[17]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[18]);

            id = Convert.ToInt32(cmd.ExecuteScalar());
            // CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function excute query to insert the Session Values
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteInsertSessionValuesQuery(SqlCommand cmd, String[] argument)
    {
        //SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@StdtSessionHdrId", SqlDbType.Int);
            cmd.Parameters["@StdtSessionHdrId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@setId", SqlDbType.Int);
            cmd.Parameters["@setId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@TrialNbr", SqlDbType.Int);
            cmd.Parameters["@TrialNbr"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@TrialName", SqlDbType.VarChar);
            cmd.Parameters["@TrialName"].Value = argument[3].ToString();

            cmd.Parameters.Add("@SessionStatusCd", SqlDbType.VarChar);
            cmd.Parameters["@SessionStatusCd"].Value = argument[4].ToString();

            cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
            cmd.Parameters["@Comments"].Value = argument[5].ToString();

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[6]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[7]);


            id = Convert.ToInt32(cmd.ExecuteScalar());
           
        }

        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function excute query to Update the Session step Values
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteUpdateSessionValuesQuery(SqlCommand cmd, String[] argument)
    {
        //SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@SessionStatusCd", SqlDbType.VarChar);
            cmd.Parameters["@SessionStatusCd"].Value = argument[0].ToString();

            cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
            cmd.Parameters["@Comments"].Value = argument[1].ToString();

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[3]);

            id = Convert.ToInt32(cmd.ExecuteScalar());
            //CloseConnection();
        }

        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }

    /// <summary>
    /// Function excute query to insert the score for a Datasheet
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteInsertSessionScoreValuesQuery(SqlCommand cmd, String[] argument)
    {
        //SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@schoolId", SqlDbType.Int);
            cmd.Parameters["@schoolId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@studentId", SqlDbType.Int);
            cmd.Parameters["@studentId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@DSTempSetColId", SqlDbType.Int);
            cmd.Parameters["@DSTempSetColId"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@DSTempSetColCalcId", SqlDbType.Int);
            cmd.Parameters["@DSTempSetColCalcId"].Value = Convert.ToInt32(argument[3]);

            cmd.Parameters.Add("@StdtSessionHdrId", SqlDbType.Int);
            cmd.Parameters["@StdtSessionHdrId"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@score", SqlDbType.Float);
            cmd.Parameters["@score"].Value = float.Parse(argument[5].ToString());

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[6]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[7]);


            id = Convert.ToInt32(cmd.ExecuteScalar());
            //CloseConnection();
        }

        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }



    /// <summary>
    /// Function excute quert to insert the score for a template
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteInsertSessionDtlsValuesQuery(SqlCommand cmd, String[] argument)
    {
        //SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {


            cmd.Parameters.Add("@StdtSessionStepId", SqlDbType.Int);
            cmd.Parameters["@StdtSessionStepId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@DSTempSetColId", SqlDbType.Int);
            cmd.Parameters["@DSTempSetColId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@StepVal", SqlDbType.VarChar);
            cmd.Parameters["@StepVal"].Value = argument[2].ToString();

            cmd.Parameters.Add("@row_number", SqlDbType.Int);
            cmd.Parameters["@row_number"].Value = Convert.ToInt32(argument[3]);

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[5]);

            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            //CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    /// <summary>
    /// Function excute quert to update the score for a template
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteUpdateSessionDtlsValuesQuery(SqlCommand cmd, String[] argument)
    {
        
        //SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {

            cmd.Parameters.Add("@StepVal", SqlDbType.VarChar);
            cmd.Parameters["@StepVal"].Value = argument[0].ToString();

            cmd.Parameters.Add("@create_by", SqlDbType.Int);
            cmd.Parameters["@create_by"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@create_on", SqlDbType.DateTime);
            cmd.Parameters["@create_on"].Value = Convert.ToDateTime(argument[2]);

            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            //CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }

    /// <summary>
    /// Function excute quert to insert the score for an assesment
    /// </summary>
    /// <param name="qry"></param>
    /// <returns></returns>

    public int ExecuteInsertScoreQuery(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {


            cmd.Parameters.Add("@scoreSheetId", SqlDbType.Int);
            cmd.Parameters["@scoreSheetId"].Value = Convert.ToInt32(argument[0]);

            cmd.Parameters.Add("@studentId", SqlDbType.Int);
            cmd.Parameters["@studentId"].Value = Convert.ToInt32(argument[1]);

            cmd.Parameters.Add("@temp_id", SqlDbType.Int);
            cmd.Parameters["@temp_id"].Value = Convert.ToInt32(argument[2]);

            cmd.Parameters.Add("@step_no", SqlDbType.Int);
            cmd.Parameters["@step_no"].Value = Convert.ToInt32(argument[3]);

            cmd.Parameters.Add("@answer", SqlDbType.VarChar);
            cmd.Parameters["@answer"].Value = argument[4].ToString();


            id = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    public int ExecuteInsertStdtGoal(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;


        cmd.Parameters.AddWithValue("@School", argument[0]);
        cmd.Parameters.AddWithValue("@Stdt", argument[1]);
        cmd.Parameters.AddWithValue("@Goal", argument[2]);
        cmd.Parameters.AddWithValue("@AssmntYr", argument[3]);
        cmd.Parameters.AddWithValue("@Stat", argument[4]);
        cmd.Parameters.AddWithValue("@By", argument[5]);
        cmd.Parameters.AddWithValue("@On", Convert.ToDateTime(argument[6]));

        id = Convert.ToInt32(cmd.ExecuteNonQuery());

        return id;
    }

    public int ExecuteNonQuery(String qry)
    {

        SqlCommand com = new SqlCommand(qry, Connect());
        int value;
        try
        {
            value = Convert.ToInt32(com.ExecuteNonQuery());
            CloseConnection();
        }
        catch (Exception ex)
        {
            value = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
        return value;
    }
    public int Execute(SqlCommand cmd)
    {
        int value;
        try
        {
            Connect();
            value = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseConnection();
        }
        catch (Exception ex)
        {
            value = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
        return value;
    }
    public int ExecuteNonQuery_sp(String qry, int schoolId, int studentId, int goalId, int createdBy)
    {

        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@schoolId", schoolId);
        com.Parameters.AddWithValue("@studentId", studentId);
        com.Parameters.AddWithValue("@goalId", goalId);
        com.Parameters.AddWithValue("@createdBy", createdBy);
        int value;
        try
        {

            value = Convert.ToInt32(com.ExecuteNonQuery());
            CloseConnection();
        }
        catch (Exception ex)
        {
            value = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
        finally
        {
            com.CommandType = CommandType.Text;
        }
        return value;
    }

    public int ExecuteScalar_sp(String qry, int ischoolId, int istudentId, int igoalId, int icreatedBy)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@schoolId", ischoolId);
        com.Parameters.AddWithValue("@studentId", istudentId);
        com.Parameters.AddWithValue("@goalId", igoalId);
        com.Parameters.AddWithValue("@createdBy", icreatedBy);
        int id;
        try
        {
            id = Convert.ToInt32(com.ExecuteScalar());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        //finally
        //{
        //    com.CommandType = CommandType.Text;
        //}

        return id;
    }

    public int EcecuteScalar_SpSetStep(String qry, int ischoolId, int iCreatedBy, int tempHeadrId, int vtLessonId)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@pschoolId", ischoolId);
        com.Parameters.AddWithValue("@pcreatedBy", iCreatedBy);
        com.Parameters.AddWithValue("@pTempHeadrId", tempHeadrId);
        com.Parameters.AddWithValue("@pVTLessonId", vtLessonId);        
        int id = 1;
        try
        {
            id = Convert.ToInt32(com.ExecuteScalar());
            CloseConnection();
        }
        catch (Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }

    public int Execute_SpCopyLesson(string qry, int lessonId,int isStEdit,int isCcEdit)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@pcopyLessonId", lessonId);
        com.Parameters.AddWithValue("@pIsStEdit", isStEdit);
        com.Parameters.AddWithValue("@pIsCCEdit", isCcEdit);
       // com.Parameters.AddWithValue("@pstudentName", studentName);
        int id = 0;
        try
        {         
            id = Convert.ToInt32(com.ExecuteScalar());                  
            CloseConnection();
        }
        catch (Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }


    public int Execute_SpCopyLessonOnRename(string qry, int lessonId, int isStEdit, int isCcEdit, string studentName,string newLessonName)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@pcopyLessonId", lessonId);
        com.Parameters.AddWithValue("@pIsStEdit", isStEdit);
        com.Parameters.AddWithValue("@pIsCCEdit", isCcEdit);
        com.Parameters.AddWithValue("@pstudentName", studentName);
        com.Parameters.AddWithValue("@prenameLessonName", newLessonName);
        int id = 0;
        try
        {
            id = Convert.ToInt32(com.ExecuteScalar());
            CloseConnection();
        }
        catch (Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }
    
    public int Execute_SpDeletion(string qry, int tempHeadrId)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@pTempHeaderId", tempHeadrId);
        int id = 1;
        try
        {
            id = Convert.ToInt32(com.ExecuteScalar());
            CloseConnection();
        }
        catch (Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());

        }
        return id;
    }
    public int ExecuteScalar(String qry)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        int id=0;
        try
        {

        if (com.ExecuteScalar() != DBNull.Value)
            {
            id = Convert.ToInt32(com.ExecuteScalar());
            }
            CloseConnection();
        }
        catch(Exception Ex)
        {
            id = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }

    public string ExecuteScalarString(String qry)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        string id = "";
        try
        {
            id = com.ExecuteScalar().ToString();
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }
    public SqlDataReader ExecuteReader(String qry)
    {
        SqlDataReader dtr = null;
        try
        {

            SqlCommand com = new SqlCommand(qry, Connect());
            dtr = com.ExecuteReader();
            //CloseConnection();

        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return dtr;
    }

    public SqlDataReader ReturnDataReader(string Query, SqlTransaction Trans, bool sql)
    {
        SqlDataReader Dtr = null;
        try
        {
            SqlCommand cmd = new SqlCommand(Query, Connect());
            cmd.Transaction = Trans;
            Dtr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd = null;

        }
        catch(Exception Ex)
        {
            Dtr = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return Dtr;
    }

    public DataSet ExecuteDataSet(String qry)
    {
        SqlDataAdapter adpt = new SqlDataAdapter(qry, Connect());
        DataSet dts = new DataSet();
        dts.Clear();
        try
        {

            adpt.Fill(dts);
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return dts;
    }

    public DataTable fillData(String str)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter adr = new SqlDataAdapter(str, Connect());
        try
        {
            adr.Fill(dt);
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return dt;

    }
    public DataTable ReturnDataTable(SqlCommand cmd)
    {
        DataTable Dt = new DataTable();
        try
        {
            Connect();
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            Da.Fill(Dt);
            cmd = null;
            Da = null;
            CloseConnection();
        }
        catch(Exception Ex)
        {
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return Dt;
    }
    public object FetchValue(SqlCommand cmd)
    {
        object x = null;
        try
        {
            Connect();
            x = cmd.ExecuteScalar();
            CloseConnection();
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return x;
    }

    public void Test()
    {

    }
}