using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
//using System.Web.UI.WebControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class EditContractApprovalLimit : System.Web.UI.Page
    {
        private ContractApprovalLimitInfo contractApprovalLimit = null;

        #region Private Methods
        private void FormToObjects()
        {
           
        }



        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            TitleBar.Title = "Edit Contract Approval Limit";
            


            if (!IsPostBack) {
                try
                {
                    Security.CheckAccess(Security.userActions.EditContractApprovalLimit);
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
            }



        }

        

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {  try
            {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            contractApprovalLimit = new ContractApprovalLimitInfo();
            contractApprovalLimit.Id =int.Parse(row.Cells[1].Text.ToString());
            contractApprovalLimit.Min = int.Parse(((TextBox)(GridView1.Rows[e.RowIndex].FindControl("TxtMin"))).Text);
            contractApprovalLimit.Max = int.Parse(((TextBox)(GridView1.Rows[e.RowIndex].FindControl("TxtMAX"))).Text);

            
                if (Page.IsValid)
                {
                    ContractsController.GetInstance().UpdateContractApprovalLimit(contractApprovalLimit);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/EditContractApprovalLimit.aspx");




        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Contract Amount";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Win";
                HeaderCell.ColumnSpan = 2;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Loss";
                HeaderCell.ColumnSpan = 3;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            }
        }
    }
}