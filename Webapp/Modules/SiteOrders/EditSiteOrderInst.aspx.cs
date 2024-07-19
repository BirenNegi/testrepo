using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;
using System.Linq;
namespace SOS.Web
{
 
   public partial class EditSiteOrderInst : SOSPage  //System.Web.UI.Page
    {
        private ProjectInfo ProjectInfo = null;
        private SiteOrderInfo SOI = null;
        private PeopleInfo ForemanInfo = null;
        private PeopleInfo ContactInfo = null;
        private PeopleInfo GivenByInfo = null;  // DS20230320
        private SubContractorInfo SubContractorInfo = null;
        private ClientVariationInfo ClientVariationInfo = null;
        private String parameterProjectId;
        private String parameterOrderTyp;
        private String parameterOrderId;
        //private List<TradeTemplateInfo> tradeTemplates = null; // DS20230320
        private List<ProjectTradesInfo> ProjectTradesInfoList = null; // DS20230822
        #region Public properties

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            parameterProjectId = Request.Params["ProjectId"];
            if (parameterProjectId == null)
                return null;

            if (parameterOrderId == null)
            {
                tempNode.ParentNode.Url = "~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp;
            }
            else
            {
                tempNode.ParentNode.Url += "?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp;
            }
            tempNode.ParentNode.ParentNode.Title = ProjectInfo.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp;

            return currentNode;
        }
        protected String GvSiteOrdersSortExpression
        {
            get { return (String)ViewState["GvSiteOrdersSortExpression"]; }
            set { ViewState["GvSiteOrdersSortExpression"] = value; }
        }

        protected SortDirection GvSiteOrdersSortDireccion
        {
            get { return (SortDirection)ViewState["GvSiteOrdersSortDirection"]; }
            set { ViewState["GvSiteOrdersSortDirection"] = value; }
        }
#endregion

#region Private Methods
        private void BindSearch()
        {
            ddlCodeType.Items.Add(new ListItem("Select Type", String.Empty));
            ddlCodeType.Items.Add(new ListItem("Bill of Quantities","BOQ" ));
            ddlCodeType.Items.Add(new ListItem("Client Variation", "CV"));
            ddlCodeType.Items.Add(new ListItem("Back Charge", "B/Ch"));
            ddlCodeType.Items.Add(new ListItem("Vaughan Variation", "V" ));
            ddlCodeType.Items.Add(new ListItem("Design Variation", "DV" ));
            ddlCodeType.Items.Add(new ListItem("Separate Account ", "A/Acc" ));

 
            GvSiteOrdersSortExpression = "Name";
            GvSiteOrdersSortDireccion = SortDirection.Ascending;

            ddTradesCode.Items.Add(new ListItem("Select Budget Code", ""));
            //List<TradeTemplateInfo> tradeTemplateSort = tradeTemplates.OrderBy(o => o.TradeDescription).ToList();    DS20230822 >>>
            //foreach (TradeTemplateInfo TradeTemplate in tradeTemplateSort)
            //{
            //    //Console.WriteLine(TradeTemplate.TradeCode + " " + TradeTemplate.TradeDescription);
            //    if (TradeTemplate.TradeDescription != "" & TradeTemplate.TradeDescription != null)
            //    {
            //        ddTradesCode.Items.Add(new ListItem(TradeTemplate.TradeCode + " - " + TradeTemplate.TradeDescription, TradeTemplate.TradeCode));
            //    }
            //}
            foreach (ProjectTradesInfo ProjectTradesInfo in ProjectTradesInfoList)
            {
                ddTradesCode.Items.Add(new ListItem(ProjectTradesInfo.Code + " - " + ProjectTradesInfo.Name, ProjectTradesInfo.Code));
            } // DS20230822 <<<
        }
        #endregion

        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            // tradeTemplates = TradesController.GetInstance().GetTradeTemplates();  //DS20230822 
            parameterOrderId = Request.Params["OrderId"];  // DS20230822
            parameterProjectId = Request.Params["ProjectId"];
            ProjectTradesInfoList = SiteOrdersController.GetProjectTradesAll(Int32.Parse(parameterProjectId)); //DS20230822
            ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
            cmdSelSubContractor.NavigateUrl = Utils.PopupCompanySub(this, txtSubContractorId.ClientID, txtSubContractorName.ClientID,ProjectInfo.BusinessUnitIdStr, parameterProjectId);
            parameterOrderTyp = Request.Params["OrderTyp"];
 
            if (parameterOrderId == null)
            {
                SOI = SiteOrdersController.InitializeSiteOrder(Int32.Parse(parameterProjectId), 0, (int)ProjectInfo.Foreman.Id);
                // DS20230304 SOI.GSTRate = UI.Utils.GetFormDecimal("0.10");
                TitleBar.Title = "Adding Site Instruction";
                cmdUpdateTop.Text = "Save & Next";
                cmdUpdateBottom.Text = "Save & Next";

            }
            else
            {
                SOI = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
                TitleBar.Title = "Updating Site Instruction  D" + Int32.Parse(parameterOrderId).ToString("000000");
                cmdUpdateTop.Text = "Save & Exit";
                cmdUpdateBottom.Text = "Save & Exit";
            }
            if (Page.IsPostBack)
            {
            }
            else
            {
                //
                // Lookup
                //
                BindSearch();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            PeopleController PeopleController = PeopleController.GetInstance();
            SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            try
            {
                Security.CheckAccess(Security.userActions.SearchSiteOrders);

                if (!Page.IsPostBack)

                {
                    if (txtSubContractorId.Value == "") txtSubContractorId.Value = "0";
                    txtTyp_Desc.Text = "Site Instruction";
                    ObjectsToForm();
                }
                else
                {
                    cmdSelSubContractor.NavigateUrl = Utils.PopupCompany(this, txtSubContractorId.ClientID, txtSubContractorName.ClientID, null);
                    cmdSelContact.NavigateUrl = Utils.PopupPeopleSub(this, txtContactId.ClientID, txtContact.ClientID, "CL", txtSubContractorId.Value);
                    txtTyp_Desc.Text = "Site Instruction";
                    FormToObjects();
                }

                ContactInfo = PeopleController.GetPersonById(SOI.ContactPeopleId);
                ForemanInfo = PeopleController.GetPersonById(SOI.ForemanID);
                SubContractorInfo = SubContractorsController.GetSubContractor(SOI.SubContractorId);
                GivenByInfo = PeopleController.GetPersonById(SOI.GivenByPeopleId);
                if (GivenByInfo != null)
                {
                    txtGivenByName.Text = GivenByInfo.Name;
                }

                if (ContactInfo != null)
                {
                    txtContact.Text = ContactInfo.Name;
                    if (txtEmail.Text == "") { txtEmail.Text = ContactInfo.Email; }

                    txtSubContractorAddr.Text = ContactInfo.Street + "," + ContactInfo.State + "," + ContactInfo.PostalCode;

                }

                if (ForemanInfo != null)
                {
                    txtPeopleForeman.Text = ForemanInfo.Name;
                }
                if (SubContractorInfo != null)
                {
                    txtSubContractorName.Text = SubContractorInfo.ShortName;
                    txtSubContractorAddr.Text = SubContractorInfo.Street + " " + SubContractorInfo.PostalCode + " " + SubContractorInfo.State;
                    ABN_DB.Value = SubContractorInfo.Abn;
                    txtABN.Text = SubContractorInfo.Abn;
                }
                if (SOI.ItemsTotal == 0)
                {
                    //BackColor="#DDDDDD" ReadOnly="True"
                    txtSubTotal.BackColor = txtNotes.BackColor;
                    txtSubTotal.ReadOnly = false;
                }
                else
                {
                    txtSubTotal.BackColor = txtGivenByName.BackColor;
                    txtSubTotal.ReadOnly = true;
                    txtSubTotal.Text = txtItemsTotal.Text;
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        private void FormToObjects()
        {
            SOI = new SiteOrderInfo();
            SOI.Title = txtTitle.Text;
            SOI.Typ = "Ins";
            SOI.ProjectId = (int)UI.Utils.GetFormInteger(parameterProjectId);
            SOI.SubContractorId = (int)UI.Utils.GetFormInteger(txtSubContractorId.Value);
            SOI.ForemanID = (int)UI.Utils.GetFormInteger(txtForemanId.Value);
            //SOI.OrderDate = sdrDate.Date;
            SOI.OrderDate = (DateTime)UI.Utils.GetFormDate(txtDate.Text);
            SOI.ItemsTotal = UI.Utils.GetFormDecimal(txtItemsTotal.Text);
            SOI.SubTotal = UI.Utils.GetFormDecimal(txtSubTotal.Text);
            // DS20230306  SOI.DeliveryFee = UI.Utils.GetFormDecimal(txtDeliveryFee.Text);
            // DS20230306 SOI.SubTotal = SOI.ItemsTotal + SOI.DeliveryFee;
            // DS20230304 SOI.GST = UI.Utils.GetFormDecimal(txtGST.Text);
            // DS20230304 SOI.Total = SOI.SubTotal + SOI.GST;
            // DS20230304 txtTotal.Text = UI.Utils.SetFormEditDecimal(SOI.Total);
            SOI.Email = txtEmail.Text;
            SOI.Contact = txtContact.Text;
            // DS20230304 SOI.ContactPhone = txtAtt.Text;
            SOI.Name = txtSubContractorName.Text;
            SOI.Street = txtSubContractorAddr.Text;
            SOI.ABN = txtABN.Text;
            //
            //
            SOI.Status = "AT";
            SOI.DateStart = DateTime.Today;
            SOI.DateEnd = DateTime.Today;
            SOI.PaymentStatus = "NEW";
            SOI.PeriodType = "M";
            if (txtVariationId.Text == "")
            {
                txtVariationId.Text = "0";
                SOI.VariationID = 0;
            }
            else
            {
                SOI.VariationID = (int)UI.Utils.GetFormInteger(txtVariationId.Text);
            }

            
            SOI.ContactPeopleId = (int)UI.Utils.GetFormInteger(txtContactId.Value);
            SOI.OrderCode = ddlCodeType.SelectedValue;
            SOI.Notes = txtNotes.Text;
            SOI.Comments = txtComments.Text; // DS20230320
            SOI.TypeInfo = txtTypeInfo.Text; // DS20230320
            SOI.TradesCode = txtTradesCode.Value; // DS20230320
            SOI.GivenByPeopleId = (int)UI.Utils.GetFormInteger(txtGivenBy.Value);// DS20230320
            SOI.TradesCode = ddTradesCode.SelectedValue;// DS20230320
            // DS20230304 SOI.GSTRate = UI.Utils.GetFormDecimal(GSTRate.Value);
        }
        private void ObjectsToForm()
        {
            char c = (char)34;
            string q = c.ToString();
            txtTitle.Text = SOI.Title;
            txtSubContractorId.Value = UI.Utils.SetFormEditInteger(SOI.SubContractorId);
            txtForemanId.Value = UI.Utils.SetFormEditInteger(SOI.ForemanID);
            //sdrDate.Date = SOI.OrderDate;
            txtDate.Text = UI.Utils.SetFormDate(SOI.OrderDate);
            txtItemsTotal.Text = UI.Utils.SetFormEditDecimal(SOI.ItemsTotal) ;
            // DS20230306 txtDeliveryFee.Text = UI.Utils.SetFormEditDecimal(SOI.DeliveryFee);
            txtSubTotal.Text = UI.Utils.SetFormEditDecimal(SOI.SubTotal);
            // DS20230304 txtGST.Text = UI.Utils.SetFormEditDecimal(SOI.GST);
            // DS20230304 txtTotal.Text = UI.Utils.SetFormEditDecimal(SOI.Total);
            txtContact.Text = SOI.Contact;
            txtEmail.Text = SOI.Email;
            // DS20230304 txtAtt.Text = SOI.ContactPhone;
            txtABN.Text = SOI.ABN;
            txtVariationId.Text = UI.Utils.SetFormEditInteger(SOI.VariationID);
            // DS20230304 GSTRate.Value = UI.Utils.SetFormEditDecimal(SOI.GSTRate);
            txtContactId.Value = UI.Utils.SetFormEditInteger(SOI.ContactPeopleId);
            cmdSelContact.NavigateUrl = Utils.PopupPeopleSub(this, txtContactId.ClientID, txtContact.ClientID, "CL", txtSubContractorId.Value);
            ddlCodeType.SelectedValue = SOI.OrderCode  ;
            txtNotes.Text = SOI.Notes;
            txtComments.Text = SOI.Comments;       // DS20230320
            txtTypeInfo.Text = SOI.TypeInfo;       // DS20230320
            txtTradesCode.Value = SOI.TradesCode; // DS20230320
            txtGivenBy.Value = UI.Utils.SetFormEditInteger(SOI.GivenByPeopleId); // DS20230320
            ddTradesCode.SelectedValue = SOI.TradesCode;// DS20230320
        }
        protected void odsSiteOrders_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SiteOrdersController.GetInstance();
        }
      
        protected void ddlSiteOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ddlCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void PostBack_Click(object sender, EventArgs e)
        {
            int x = 1;
        }

            protected void cmdSearch_Click(object sender, EventArgs e)
        {
        }

        protected void gvSiteOrders_OnSorting(object sender, GridViewSortEventArgs e)
        {

            if (GvSiteOrdersSortExpression == e.SortExpression)
                GvSiteOrdersSortDireccion = Utils.ChangeSortDirection(GvSiteOrdersSortDireccion);
            else
            {
                GvSiteOrdersSortExpression = e.SortExpression;
                GvSiteOrdersSortDireccion = SortDirection.Ascending;
            }

            e.SortDirection = GvSiteOrdersSortDireccion;
        }

        protected void gvSiteOrders_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
        }
        #endregion
        protected void txtSubContractorId_Changed(object sender, EventArgs e)
            {
            parameterOrderId = Request.Params["OrderId"];
        }
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            parameterProjectId = Request.Params["ProjectId"];
            if (parameterOrderId == null)
            {
                Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);
            }
            else
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            } 
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
            protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            PeopleController PeopleController = PeopleController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            
            if (Page.IsValid)
            {
                SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
                parameterOrderId = Request.Params["OrderId"];
                FormToObjects();
                lblMessage.Text = "";
                if (SOI.Title == "" )
                    {
                }
                if (txtTitle.Text == "") lblMessage.Text += "Please enter Order Title!" + "\r\n";
                if (SOI.OrderCode == "") { lblMessage.Text += "Please Select Type!" + "\r\n"; }
                if (SOI.OrderCode == "B/Ch")
                {
                    if (SOI.TypeInfo == "")
                    {
                        lblMessage.Text += "Type Info is Mandatory for Back Charge!" + "\r\n";
                    }
                }
                if (txtSubContractorId.Value == "0") 
                {
                    lblMessage.Text += "Please Select Sub-Contractor!" + "\r\n";
                }
                else
                {
                    SubContractorInfo = SubContractorsController.GetSubContractor(Int32.Parse(txtSubContractorId.Value));
                    if (SubContractorInfo != null)
                    {
                        
                        txtSubContractorName.Text = SubContractorInfo.ShortName;
                        txtSubContractorAddr.Text = SubContractorInfo.Street + " " + SubContractorInfo.PostalCode + " " + SubContractorInfo.State;
                        ABN_DB.Value = SubContractorInfo.Abn;
                        txtABN.Text = SubContractorInfo.Abn;
                    }
                    else
                    {
                        lblMessage.Text += "Please Select Sub-Contractor!" + "\r\n";
                    }
                }
                // if (txtABN.Text != ABN_DB.Value) lblMessage.Text += "ABN does not match Sub-Contractor!" + "\r\n";  20230320
                ContactInfo = null;
                if (SOI.ContactPeopleId == 0)
                {
                   // lblMessage.Text += "Please Select Contact!" + "\r\n";
                }
                else
                { 
                    ContactInfo = PeopleController.GetPersonById(SOI.ContactPeopleId);
                }
                if (ContactInfo == null)
                {
                   // lblMessage.Text += "Contact Not found!" + "\r\n";
                }
                else
                { 
                    SOI.Contact = ContactInfo.Name;
                    // DS20230304 SOI.ContactPhone = ContactInfo.Phone;
                    if (SOI.Email == "") { SOI.Email = ContactInfo.Email; }


                }
                if (SOI.Email == "") { lblMessage.Text += "e-mail is mandatory!" + "\r\n"; }
                if (txtVariationId.Text != "0")
                {
                    parameterProjectId = Request.Params["ProjectId"];
                    ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
                    int VariationId = Int32.Parse(txtVariationId.Text);
                    ClientVariationInfo = ProjectsController.GetClientVariationByNumber(VariationId, ProjectInfo, "CV");
                    if (ClientVariationInfo == null) lblMessage.Text += "Invalid Client Variation!" + "\r\n";
                }
                if (SOI.TradesCode == "") { lblMessage.Text += "Budget Code is Mandatory!" + "\r\n"; }

                if (lblMessage.Text != "")
                {
                    lblMessage.Visible = true;
                    return;

                }
                if (parameterOrderId == null)
                {
                    SOI.Id = SiteOrdersController.GetInstance().AddSiteOrder(SOI);
                    int rc  = (int)SiteOrdersController.GetInstance().AddSiteOrderApprovalsProcess(SOI);
                }
                else
                {
                    SOI.Id = (int)UI.Utils.GetFormInteger(parameterOrderId); 
                    SiteOrdersController.GetInstance().AddUpdateSiteOrder(SOI);
                }

                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                if (parameterOrderId == null)
                {
                    parameterOrderId = SOI.IdStr;
                    Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetailInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                }
                else
                {
                    Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                }
            }
        }

        protected void txtSubContractorName_TextChanged(object sender, EventArgs e)
        {
            int a = 1;
        }
    }
}
