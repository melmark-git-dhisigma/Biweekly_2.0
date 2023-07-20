<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testVisualList.aspx.cs" Inherits="Phase002_1_testVisualList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <table>
              <tr>
                  <td>
                      No: Of Sets: 
                  </td>
                  <td>
                      <asp:TextBox ID="txtSets" runat="server"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td>
                      No: Of Steps
                  </td>
                  <td>
                      <asp:TextBox ID="txtSteps" runat="server"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td>
                      Lesson ID:
                  </td>
                  <td>
                      <asp:TextBox ID="txtLessonId" runat="server"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td>
                      No: Of trials:
                  </td>
                  <td>
                      <asp:TextBox ID="txtNumofTrials" runat="server"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td colspan ="2" style ="text-align:center;">
                      <asp:Button ID="BtnSubmit" runat="server" Text="Submit" />
                  </td>
              </tr>
          </table>
    </div>
    </form>
</body>
</html>
