using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class AddGridViewTemplate : ITemplate
{
    private ListItemType templateType;
    private string columnName;

    public AddGridViewTemplate(ListItemType type, string colName)
    {
        templateType = type;
        columnName = colName;
    }

    public void InstantiateIn(Control container)
    {
        if (templateType == ListItemType.Item)
        {
            Label lbl = new Label();
            lbl.ID = columnName; // Set the ID for finding control later
            lbl.DataBinding += (sender, e) =>
            {
                // Perform data binding here
                Label lblBinding = (Label)sender;
                GridViewRow row = (GridViewRow)lblBinding.NamingContainer;
                lblBinding.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
            };
            container.Controls.Add(lbl);
        }
    }
}


//public class AddGridViewTemplate : ITemplate
//{
//    ListItemType templateType;
//    string headerText; 

//    public AddGridViewTemplate(ListItemType type, string text)
//    {
//        templateType = type;
//        headerText = text; 
//    }

//    public void InstantiateIn(Control container)
//    {
//        if (templateType == ListItemType.Header)
//        {
//            Label lblHeader = new Label
//            {
//                Text = headerText, 
//                Font = { Bold = true } 
//            };
//            container.Controls.Add(lblHeader);
//        }
//        else if (templateType == ListItemType.Item)
//        {
//            // Create an item label
//            Label lblItem = new Label();
//            lblItem.DataBinding += new EventHandler(DataBindLabel);
//            container.Controls.Add(lblItem);
//        }
//    }

//    private void DataBindLabel(object sender, EventArgs e)
//    {
//        Label lbl = (Label)sender;
//        GridViewRow row = (GridViewRow)lbl.NamingContainer;
//        lbl.Text = DataBinder.Eval(row.DataItem, headerText).ToString(); 
//    }
//}