using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/// <summary>
/// Summary description for Class1
/// </summary>
public class DynamicGridViewTextTemplate : ITemplate
{
    string _ColName;
    DataControlRowType _rowType;
    int _Count;
    public DynamicGridViewTextTemplate(string ColName, DataControlRowType RowType)
    {
        _ColName = ColName;
        _rowType = RowType;
    }
    public DynamicGridViewTextTemplate(DataControlRowType RowType, int ArticleCount)
    {
        _rowType = RowType;
        _Count = ArticleCount;
    }
    public void InstantiateIn(System.Web.UI.Control container)
    {
        switch (_rowType)
        {
            case DataControlRowType.Header:
                Literal lc = new Literal();
                lc.Text = "<b>" + _ColName + "</b>";
                container.Controls.Add(lc);
                break;
            case DataControlRowType.DataRow:
                Label lbl = new Label();
                lbl.DataBinding += new EventHandler(this.lbl_DataBind);
                container.Controls.Add(lbl);
                break;
            case DataControlRowType.Footer:
                Literal flc = new Literal();
                flc.Text = "<b>Total No of Articles:" + _Count + "</b>";
                container.Controls.Add(flc);
                break;
            default:
                break;
        }
    }
    private void lbl_DataBind(Object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;
        lbl.Text = DataBinder.Eval(row.DataItem, _ColName).ToString();
    }
}


public class DynamicGridViewURLTemplate : ITemplate
{
    string _ColNameText;
    string _ColNameURL;
    DataControlRowType _rowType;
    public DynamicGridViewURLTemplate(string ColNameText, string ColNameURL, DataControlRowType RowType)
    {
        _ColNameText = ColNameText;
        _rowType = RowType;
        _ColNameURL = ColNameURL;
    }
    public void InstantiateIn(System.Web.UI.Control container)
    {
        switch (_rowType)
        {
            case DataControlRowType.Header:
                Literal lc = new Literal();
                lc.Text = "<b>" + _ColNameURL + "</b>";
                container.Controls.Add(lc);
                break;
            case DataControlRowType.DataRow:
                HyperLink hpl = new HyperLink();
                hpl.Target = "_blank";
                hpl.DataBinding += new EventHandler(this.hpl_DataBind);
                container.Controls.Add(hpl);
                break;
            default:
                break;
        }
    }
    private void hpl_DataBind(Object sender, EventArgs e)
    {
        HyperLink hpl = (HyperLink)sender;
        GridViewRow row = (GridViewRow)hpl.NamingContainer;
        hpl.NavigateUrl = DataBinder.Eval(row.DataItem, _ColNameURL).ToString();
        hpl.Text = "<div class=\"Post\"><div class=\"PostTitle\">" + DataBinder.Eval(row.DataItem, _ColNameText).ToString() + "</div></div>";
    }
}