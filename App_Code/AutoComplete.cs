using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Script.Services;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{

    public AutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    public class Student
    {
        public string ID { get; set; }
        public string StudentName { get; set; }

        public Student()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    [WebMethod]
    public List<Student> getStudent(string input)
    {
        clsData objData = new clsData();
        List<Student> name = new List<Student>();

        String query = "select StudentId as Id,StudentLname+ ','+StudentFname as Name from Student WHERE ActiveInd = 'A' order by StudentLname";
        SqlDataReader drStudent = objData.ReturnDataReader(query, false);
        try
        {
            while (drStudent.Read())
            {
                name.Add(new Student() { ID = drStudent["Id"].ToString(), StudentName = drStudent["Name"].ToString() });
            }


            drStudent.Close();
        }
        catch (Exception exp)
        {
            drStudent.Close();
        }
        if (input == "*")
        {
            var fetchStudentName = from m in name select m;
            return fetchStudentName.ToList();
        }
        else
        {
            var fetchStudentName = from m in name where m.StudentName.StartsWith(input, StringComparison.CurrentCultureIgnoreCase) select m;
            return fetchStudentName.ToList();
        }

    }

}
