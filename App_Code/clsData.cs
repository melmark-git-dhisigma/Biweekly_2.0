﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsData
/// </summary>
public class clsData
{
    public clsData()
    {

    }


    private string mConectionString = "";
    SqlCommand cmd = null;
    SqlDataAdapter DAdap = null;

    public static bool blnTrans = true;

    public void Reset()
    {
        cmd = null;
        DAdap = null;
    }

    //public byte[] FetchPhoto(string SQL)
    //{
    //    byte[] content;
    //    using (cmd = new SqlCommand())
    //    {
    //        SqlConnection con = Open();
    //        if (blnTrans) cmd.Transaction = Trans;
    //        cmd.CommandText = SQL;
    //        cmd.Connection = con;
    //        content = (byte[])cmd.ExecuteScalar();
    //    }
    //    return content;
    //}

    public object ExecuteSp()
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            // if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandTimeout = 350;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SessionScore_Calculation";

            object content = cmd.ExecuteScalar();
            Close(con);
            return content;
        }
    }

    public object ExecuteSetSp()
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            // if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandTimeout = 200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SessionScore_Calculation_Set";

            object content = cmd.ExecuteScalar();
            Close(con);
            return content;
        }
    }

    public DataTable ExecuteStudentsDetails(string ProcName,int SchoolId, int StudentId,string Type)
    {
        DataTable Dt = new DataTable();      
        SqlConnection con = Open();
        try
        {
                 
            
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand(ProcName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", StudentId);
            cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
            cmd.Parameters.AddWithValue("@Type", Type);
            da = new SqlDataAdapter(cmd);
            da.Fill(Dt);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e);
        }
        finally
        {
            Close(con);
        }
        return Dt;
    }

    public DataTable ExecuteCoversheetBehavior(DateTime start,DateTime enddate,int schoolid,int studentid)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cmd = new SqlCommand("Coversheet_Behavior", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", start);
            cmd.Parameters.AddWithValue("@ENDDate", enddate);
            cmd.Parameters.AddWithValue("@Studentid", studentid);
            cmd.Parameters.AddWithValue("@SchoolId", schoolid);
            
            da = new SqlDataAdapter(cmd);
            da.Fill(Dt);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e);
        }
        finally
        {
            Close(con);
        }
        return Dt;
    }
    public string ExecuteDateIntervalExist(string StartTime, string EndTime, string StartDate, string EndDate, int schoolId, int ClassId, int NumOfTime)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            //  if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DateIntervalExist";
            cmd.Parameters.AddWithValue("@StartTime", StartTime);
            cmd.Parameters.AddWithValue("@EndTime", EndTime);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@schoolId", schoolId);
            cmd.Parameters.AddWithValue("@ClassId", ClassId);
            cmd.Parameters.AddWithValue("@NumOfTime", NumOfTime);

            string content = cmd.ExecuteScalar().ToString();
            Close(con);
            return content;
        }
    }

    public int SaveLessonPlanData(string name, string contentType, byte[] Data)
    {
        int index = 0;
        using (cmd = new SqlCommand())
        {
            string query = "insert into tbl_Files values (@Name, @ContentType, @Data)";
            SqlConnection con = Open();
            cmd.Connection = con;
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@ContentType", contentType);
            cmd.Parameters.AddWithValue("@Data", Data);

            index = (int)cmd.ExecuteNonQuery();

            Close(con);
            return index;
        }
    }


    public int DownloadLpData(int DocId, out string fileName, out string contentType, out byte[] bytes)
    {
        int index = 0;
        //byte[] bytes = null;
        //string fileName = "";
        //string contentType = "";
        string selQuerry = "";

        using (cmd = new SqlCommand())
        {
            string querry = "select Name, Data, ContentType from tbl_Files where DocId=@DocId";

            SqlConnection con = Open();
            cmd.Connection = con;
            cmd.CommandText = querry;
            cmd.Parameters.AddWithValue("@DocId", DocId);

            using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                sdr.Read();
                bytes = (byte[])sdr["Data"];
                contentType = sdr["ContentType"].ToString();
                fileName = sdr["Name"].ToString();
            }

            Close(con);
        }

        return 1;
    }
   

    public object ExecuteIOAPercCalculation(int NormalTable, int IOATable)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            // if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NormalSessHdr", NormalTable);
            cmd.Parameters.AddWithValue("@IOASessHdr", IOATable);
            cmd.CommandText = "IOAPercentage_Calculation";

            object content = cmd.ExecuteScalar();
            Close(con);
            return content;
        }
    }

    #region Commented and added the same code in the ProgressSummaryReport.aspx.cs for moving to Production on 30-11-2018
    ///Execute SP for Progress Summary Report
    ///
    //    public DataTable Execute_PSR_Data(string LessonId, string LPStatus, string StartDate, string enddate, int StudentId)
    //{
    //    DataTable Dt = new DataTable();
    //    SqlConnection con = Open();
    //    try
    //    {
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        cmd = new SqlCommand("PSR_Data", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@StartDate", StartDate);
    //        cmd.Parameters.AddWithValue("@ENDDate", enddate);
    //        cmd.Parameters.AddWithValue("@Studentid", StudentId);
    //        cmd.Parameters.AddWithValue("@LessonPlanId", LessonId);
    //        cmd.Parameters.AddWithValue("@LPStatus", LPStatus);
    //        da = new SqlDataAdapter(cmd);
    //        da.Fill(Dt);
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine("Error: " + e);
    //    }
    //    finally
    //    {
    //        Close(con);
    //    }
    //    return Dt;
    //}
    #endregion

    // function to execute IOABehaviorPercentage_Calculation
    public object ExecuteIOAPercBehaviorCalc(string MeasurementId, string StudentId, int BehaviorIOAId, int NormalBehavId, string Status)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@MeasurmentId", MeasurementId);
            cmd.Parameters.AddWithValue("@StudentId", StudentId);
            cmd.Parameters.AddWithValue("@BehaviorIOAId", BehaviorIOAId);
            cmd.Parameters.AddWithValue("@NormalBehavId", NormalBehavId);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.CommandText = "IOABehaviorPercentage_Calculation";

            object content = cmd.ExecuteScalar();
            Close(con);
            return content;
        }
    }

    public void ExecutePhoto(byte[] content, int Id, bool val)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            //   if (blnTrans) cmd.Transaction = Trans;
            try
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                if (val == true)
                {
                    cmd.CommandText = "saveImage";
                }
                else
                {
                    cmd.CommandText = "updateImage";
                }
                cmd.Parameters.Add("@SMS_AdmReg_ID", SqlDbType.Int);
                cmd.Parameters.Add("@Photo", SqlDbType.Image);
                cmd.Parameters["@SMS_AdmReg_ID"].Value = Id;
                cmd.Parameters["@Photo"].Value = content;
                cmd.ExecuteNonQuery();
                Close(con);
            }
            catch (Exception ex)
            {
                Close(con);
                if (ex.Message.Contains("Cannot insert duplicate"))
                    throw new Exception("Duplicate");
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
            }
        }
    }

    public void Dispose()
    {
        mConectionString = "";
        cmd = null;
        DAdap = null;
        //  Trans = null;
        blnTrans = false;
    }

    public string ConnectionString
    {
        get
        {
            return mConectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();
        }
    }
    //Get One Value From Table........
    public object FetchValue(string SQL)
    {
        object x = null;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();

            cmd = null;
            using (cmd = new SqlCommand())
            {
                cmd.CommandText = SQL;
                cmd.Connection = con;

                x = cmd.ExecuteScalar();
            }
            Close(con);
        }
        catch (Exception exp)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + SQL + "\n" + exp.ToString());
        }

        return x;
    }
    public object FetchValueTrans(string SQL, SqlTransaction Transs, SqlConnection Con)
    {
        object x = null;

        try
        {
            cmd = null;
            using (cmd = new SqlCommand())
            {
                cmd.Transaction = Transs;
                cmd.CommandText = SQL;
                cmd.Connection = Con;

                x = cmd.ExecuteScalar();
            }

        }
        catch (Exception Ex)
        {
            Close(Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + SQL + "\n" + Ex.ToString());
        }
        return x;
    }

    public DataTable ReturnDataTableWithTransaction(string Query, SqlConnection con, SqlTransaction Trans, bool sql)
    {
        DataTable Dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Transaction = Trans;
            DAdap = new SqlDataAdapter(cmd);
            DAdap.Fill(Dt);
            cmd = null;

        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
            throw Ex;
        }
        return Dt;
    }


    public DataTable ReturnDataTableDropDown(string Query, bool sql)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {
          
            SqlCommand cmd = new SqlCommand(Query, con);
            DAdap = new SqlDataAdapter(cmd);
            DAdap.Fill(Dt);
            cmd = null;
            Close(con);

        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }
    public object GetCurrentAutoIncID()
    {
        object x = null;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (SqlCommand cmd = new SqlCommand("SELECT SCOPE_IDENTITY()", con))
            {
                //  if (blnTrans) cmd.Transaction = Trans;
                x = cmd.ExecuteScalar();
            }
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        return x;
    }

    public bool IFExists(string SQL)
    {
        bool returnvalue = false;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (cmd = new SqlCommand(SQL, con))
            {
                using (SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (rd != null) if (rd.Read()) returnvalue = true;
                    rd.Close();
                }
            }
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + SQL + "\n" + Ex.ToString());
        }
        return returnvalue;

    }



    public bool IFExistsWithTranss(string SQL, SqlTransaction Trans, SqlConnection con)
    {
        bool returnvalue = false;
        try
        {


            using (cmd = new SqlCommand(SQL, con))
            {
                cmd.Transaction = Trans;
                try
                {
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) returnvalue = true;
                        rd.Close();
                    }
                }
                catch (Exception ex)
                {
                    RollBackTransation(Trans, con);
                    Close(con);
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + SQL + "\n" + ex.ToString());
                }

                //  Close();

            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + SQL + "\n" + Ex.ToString());
        }

        return returnvalue;


    }

    public int Save(ref DataTable dtAdd)
    {
        int returnValue = 0;
        SqlCommand cmd = null;
        DataTable Dt = new DataTable();

        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (cmd = new SqlCommand("SELECT * FROM " + dtAdd.TableName + " WHERE 1=2", con))
            {
                //  if (blnTrans) cmd.Transaction = Trans;
                using (DAdap = new SqlDataAdapter(cmd))
                {
                    DAdap.Fill(Dt);
                    DAdap.FillSchema(Dt, SchemaType.Source);

                    DataRow Dr;

                    foreach (DataRow D in dtAdd.Rows)
                    {
                        Dr = Dt.NewRow();
                        Dr.ItemArray = D.ItemArray;
                        Dt.Rows.Add(Dr);
                    }

                    Dt.GetChanges();

                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(DAdap);
                    DAdap.InsertCommand = cmdBuilder.GetInsertCommand();
                    returnValue = DAdap.Update(Dt);
                }
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());

        }
        finally
        {
        }
        Close(con);
        return returnValue;

    }




    public int Update(ref DataTable dtUpd, DataColumn[] primaryKey, string WhereCondition)
    {
        int returnValue = 0;
        DataTable dt = new DataTable();
        SqlConnection con = Open();
        using (cmd = new SqlCommand("SELECT * FROM " + dtUpd.TableName + " WHERE " + WhereCondition, con))
        {
            //if (blnTrans) cmd.Transaction = Trans;
            using (DAdap = new SqlDataAdapter(cmd))
            {
                string pkey = "";
                foreach (DataColumn C in primaryKey)
                {
                    pkey += C.ColumnName + " ";
                }


                DAdap.Fill(dt);
                DAdap.FillSchema(dt, SchemaType.Source);
                DataRow Dr;

                foreach (DataRow D in dtUpd.Rows)
                {
                    Dr = dt.NewRow();
                    foreach (DataColumn C in dtUpd.Columns)
                    {
                        if (pkey.Contains(C.ColumnName)) continue;
                        Dr[C.ColumnName] = D[C.ColumnName];
                    }
                    dt.Rows.Add(Dr);
                }

                dt.GetChanges();
                dt.PrimaryKey = primaryKey;

                using (SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(DAdap))
                {
                    DAdap.UpdateCommand = cmdBuilder.GetUpdateCommand();
                }

                returnValue = DAdap.Update(dt);
            }
        }
        return returnValue;
    }


    public SqlConnection Open()
    {
        SqlConnection mCon = new SqlConnection(ConnectionString);

        try
        {

            mCon.Open();


        }
        catch (Exception eX)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + eX.ToString());
        }
        return mCon;
    }

    public SqlConnection Open(bool BeginTrans)
    {
        SqlTransaction Trans;
        SqlConnection con = Open();
        try
        {
            blnTrans = BeginTrans;
            if (BeginTrans) Trans = con.BeginTransaction();

        }
        catch (Exception eX)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + eX.ToString());
        }
        return con;
    }

    public void Close(SqlConnection con)
    {
        try
        {

            con.Close();
            con.ConnectionString = "";
            con = null;
            cmd = null;
            DAdap = null;


        }
        catch (Exception eX)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + eX.ToString());
        }
    }
    public void CommitTransation()
    {

        // if (blnTrans) Trans.Commit();
        blnTrans = false;

    }
    public void CommitTransation(SqlTransaction Transs, SqlConnection con)
    {
        if (Transs != null)
        {
            Transs.Commit();
            blnTrans = false;
            Close(con);
        }
    }
    public void RollBackTransation()
    {
        // Trans.Rollback();
        blnTrans = false;

    }
    public void RollBackTransation(SqlTransaction Transs, SqlConnection con)
    {
        try
        {
            Transs.Rollback();
            blnTrans = false;
            Close(con);
        }
        catch
        { 
        }

    }


    //Use this For Insertin ,Update and Deleting.......

    public int ExecuteWithScopeandConnection(string sql, SqlConnection con, SqlTransaction Transs)
    {
        int retval = 0;

        try
        {
            sql = sql + "\nSELECT SCOPE_IDENTITY()";
            using (cmd = new SqlCommand(sql, con))
            {
                cmd.Transaction = Transs;
                try
                {
                    cmd.Connection = con;
                    retval = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    RollBackTransation(Transs, con);
                    //Close();
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + ex.ToString());
                    throw ex;
                }

                //  Close();

            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + Ex.ToString());
            throw Ex;
        }
        return retval;
    }

    public int ExecuteWithScope(string sql)
    {
        int retval = 0;
        SqlConnection con = Open();
        try
        {

            sql = sql + "\nSELECT SCOPE_IDENTITY()";
            using (cmd = new SqlCommand(sql, con))
            {
                try
                {
                    cmd.Connection = con;
                    retval = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // RollBackTransation(Trans,con);
                    Close(con);
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + ex.ToString());
                }

                Close(con);

            }
        }
        catch (Exception Ex)
        {
            Close(con);
            retval = -1;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + Ex.ToString());
        }
        return retval;
    }
    public int ExecuteWithTrans(string sql, SqlConnection con, SqlTransaction Transs)
    {
        int retval = 0;

        using (cmd = new SqlCommand(sql, con))
        {
            cmd.Transaction = Transs;
            try
            {

                cmd.Connection = con;
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RollBackTransation(Transs, con);
                Close(con);
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + ex.ToString());
                return retval;
               
            }
            return retval;
        }
    }
    public int Execute(string sql)
    {
        int retval = 0;
        SqlConnection con = Open();
        using (cmd = new SqlCommand(sql, con))
        {
            // if (blnTrans) cmd.Transaction = Trans;
            try
            {
                cmd.Connection = con;
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Close(con);
                ClsErrorLog errlog = new ClsErrorLog();
                errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + sql + "\n" + ex.ToString());
                return retval;
                
            }
            Close(con);
            return retval;
        }
    }

    public string Execute2(string sql)
    {
        int retval = 0;
        SqlConnection con = Open();
        using (cmd = new SqlCommand(sql, con))
        {
            // if (blnTrans) cmd.Transaction = Trans;
            try
            {
                cmd.Connection = con;
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Close(con);
                throw ex;
            }
            Close(con);
            return "Success";
        }
    }

    public DataTable ReturnDataTable(string TableName)
    {
        DataTable Dt;
        SqlConnection con = Open();
        using (cmd = new SqlCommand("SELECT * FROM " + TableName, con))
        {
            // if (blnTrans) cmd.Transaction = Trans;
            using (DAdap = new SqlDataAdapter(cmd))
            {
                Dt = new DataTable();
                DAdap.Fill(Dt);
            }
        }
        return Dt;
    }

    public SqlDataReader ReturnDataReader(string Query, bool sql)
    {
        SqlConnection con = Open();
        SqlDataReader dr = null;
        if (cmd != null) cmd.Dispose();
        cmd = new SqlCommand(Query, con);

        if (dr != null) if (dr.Read()) dr.Close();
        //  if (blnTrans) cmd.Transaction = Trans;
        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        cmd = null;
        //   Close(con);
        return dr;
    }

    public DataTable ReturnDataTable(string Query, bool sql)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 120;
            Da.Fill(Dt);
            cmd = null;
            Da = null;
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }
    public DataTable ReturnDataTable(string Query, SqlConnection con, SqlTransaction Trans, bool sql)
    {
        DataTable Dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            Da.Fill(Dt);
            cmd = null;
            Da = null;
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }


    public DataSet ReturnDataSet(string Query,bool sql)
    {
        DataSet Dt = new DataSet();
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            Da.Fill(Dt);
            cmd = null;
            Da = null;
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }
    /* DataSet ds = new DataSet();
        string cmdstr = "select id,country from Country";
        SqlDataAdapter adp = new SqlDataAdapter(cmdstr, conn);
        adp.Fill(ds);*/


    public DataTable ReturnDataTable(string Query, bool sql, bool GetSchema)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            //    if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);

            Da.Fill(Dt);
            if (GetSchema)
            {
                try
                {
                    Da.FillSchema(Dt, SchemaType.Source);
                }
                catch (Exception e)
                {
                    if (e.Message == "")
                    {
                        ClsErrorLog errlog = new ClsErrorLog();
                        errlog.WriteToLog(e.ToString());
                    }
                    Dt = null;

                }
            }
            cmd = null;
            Da = null;
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null; ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }

    public DataTable ReturnDataTableForDropDown(string Query, bool sql)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            // if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);

            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "(Select)";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = "";
                    dr[1] = "(Select)";
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        return Dt;
    }

    public void ReturnDataTableForRadioList(string Query, RadioButtonList rdo)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //  if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    rdo.DataSource = Dt;
                    rdo.DataTextField = "Name";
                    rdo.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    rdo.DataSource = Dt;
                    rdo.DataValueField = "Id";
                    rdo.DataTextField = "Name";
                    rdo.DataBind();
                }

            }

        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }

    public void ReturnDropDown_LessonPlan(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            // if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();

                if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "----------All Lesson Plan----------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }

    public void ReturnDropDown(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //     if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "---------------Select--------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "---------------Select--------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }
    public void ReturnDropDownclinical(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //     if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "---------------Select--------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "---------------Select--------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }
            if (Dt.Rows.Count == 0)
            {
                if (Dt.Columns.Count == 2)
                {
                    DataRow dr = Dt.NewRow();
                    dr[0] = 0;
                    dr[1] = "---------------No Behaviors--------------";
                    Dt.Rows.InsertAt(dr, 0);
                }
            }
                         
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }


    public void ReturnDropDownDomain(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            //      if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "------------Select Domain--------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "------------Select Domain--------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }


    public void ReturnDropDownForStep(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //     if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "----------------------------------ALL----------------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "----------------------------------ALL----------------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch { Close(con); }
        Close(con);

    }


    public void ReturnDropDownForSortParentSet(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //     if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "----------------------------------SELECT----------------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "----------------------------------SELECT----------------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }



            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch { Close(con); }
        Close(con);

    }


    public void ReturnDropDownForCriteria(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //      if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "---------------Select Column--------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "---------------Select Column--------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }

            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch { Close(con); }
        Close(con);

    }



    public void ReturnDropDownForMeasureCriteria(string Query, DropDownList Lst)
    {
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            //if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            //Da.FillSchema(Dt, SchemaType.Source);
            cmd = null;
            Da = null;
            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    dr[0] = "---------------Select Measure--------------";
                }

                else if (Dt.Columns.Count == 2)
                {
                    dr[0] = 0;
                    dr[1] = "---------------Select Measure--------------";
                }
                Dt.Rows.InsertAt(dr, 0);
            }





            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch { Close(con); }
        Close(con);

    }

    public void ReturnCheckBoxList(string Query, CheckBoxList Lst)
    {
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            //   if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            cmd = null;
            Da = null;


            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }

        Close(con);

    }


    public void ReturnListBox(string Query, ListBox Lst)
    {
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            //    if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Da.Fill(Dt);
            cmd = null;
            Da = null;


            if (Dt.Rows.Count > 0)
            {
                DataRow dr = Dt.NewRow();
                if (Dt.Columns.Count == 1)
                {
                    Lst.DataSource = Dt;
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }

                else if (Dt.Columns.Count == 2)
                {
                    Lst.DataSource = Dt;
                    Lst.DataValueField = "Id";
                    Lst.DataTextField = "Name";
                    Lst.DataBind();
                }
                Dt.Rows.InsertAt(dr, 0);
            }

        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n Query: " + Query + "\n" + Ex.ToString());
        }
        Close(con);

    }


}
