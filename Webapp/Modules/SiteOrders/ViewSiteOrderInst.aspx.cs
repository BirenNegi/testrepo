using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SOS.Core;
using Microsoft.Reporting.WebForms;
using SOS.Reports;
using System.Configuration;
using System.Drawing;
using System.Globalization;

namespace SOS.Web
{
    public partial class ViewSiteOrderPageInst : SOSPage
    {

        #region Members
        
        private ProjectInfo ProjectInfo = null;
        private SiteOrderInfo SiteOrderInfo = null;
        //private SiteOrderDetailInfo SiteOrderDetailInfo = null; // DS20230320
        private PeopleInfo ForemanInfo = null;
        //private PeopleInfo ContactInfo = null; // DS20230320
        private List<SiteOrderApprovalsSearchInfo> siteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>(); // DS20230320
        //private List<TradeTemplateInfo> tradeTemplates = null; // DS20230822
        private ProjectTradesInfo projectTradesInfo = null;      // DS20230822
        private SubContractorInfo SubContractorInfo = null; // DS20230320
        private PeopleInfo GivenByInfo = null;  // DS20230308
        private bool isReadOnly = false; // DS20230629
        #endregion

        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;
            string parameterReadOnly = Request.Params["ReadOnly"];
            if (parameterReadOnly != null)
            {
                tempNode.ParentNode.Url = "~/Modules/SiteOrders/SelectSiteOrderSub.aspx";
                tempNode.ParentNode.ParentNode.Url = "~/Modules/SiteOrders/SelectSiteOrderSub.aspx";
                tempNode.ParentNode.ParentNode.ParentNode.Url = "~/Modules/Core/Home.aspx";
                return currentNode;
            }
            if (SiteOrderInfo == null)
                return null;

            tempNode.Title = SiteOrderInfo.Title;// + (siteOrderInfo.IsStatusProposal ? " (Proposal)" : ""); ;
            tempNode.ParentNode.Url += "?ProjectId=" + ProjectInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = ProjectInfo.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + ProjectInfo.IdStr;
            return currentNode;
        }

        private ProcessStepInfo CurrentStep(TradeInfo tradeInfo)
        {
            ProcessController processController = ProcessController.GetInstance();
            ProcessStepInfo processStepInfo = null;

            if (tradeInfo.Contract != null)
            {
                processStepInfo = processController.GetLastStep(tradeInfo.Contract.Process);
                if (processStepInfo == null)
                {
                    processStepInfo = processController.GetLastStep(tradeInfo.Process);
                }
            }
            else
            {
                processStepInfo = processController.GetLastStep(tradeInfo.Process);
            }

            return processStepInfo;
        }

        protected String InfoStatus(TradeInfo tradeInfo)
        {
            ProcessStepInfo processStepInfo = CurrentStep(tradeInfo);

            if (processStepInfo != null)
                return processStepInfo.Name;
            else
                return String.Empty;
        }

        protected String DateStatus(TradeInfo tradeInfo)
        {
            ProcessStepInfo processStepInfo = CurrentStep(tradeInfo);

            if (processStepInfo != null)
                return UI.Utils.SetFormDate(processStepInfo.ActualDate);
            else
                return String.Empty;
        }

        protected String LinkContract(TradeInfo tradeInfo)
        {
            if (tradeInfo.ContractApproved)
                return "~/Modules/Contracts/ShowContract.aspx?ContractId=" + tradeInfo.Contract.IdStr;
            else
                return null;
        }

        protected String StyleName(int? numDays)
        {
            if (numDays != null)
                if ((Int32)numDays > 0)
                    return "lstItemError";

            return String.Empty;
        }

        protected String SetIconPath(int? flag)
        {
            if (flag == null)
                return "~/Images/IconView.gif"; 
            else if (flag.Value == TradeInfo.FlagRed)
                return "~/Images/RedFlag.png";
            else if (flag.Value == TradeInfo.FlagGreen)
                return "~/Images/GreenFlag.png";
            else
                return "~/Images/IconView.gif";
        }

        private void BindSiteOrder()
        {
            //List<JobTypeInfo> jobTypes = ContractsController.GetInstance().GetJobTypes();
            ddlCodeType.Items.Add(new ListItem("Select Type", String.Empty));
            ddlCodeType.Items.Add(new ListItem("Bill of Quantities", "BOQ"));
            ddlCodeType.Items.Add(new ListItem("Client Variation", "CV"));
            ddlCodeType.Items.Add(new ListItem("Back Charge", "B/Ch"));
            ddlCodeType.Items.Add(new ListItem("Vaughan Variation", "V"));
            ddlCodeType.Items.Add(new ListItem("Design Variation", "DV"));
            ddlCodeType.Items.Add(new ListItem("Separate Account ", "A/Acc"));
            //if (projectInfo.IsStatusProposal)
            //{
            //    pnlStatusActive.Visible = false;
            //}

            txtTitle.Text = UI.Utils.SetFormString(SiteOrderInfo.Title);
            ddlCodeType.SelectedValue = SiteOrderInfo.OrderCode; 
            txtTyp_Desc.Text = UI.Utils.SetFormString(SiteOrderInfo.Typ);
            if (txtTyp_Desc.Text == "Ins") txtTyp_Desc.Text = "Site Instruction";
            txtOrderDateShow.Text = UI.Utils.SetFormDate(SiteOrderInfo.OrderDate);
            txtItemsTotal.Text = UI.Utils.SetFormEditDecimal(SiteOrderInfo.ItemsTotal);
            // DS20230306 txtDeliveryFee.Text = UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee);
            txtSubTotal.Text = UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal);
            // DS20230304 txtGST.Text = UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST);
            // DS20230304 txtTotal.Text = UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total);
            txtContact.Text = SiteOrderInfo.Contact;
            // DS20230304 txtAtt.Text = SiteOrderInfo.ContactPhone;
            txtEmail.Text = UI.Utils.SetFormString(SiteOrderInfo.Email);
            txtNotes.Text = UI.Utils.SetFormString(SiteOrderInfo.Notes);
            txtVariationId.Text = UI.Utils.SetFormEditInteger(SiteOrderInfo.VariationID);
            gvSiteOrderItems.DataSource = SiteOrderInfo.Items;
            gvSiteOrderItems.DataBind();
            gvSiteOrderDocs.DataSource = SiteOrderInfo.Docs;
            gvSiteOrderDocs.DataBind();
            gvSiteOrderApprovals.DataSource = siteOrderApprovalsSearchInfoList;
            gvSiteOrderApprovals.DataBind();
            gvSiteOrderApprovals.Columns[8].Visible = false;
            gvSiteOrderApprovals.Columns[9].Visible = false;
            gvSiteOrderApprovals.Columns[10].Visible = false;
            gvSiteOrderApprovals.Columns[11].Visible = false;
            if (SiteOrderInfo.SubContractorId == 0)
            {
             //                 txtSubContractorAddr.Text = UI.Utils.SetFormString(SiteOrderInfo.Street);
            }
            //foreach (TradeTemplateInfo TradeTemplate in tradeTemplates) //DS20230822 >>>
            //{
            //    ddTradesCode.Items.Add(new ListItem(TradeTemplate.TradeCode + " - " + TradeTemplate.TradeDescription, TradeTemplate.TradeCode));
            //}
            //txtTypeInfo.Text = UI.Utils.SetFormString(SiteOrderInfo.TypeInfo);
            if (projectTradesInfo != null)
            {
                ddTradesCode.Items.Add(new ListItem(projectTradesInfo.Code + " - " + projectTradesInfo.Name, projectTradesInfo.Code));   //DS20230822 <<<
            }
            txtTypeInfo.Text = UI.Utils.SetFormString(SiteOrderInfo.TypeInfo);
            txtComments.Text = UI.Utils.SetFormString(SiteOrderInfo.Comments); // DS20230320
        }
        #endregion

        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            string parameterReadOnly = Request.Params["ReadOnly"];
            if (parameterReadOnly != null || isReadOnly)   // DS20230629
            {
                cmdEditTop.Visible = false;
                btnAdd.Visible = false;
                btnApprovals.Visible = false;
                btnEditDoc.Visible = false;
                gvSiteOrderItems.Columns[0].Visible = false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            SiteOrdersController siteOrdersController = SiteOrdersController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            PeopleController PeopleController = PeopleController.GetInstance();
            SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
            int UserId = (int)Web.Utils.GetCurrentUserId();
            try
            {
                Security.CheckAccess(Security.userActions.ViewSiteOrder);

                String parameterProjectId = Utils.CheckParameter("ProjectId");
                ProjectInfo = projectsController.GetProjectWithTradesParticipations(Int32.Parse(parameterProjectId));
                //TitleBar1.Title = projectInfo.Name + " - Site Orders";
                Core.Utils.CheckNullObject(ProjectInfo, parameterProjectId, "Project");

                String parameterOrderId = Utils.CheckParameter("OrderId");
                SiteOrderInfo = siteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
                //tradeTemplates = TradesController.GetInstance().GetTradeTemplatesFromCode(SiteOrderInfo.TradesCode);  DS20230822
                projectTradesInfo = SiteOrdersController.GetInstance().GetProjectTrades(Int32.Parse(parameterProjectId), SiteOrderInfo.TradesCode);//DS20230822
                if (SiteOrderInfo.SubContractorId == 0)
                {
                    TitleBar1.Title = "View Site Instruction (No Subcontractor)  D" + Int32.Parse(parameterOrderId).ToString("000000"); // DS20230320
                }
                else
                { 
                TitleBar1.Title = "View Site Instruction  D" + Int32.Parse(parameterOrderId).ToString("000000"); // DS20230320
                }
 
                //SiteOrderDetailInfo SiteOrderDetailInfo
                // List<SiteOrderDetailInfo> SiteOrderDetailInfo = siteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterOrderId));
                SiteOrderInfo.Items = siteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterOrderId));
                SiteOrderInfo.Docs = siteOrdersController.GetSiteOrderDocs(Int32.Parse(parameterOrderId));
                //if (SiteOrderInfo.Docs.Count == 0) { btnShowDocs.Visible = false; } else { btnShowDocs.Visible = true; }
                siteOrderApprovalsSearchInfoList = siteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                //foreach (SiteOrderApprovalsSearchInfo SOA in siteOrderApprovalsSearchInfoList)   // DS20230629
                //{
                //    if (SOA.Process == "DOS" & SOA.Status == "A") { isReadOnly = true; }
                //}
                if (SiteOrderInfo.GivenByPeopleId != UserId)
                {
                    if (ProjectInfo.ProjectManager.Id != UserId) { isReadOnly = true; }
                }

                if (SiteOrderInfo.isOrderApproved == 1 || isReadOnly)     // DS20230629
                {
                    cmdEditTop.Visible = false;
                    //lnkEditItems.Visible = false;
                    btnAdd.Visible = false;
                    if (SiteOrderInfo.isOrderApproved == 1 || isReadOnly) { TitleBar1.Title = TitleBar1.Title + "  ## Approved ##"; }
                    gvSiteOrderItems.Columns[0].Visible = false;
                }
                else
                {
                    cmdEditTop.Visible = true;
                }
                //newsiteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterOrderId));
                //Core.Utils.CheckNullObject(siteOrderInfo, parameterOrderId, "SiteOrder");
                ForemanInfo = PeopleController.GetPersonById(SiteOrderInfo.ForemanID);
                SubContractorInfo = SubContractorsController.GetSubContractor(SiteOrderInfo.SubContractorId);
                // parameterOrderTyp = Request.Params["OrderTyp"];
                if (ForemanInfo != null)
                {
                    txtPeopleForeman.Text = ForemanInfo.Name;
                   // txtPeopleForemanPh.Text = ForemanInfo.Phone;
                }
                if (SubContractorInfo != null)
                {
                    txtABN.Text = SubContractorInfo.Abn;
                    txtSubContractorName.Text = SubContractorInfo.ShortName;
                    txtSubContractorAddr.Text = SubContractorInfo.Street + " " + SubContractorInfo.PostalCode + " " + SubContractorInfo.State;
                }
                 else
                {
                    txtABN.Text = SiteOrderInfo.ABN;
                    txtSubContractorName.Text = SiteOrderInfo.Name;
                    txtSubContractorAddr.Text = SiteOrderInfo.Street + ", " + SiteOrderInfo.Locality + ", " + SiteOrderInfo.PostalCode + ", " + SiteOrderInfo.State;

                }

                GivenByInfo = PeopleController.GetPersonById(SiteOrderInfo.GivenByPeopleId);
                if (GivenByInfo != null)
                {
                    txtGivenByName.Text = GivenByInfo.Name;
                }
                if (!Page.IsPostBack)
                {
                   
                    if (Security.ViewAccess(Security.userActions.ViewSiteOrder))
                    {
                        if (processController.AllowEditCurrentUser(SiteOrderInfo))
                        {
                          //  cmdEditTop.Visible = true;
                           // cmdDeleteTop.Visible = true;

                            //if (siteOrderInfo.Items.Count > 0)
                            //    cmdDeleteTop.Enabled = false;
                            //else
                            //    cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Project ?');");
                        }
                    }

                    BindSiteOrder();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        protected void cmdShowDoc_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;
                //Get the value passed from gridview
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                string DocId = args[0];
                string DocName = args[1];
                string parameterOrderId = Request.Params["OrderId"];
                string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];
                string result = SOS.UI.Utils.showSiteOrderDoc(this, parameterProjectId, parameterOrderId, DocId, DocName);
            }

        }
        protected void cmdShowDocs_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;
                //Get the value passed from gridview
                //string arguments = lnkbtn.CommandArgument.ToString();
                //string[] args = arguments.Split(';');
                //string DocId = args[0];
                //string DocName = args[1];
                string parameterOrderId = Request.Params["OrderId"];
                //string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];

                //string result = SOS.UI.Utils.showSiteOrderDoc(this, parameterProjectId, parameterOrderId, DocId, DocName);
                //if (SiteOrderInfo.Docs != null)
                //{
                    string result = SOS.UI.Utils.showSiteOrderDocs(this, parameterProjectId, parameterOrderId, SiteOrderInfo.Docs);
                //}
            }

        }
        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            if (SiteOrderInfo.SubContractorId == 0)
            {
                Response.Redirect($"~/Modules/SiteOrders/EditSiteOrderInstNS.aspx?ProjectId={ProjectInfo.IdStr}&OrderId={SiteOrderInfo.IdStr}&OrderTyp={SiteOrderInfo.Typ}");
            }
            else
            {
                Response.Redirect($"~/Modules/SiteOrders/EditSiteOrderInst.aspx?ProjectId={ProjectInfo.IdStr}&OrderId={SiteOrderInfo.IdStr}&OrderTyp={SiteOrderInfo.Typ}");
            }
        }
        protected void cmdEditItems_Click(object sender, EventArgs e)
        {
            if (gvSiteOrderItems.Rows.Count == 0)
            {
                Response.Redirect($"~/Modules/SiteOrders/EditSiteOrderDetailInst.aspx?ProjectId={ProjectInfo.IdStr}&OrderId={SiteOrderInfo.IdStr}&OrderTyp={SiteOrderInfo.Typ}");
            }
            else
            {
                Response.Redirect($"~/Modules/SiteOrders/SearchSiteOrderDetailInst.aspx?ProjectId={ProjectInfo.IdStr}&OrderId={SiteOrderInfo.IdStr}&OrderTyp={SiteOrderInfo.Typ}");
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SiteOrdersController.GetInstance().DeleteSiteOrder(SiteOrderInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/SearchProjects.aspx");
        }
        //cmdShowDoc_Click
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            string SiteOrderNav = Session["SiteOrderNav"] as string;
            if (SiteOrderNav == "")
            {
                String parameterProjectId = Request.Params["ProjectId"];
                Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);
            }
            else
            {
                String parameterProjectId = Request.Params["ProjectId"];
                Response.Redirect("~/Modules/SiteOrders/" + SiteOrderNav);
            }
        }
        protected void cmdAddNew_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            String parameterProjectId = Request.Params["ProjectId"];
            String parameterOrderId = Request.Params["OrderId"];
            String parameterOrderTyp = Request.Params["OrderTyp"];
            //Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?ProjectId=" + parameterProjectId);
            Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetailInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdEditDoc_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            String parameterProjectId = Request.Params["ProjectId"];
            String parameterOrderId = Request.Params["OrderId"];
            String parameterOrderTyp = Request.Params["OrderTyp"];
            //Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?ProjectId=" + parameterProjectId);
            Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDocs.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdPrintOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Modules/SiteOrders/ShowSiteOrderPage.aspx?OrderId={SiteOrderInfo.IdStr}");
        }
        protected void cmdEmailOrder_Click(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();

            //Response.Redirect($"~/Modules/SiteOrders/ShowSiteOrderPage.aspx?OrderId={SiteOrderInfo.IdStr}");
            string SendAddress = "";  //DS20230329
            string Notes = SiteOrderInfo.Notes.Replace("\r\n", "\n");
            bool FoundDetail = false;
            string[] SNotes = (SiteOrderInfo.Notes).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);   //DS20230309
            for (int i = 0; i < SNotes.Length; i++)
            {
                if (SNotes[i] != "") { FoundDetail = true; }
            }
            if (FoundDetail == false)
            {
                if (SiteOrderInfo.Items.Count == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Order does not have any details";  //DS20230329
                    return;
                }
            }
            try
            {
                SendAddress = SiteOrdersController.SendSubmisionNotification(SiteOrderInfo.IdStr, (int)Web.Utils.GetCurrentUserId());  //DS20230329
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
            lblMessage.Visible = true;
            lblMessage.Text = "E-Mail Sent to " + SendAddress;  //DS20230329
            int ApprovedCount = SiteOrdersController.ApproveDraftOrder(SiteOrderInfo);
            if (ApprovedCount != 0)
            {
                lblMessage.Text += " - Status updated for Order Sent ";
                siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals((int)SiteOrderInfo.Id);
                gvSiteOrderApprovals.DataSource = siteOrderApprovalsSearchInfoList;
                gvSiteOrderApprovals.DataBind();
            }
            SiteOrderInfo.Docs = SiteOrdersController.GetSiteOrderDocs((int)SiteOrderInfo.Id);  //DS20230907
            gvSiteOrderDocs.DataSource = SiteOrderInfo.Docs;
            gvSiteOrderDocs.DataBind();        //DS20230907
        }
        protected void cmdEditApproval_Click(object sender, EventArgs e)
        {
            String parameterProjectId = Request.Params["ProjectId"];
            String parameterOrderId = Request.Params["OrderId"];
            String parameterOrderTyp = Request.Params["OrderTyp"];
            //Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?ProjectId=" + parameterProjectId);
            Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovals.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            //   else
        }

        #endregion
        protected void gvSiteOrderApprovals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {

                    //int index = GetColumnIndexByName(e.Row, "myDataField");
                    string tx = e.Row.Cells[5].Text;
                    string isApprovalCurrent = e.Row.Cells[8].Text;
                    int AssignedPeopleId = int.Parse(e.Row.Cells[10].Text);
                    ((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                    ((CheckBox)e.Row.FindControl("Reject")).Enabled = false;
                    if (tx != null)
                    {
                        if (tx == "Approved") ((CheckBox)e.Row.FindControl("Approve")).Checked = true;
                        if (tx == "Rejected") ((CheckBox)e.Row.FindControl("Reject")).Checked = true;

                    }
                    if (isApprovalCurrent != null)
                    {
                        if (isApprovalCurrent == "1")
                        {
                            int UserId = (int)Web.Utils.GetCurrentUserId();
                            if (UserId == AssignedPeopleId)
                            {
                                //((CheckBox)e.Row.FindControl("Approve")).Enabled = true;
                                //((CheckBox)e.Row.FindControl("Reject")).Enabled = true;
                            }
                            else
                            {
                                //((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                                //((CheckBox)e.Row.FindControl("Reject")).Enabled = false;
                            }
                            GridViewRow row = e.Row;
                            row.BackColor = Color.LightSeaGreen;
                        }
                        //if (isApprovalCurrent == "0") ((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                        //if (isApprovalCurrent == "0") ((CheckBox)e.Row.FindControl("Reject")).Enabled = false;

                    }

                }
                catch
                {

                }


            }
        }
    }
}

