using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


/// <summary>
/// Summary description for cls_data
/// </summary>
public class cls_data : cls_connection
{


    public SqlParameter @MediaId = new SqlParameter();
    public SqlParameter @Name = new SqlParameter();
    public SqlParameter @Description = new SqlParameter();
    public SqlParameter @path = new SqlParameter();
    public SqlParameter @type = new SqlParameter();






    /// <summary>
    /// Function to save media elements
    /// </summary>
    /// <param name="qry">User Query</param>
    /// <param name="argument">List of arguements</param>
    /// <returns>int value</returns>
    public int saveMediaElements(String qry, String[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        int id = 0;
        try
        {
            cmd.Parameters.Add("@name", SqlDbType.VarChar);
            cmd.Parameters["@name"].Value = (argument[0]);

            cmd.Parameters.Add("@description", SqlDbType.VarChar);
            cmd.Parameters["@description"].Value = (argument[1]);

            cmd.Parameters.Add("@path", SqlDbType.VarChar);
            cmd.Parameters["@path"].Value = argument[2];

            cmd.Parameters.Add("@type", SqlDbType.VarChar);
            cmd.Parameters["@type"].Value = argument[3];

            cmd.Parameters.Add("@mediaId", SqlDbType.Int);
            cmd.Parameters["@mediaId"].Value = Convert.ToInt32(argument[4]);

            cmd.Parameters.Add("@keyword", SqlDbType.VarChar);
            cmd.Parameters["@keyword"].Value = (argument[5]);

            cmd.Parameters.Add("@thumbnail", SqlDbType.VarChar);
            cmd.Parameters["@thumbnail"].Value = (argument[6]);


            id = Convert.ToInt32(cmd.ExecuteScalar());
            CloseConnection();
        }
        catch(Exception Ex)
        {
            CloseConnection();
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        return id;
    }

    public DataTable getMediaItems(string qry, string[] argument)
    {
        SqlCommand cmd = new SqlCommand(qry, Connect());
        DataTable dt = new DataTable();
        try
        {
            cmd.Parameters.Add("@name", SqlDbType.VarChar);
            cmd.Parameters["@name"].Value = (argument[0]);

            cmd.Parameters.Add("@description", SqlDbType.VarChar);
            cmd.Parameters["@description"].Value = (argument[1]);

            cmd.Parameters.Add("@path", SqlDbType.VarChar);
            cmd.Parameters["@path"].Value = argument[2];

            cmd.Parameters.Add("@type", SqlDbType.VarChar);
            cmd.Parameters["@type"].Value = argument[3];

            cmd.Parameters.Add("@mediaId", SqlDbType.Int);
            cmd.Parameters["@mediaId"].Value = Convert.ToInt32(argument[4]);


            cmd.Parameters.Add("@keyword", SqlDbType.VarChar);
            cmd.Parameters["@keyword"].Value = (argument[5]);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            CloseConnection();
        }
        catch(Exception Ex)
        {
            CloseConnection();
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }

        return dt;
    }

    public int ExecuteScalar_sp(String qry, string lessonName, string lpDesc, string lpKeyword, int lpDomain, string lpCategory, string isDiscreate, int setNmbr, int stepNumbr)
    {
        SqlCommand com = new SqlCommand(qry, Connect());
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@lessonName", lessonName);
        com.Parameters.AddWithValue("@description", lpDesc);
        com.Parameters.AddWithValue("@keyword", lpKeyword);
        com.Parameters.AddWithValue("@domain", lpDomain);
        com.Parameters.AddWithValue("@category", lpCategory);
        com.Parameters.AddWithValue("@discreate", isDiscreate);
        com.Parameters.AddWithValue("@setNumber", setNmbr);
        com.Parameters.AddWithValue("@stepNumber", stepNumbr);

        //com.Parameters.AddWithValue("@goalId", igoalId);
        //com.Parameters.AddWithValue("@createdBy", icreatedBy);
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


    public string GetArrayInRandomOrder(string[] arrayData)
    {
        string[] returnArrayData = arrayData;
        string arrayTime = "";
        string arrayStatus = "";
        string times = "";
        string coff = "";
        int i = 0;
        string[] arCommon = new string[returnArrayData.Length];
        string[] arTime = new string[returnArrayData.Length];
        string[] arCoff = new string[returnArrayData.Length];
        foreach (string time in returnArrayData)
        {

            arCommon = time.Split('^');
            arrayTime = arCommon[0].ToString().Replace(",", ".");
            arrayStatus = arCommon[1].ToString();
            times += arrayTime + ",";
            coff += arrayStatus + ",";
            arCoff[i] = arrayStatus;
            arTime[i] = arrayTime;
            i++;
        }
        string contentData = "";
        int[] ar = new int[returnArrayData.Length];
        for (i = 0; i < returnArrayData.Length; i++)
        {
            ar[i] = i;
        }

        int[] returnArray = new int[1000];
        returnArray = shuffleArray(ar);
        foreach (int t in returnArray)
        {
            contentData += arTime[t] + "^" + arCoff[t] + "@";
        }


        return contentData;

    }
    private static int[] shuffleArray(int[] numArray)
    {
        for (int index = 0; index < numArray.Length; index++)
        {
            Random rand = new Random();
            //create a random number between 0-numArray.length
            int randomNum = rand.Next(0, numArray.Length);
            //save temp variable
            int temp = numArray[index];
            //swap first index with random index
            numArray[index] = numArray[randomNum];
            //assign random index to first index
            numArray[randomNum] = temp;
            Console.WriteLine(numArray[index]);
        }
        return numArray;
    }
}