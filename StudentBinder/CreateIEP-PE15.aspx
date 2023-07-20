<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE15.aspx.cs" Inherits="StudentBinder_CreateIEP_PE15" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="CSS/StylePE15.css" rel="stylesheet" />


    <title></title>

    <style type="text/css">
        .chkLeft {
            float: left;
            margin: 3px 0 0;
            padding: 0;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="ContentAreaContainer">
                <div class="clear"></div>
                <table cellpadding="0" cellspacing="0" width="96%">
                    <tr>
                        <td id="tdMsg" runat="server" class="top righ"></td>
                    </tr>

                    <tr>
                        <td class="righ">INDIVIDUALISED EDUCATION PROGRAM(IEP)
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" class="righ">Individual's Name :
                            <asp:Label runat="server" ID="lblStudentName"></asp:Label></td>
                    </tr>
                </table>
				 <table border="0" cellpadding="0" cellspacing="0"> 
            <tr>
                 <td class="righ"><h2 class="simble">VIII. PENNDATA REPORTING</h2>
                    </td>
					</tr>
				</table>
                <h4> Educational Environment (Complete either Section A or B; Select only one Educational Environment)</h4>
                <p>To calculate the percentage of time inside the regular classroom, divide the number of hours the Individual spends inside the regular classroom by the total number of hours in the school day (including lunch, recess, study periods). The result is then multiplied by 100.</p>

                <p><strong>SECTION A: For Individuals Educated in Regular School Buildings with Nondisabled Peers – Indicate the percentage of time INSIDE the regular classroom for this Individual:</strong></p>

                <p>Time spent outside the regular classroom receiving services unrelated to the Individual’s disability (e.g., time receiving ESL services) should be considered time inside the regular classroom. Educational time spent in age-appropriate community-based settings that include individuals with and without disabilities, such as college campuses or vocational sites, should be counted as time spent inside the regular classroom.</p>
                <h3>Calculation for this Individual:</h3>

                <table border="0" cellpadding="0" cellspacing="0" class="container">
                    <tr>
                        <td class="top1" width="10%">Column 1</td>
                        <td class="top1" width="10%">Column 2</td>
                        <td class="top1" width="10%">Calculation	</td>
                        <td class="top1" width="10%">Indicate Percentage</td>
                        <td class="top1 righ" width="25%">Percentage Category</td>
                    </tr>
                    <tr>
                        <td class="top2">Total hours the Individual spends in the regular classroom per day</td>
                        <td class="top2">Total hours in a typical school day
(including lunch, recess & study periods)
                        </td>
                        <td class="top2">(Hours inside regular classroom ÷ hours in school day) x 100 = %
(Column 1 ÷ Column 2) x 100 = %
                        </td>
                        <td class="top2">Section A: The percentage of time Individual spends inside the regular classroom:</td>
                        <td class="top2">Using the calculation result – select the appropriate percentage category</td>
                    </tr>
                    <tr>
                        <td class="top2"></td>
                        <td class="top2"></td>
                        <td class="top2"></td>
                        <td class="top2">% of the day</td>
                        <td class="righ">
                            <asp:CheckBox ID="chkregularcls80" runat="server" CssClass="chkbx" /><label>INSIDE the Regular Classroom 80% or More of the Day</label>
                            <asp:CheckBox ID="chkregularcls79" runat="server" CssClass="chkbx" /><label>INSIDE the Regular Classroom 79-40% of the Day</label>
                            <asp:CheckBox ID="chkregularcls40" runat="server" CssClass="chkbx" />
                            <label>INSIDE the Regular Classroom Less Than 40% of the Day</label>

                        </td>
                    </tr>

                </table>
                <p><strong>SECTION B: This section required only for Individuals Educated OUTSIDE Regular School Buildings for more than 50% of the day – select and indicate the Name of School or Facility</strong> on the line corresponding with the appropriate selection: (If a Individual spends less than 50% of the day in one of these locations, the IEP team must do the calculation in Section A)</p>

                <table border="0" cellpadding="0" cellspacing="0" class="inr">
                    <tr>
                        <td class="top" width="15%">
                            <asp:CheckBox ID="chkApproveprivateschool" runat="server" CssClass="chkbx" /><label>Approved Private School (Non Residential)</label></td>
                        <td class="top" width="15%">
                            <asp:TextBox ID="txtApprovePrivate" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                        <td class="top" width="15%">
                            <asp:CheckBox ID="chkotherpublic" runat="server" CssClass="chkbx" /><label>Other Public Facility (Non Residential)</label></td>
                        <td class="top righ" width="15%">
                            <asp:TextBox ID="txtotherpublic" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="top2">
                            <asp:CheckBox ID="chkapproveresidential" runat="server" CssClass="chkbx" /><label>Approved Private School (Residential)</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtapproveresidential" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                        <td class="top2">
                            <asp:CheckBox ID="chkhospital" runat="server" CssClass="chkbx" /><label>Hospital/Homebound</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txthospital" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="top2">
                            <asp:CheckBox ID="chkprivatefacility" runat="server" CssClass="chkbx" /><label>Other Private Facility (Non Residential)</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtprivatefacility" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                        <td class="top2">
                            <asp:CheckBox ID="chkcorrectionalfacility" runat="server" CssClass="chkbx" /><label>Correctional Facility</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtcorrectfacility" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="top2">
                            <asp:CheckBox ID="chkprivateresidential" runat="server" CssClass="chkbx" /><label>Other Private Facility (Residential)</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtprivateresidential" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                        <td class="top2">
                            <asp:CheckBox ID="chkoutofstate" runat="server" CssClass="chkbx" /><label>Out of State Facility</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtoutofstate" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="top2">
                            <asp:CheckBox ID="chkpublicfacility" runat="server" CssClass="chkbx" /><label>Other Public Facility (Residential)</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtpublicfacility" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                        <td class="top2">
                            <asp:CheckBox ID="chkInstructionconducted" runat="server" CssClass="chkbx" /><label>Instruction Conducted in the Home</label></td>
                        <td class="top2">
                            <asp:TextBox ID="txtinstructionalconduct" runat="server" CssClass="textfield" MaxLength="100"></asp:TextBox></td>
                    </tr>
                </table>
                <p><strong>EXAMPLES </strong>for Section A: How to Calculate PennData – Educational Environment Percentages</p>
                <table border="0" cellpadding="0" cellspacing="0" class="container">
                    <tr>
                        <td class="top1" width="15%"></td>
                        <td class="top1" width="15%">Column 1</td>
                        <td class="top1" width="15%">Column 2</td>
                        <td class="top1" width="15%">Calculation	</td>
                        <td width="15%" class="top1 righ">Indicate Percentage</td>
                    </tr>
                    <tr>
                        <td class="top2"></td>
                        <td class="top2">Total hours the Individual spends in the regular classroom – per day</td>
                        <td class="top2" width="15%">Total hours in a typical school day (including lunch, recess & study periods)</td>
                        <td class="top2" width="15%">(Hours inside regular classroom ÷ hours in school day) x 100 = %
(Column 1 ÷ Column 2) x 100 = %
                        </td>
                        <td width="15%" class="righ btmm">Section A: The percentage of time Individual spends inside the regular classroom:</td>
                    </tr>
                    <tr>
                        <td class="top2">Example 1  </td>
                        <td class="top2">5.5</td>
                        <td class="top2" width="15%">6.5</td>
                        <td class="top2" width="15%">(5.5 ÷ 6.5) x 100 = 85%</td>
                        <td width="15%" class="righ btmm">85% of the day (Inside 80% or More of Day)</td>
                    </tr>
                    <tr>
                        <td class="top2">Example 2  </td>
                        <td class="top2">3</td>
                        <td class="top2" width="15%">5</td>
                        <td class="top2" width="15%">(3 ÷ 5) x 100 = 60%</td>
                        <td width="15%" class="righ btmm">60% of the day (Inside 79-40% of Day)</td>
                    </tr>

                    <tr>
                        <td class="top2 btmm">Example 3  </td>
                        <td class="top2 btmm">1</td>
                        <td class="top2 btmm" width="15%">5</td>
                        <td class="top2 btmm" width="15%">(1 ÷ 5) x 100 = 20%</td>
                        <td width="15%" class="righ btmm">20% of the day (Inside less than 40% of Day)</td>
                    </tr>


                </table>
                <p>For help in understanding this form, an annotated IEP is available on the PaTTAN website at www.pattan.net  Type “Annotated Forms” in the Search feature on the website. If you do not have access to the Internet, you can request the annotated form by calling PaTTAN at 800-441-3215.</p>
                <br />
                <table>
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                                OnClick="btnSave_Click" Text="Save and continue" />

                            <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                                OnClick="btnSave_hdn_Click" Text="dummy"  style="display:none;"/>--%>
                        </td>

                    </tr>
                </table>





                <div class="clear"></div>
            </div>

            <div class="clear"></div>

            <div class="footer">
                <img src="images/smllogo.JPG" width="109" height="18" />
            </div>


            <div class="clear"></div>
        </div>
    </form>
</body>
</html>
