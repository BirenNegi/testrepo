using System;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewTransmittalPage : SOSPage
    {

#region Members
        private TransmittalInfo transmittalInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (transmittalInfo == null)
                return null;

            tempNode.ParentNode.Url += "?ProjectId=" + transmittalInfo.Project.IdStr;

            tempNode.ParentNode.ParentNode.Title = transmittalInfo.Project.Name + (transmittalInfo.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + transmittalInfo.Project.IdStr;

            return currentNode;
        }

        private void BindTree()
        {
            XmlDocument xmlDocument = ProjectsController.GetInstance().CheckTransmittal(transmittalInfo);

            if (xmlDocument.DocumentElement != null)
            {
                TreeViewCheck.Nodes.Clear();
                TreeViewCheck.Nodes.Add(new TreeNode());
                Utils.AddNode(xmlDocument.DocumentElement, TreeViewCheck.Nodes[0]);
                TreeViewCheck.ExpandAll();
                pnlErrors.Visible = true;

                cmdSendByEmail.OnClientClick = "javascript:alert('See Tansmittal Check for errors or missing fields'); return false;";
            }
            else
            {
                pnlErrors.Visible = false;

                if (transmittalInfo.SentDate != null)
                    cmdSendByEmail.OnClientClick = "javascript:alert('The transmittal was sent on " + UI.Utils.SetFormDate(transmittalInfo.SentDate) + "'); return false;";
                else
                    cmdSendByEmail.OnClientClick = "javascript:return confirm('Send by Email ?');";
            }
        }

        protected void BindJavaScripts()
        {
            lnkPrint.NavigateUrl = "~/Modules/Projects/ShowTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr;
            //#--- cmdSelRecipient.NavigateUrl = Utils.PopupPeople(this, txtRecipientId.ClientID, txtRecipientName.ClientID, PeopleInfo.PeopleTypeContact, transmittalInfo.Project.BusinessUnit);
            cmdSelRecipient.NavigateUrl = Utils.PopupPeopleFromProject(this, txtRecipientId.ClientID, txtRecipientName.ClientID, PeopleInfo.PeopleTypeContact, transmittalInfo);


        }

        private void BindRecipients()
        {
            gvRecipients.DataSource = transmittalInfo.Contacts;
            gvRecipients.DataBind();
            //#---
            GVDistributionList.DataSource = transmittalInfo.ClientContacts;
            GVDistributionList.DataBind();
            //#---

        }

        private void BindRevisions()
        {
            Boolean includeInList;

            repDrawings.DataSource = transmittalInfo.TransmittalRevisions;
            repDrawings.DataBind();

            ddlDrawings.Items.Clear();
            ddlDrawings.Items.Add(new ListItem(String.Empty, String.Empty));

            foreach (DrawingInfo drawingInfo in transmittalInfo.Project.Drawings)
            {
                if (drawingInfo.LastRevision != null)
                {
                    includeInList = true;
                    if (transmittalInfo.TransmittalRevisions != null)
                    {
                        foreach (TransmittalRevisionInfo transmittalRevisionInfo in transmittalInfo.TransmittalRevisions)
                        {
                            if (transmittalRevisionInfo.Revision.Drawing.Equals(drawingInfo))
                            {
                                includeInList = false;
                                break;
                            }
                        }
                    }

                    if (includeInList)
                        ddlDrawings.Items.Add(new ListItem(drawingInfo.Summary, drawingInfo.LastRevision.IdStr));
                }
            }
        }

        private void BindSentDate()
        {
            lblSentDate.Text = UI.Utils.SetFormDate(transmittalInfo.SentDate);
        }

        private void BindTransmittal()
        {
            if (transmittalInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            lblNumber.Text = UI.Utils.SetFormInteger(transmittalInfo.TransmittalNumber);
            lblDate.Text = UI.Utils.SetFormDate(transmittalInfo.TransmissionDate);
            lblDevilvery.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("Transmittals", "DeliveryMethod", transmittalInfo.DeliveryMethod));
            lblComments.Text = UI.Utils.SetFormString(transmittalInfo.Comments);
            lblType.Text = UI.Utils.SetFormString(Utils.GetConfigListItemNameAndOther("Transmittals", "TransmittalType", transmittalInfo.TransmittalType, transmittalInfo.TransmittalTypeOther));
            lblAction.Text = UI.Utils.SetFormString(Utils.GetConfigListItemNameAndOther("Transmittals", "RequiredAction", transmittalInfo.RequiredAction, transmittalInfo.RequiredActionOther));
           
            //#---
                #region Old
            /*
             lblClientContactCompany.Text=UI.Utils.SetFormString(transmittalInfo.Project.ClientContact.CompanyName);
            lblClientContactName.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact.Name);
            lnkClientContactEmail.Text = transmittalInfo.Project.ClientContact.Email;
            lnkClientContactEmail.NavigateUrl = String.Format("mailto:{0}", UI.Utils.SetFormString(transmittalInfo.Project.ClientContact.Email));
            rowClientContact.Visible = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact);

            lblClientContact1Company.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact1.CompanyName);
            lblClientContact1Name.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact1.Name);
            lnkClientContact1Email.Text = transmittalInfo.Project.ClientContact1.Email;
            lnkClientContact1Email.NavigateUrl = String.Format("mailto:{0}", UI.Utils.SetFormString(transmittalInfo.Project.ClientContact1.Email));
            rowClientContact1.Visible = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact1);

            lblClientContact2Company.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact2.CompanyName);
            lblClientContact2Name.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact2.Name);
            lnkClientContact2Email.Text = transmittalInfo.Project.ClientContact2.Email;
            lnkClientContact2Email.NavigateUrl = String.Format("mailto:{0}", UI.Utils.SetFormString(transmittalInfo.Project.ClientContact2.Email));
            rowClientContact2.Visible = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact2);

            lblSuperintendentCompany.Text = UI.Utils.SetFormString(transmittalInfo.Project.Superintendent.CompanyName);
            lblSuperintendentName.Text = UI.Utils.SetFormString(transmittalInfo.Project.Superintendent.Name);
            lnkSuperintendentEmail.Text = transmittalInfo.Project.Superintendent.Email;
            lnkSuperintendentEmail.NavigateUrl = String.Format("mailto:{0}", UI.Utils.SetFormString(transmittalInfo.Project.Superintendent.Email));
            rowSuperintendent.Visible = UI.Utils.SetFormBoolean(transmittalInfo.SendSuperintendent);

            lblQuantitySurveyorCompany.Text = UI.Utils.SetFormString(transmittalInfo.Project.QuantitySurveyor.CompanyName);
            lblQuantitySurveyorName.Text = UI.Utils.SetFormString(transmittalInfo.Project.QuantitySurveyor.Name);
            lnkQuantitySurveyorEmail.Text = transmittalInfo.Project.QuantitySurveyor.Email;
            lnkQuantitySurveyorEmail.NavigateUrl = String.Format("mailto:{0}", UI.Utils.SetFormString(transmittalInfo.Project.QuantitySurveyor.Email));
            rowQuantitySurveyor.Visible = UI.Utils.SetFormBoolean(transmittalInfo.SendQuantitySurveyor);
            */
            #endregion
            //#---


            if (transmittalInfo.SubContractor != null)
            {
                lnkSubcontractor.Text = UI.Utils.SetFormString(transmittalInfo.SubContractor.Name);
                lnkSubcontractor.NavigateUrl = "~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId=" + transmittalInfo.SubContractor.IdStr;
            }

            if (transmittalInfo.Contact != null)
            {
                lnkContact.Text = UI.Utils.SetFormString(transmittalInfo.Contact.Name);
                lnkContact.NavigateUrl = "~/Modules/People/ViewContact.aspx?PeopleId=" + transmittalInfo.Contact.IdStr;
            }

            BindSentDate();
            BindRecipients();
            BindRevisions();
            BindTree();
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            String parameterTransmittalId;
            //#----
            ChkIndvidual.Enabled = false;
            //#-----

            try
            {
                Security.CheckAccess(Security.userActions.ViewTransmittal);
                parameterTransmittalId = Utils.CheckParameter("TransmittalId");
                transmittalInfo = projectsController.GetDeepTransmittal(Int32.Parse(parameterTransmittalId));
                Core.Utils.CheckNullObject(transmittalInfo, parameterTransmittalId, "Transmittal");

                if (transmittalInfo.IsActive)
                    transmittalInfo.Project = projectsController.GetProjectWithDrawingsActive(transmittalInfo.Project.Id);
                else if (transmittalInfo.IsProposal)
                    transmittalInfo.Project = projectsController.GetProjectWithDrawingsProposal(transmittalInfo.Project.Id);
                else
                    throw new Exception("Invalid transmittal type");
                
                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTransmittal))
                    {
                        if (processController.AllowUpdateDrawingsCurrentUser(transmittalInfo.Project))
                        {
                            ddlDrawings.Visible = true;
                            txtCopies.Visible = true;
                            cmdSave.Visible = true;
                            cmdCopy.Visible = true;

                            //#--to stop editing mode when its already sent

                            if (transmittalInfo.SentDate != null)
                            {
                                cmdEditTop.Visible = false;
                                cmdDeleteTop.Visible = false;
                                phAddRecipient.Visible = false;
                            }
                            else {
                                cmdEditTop.Visible = true;
                                cmdDeleteTop.Visible = true;
                                phAddRecipient.Visible = true;
                            }
                            //#-- 


                            //#---cmdEditTop.Visible = true;
                            //#---cmdDeleteTop.Visible = true;
                            //#--- phAddRecipient.Visible = true;





                            //#--- if (transmittalInfo.DeliveryMethod == TransmittalInfo.DeliveryMethodEmail)

                           

                            if (transmittalInfo.SentDate == null && (transmittalInfo.DeliveryMethod == TransmittalInfo.DeliveryMethodEmail || transmittalInfo.DeliveryMethod == TransmittalInfo.DeliveryMethodDownloadFromSOS))//#----
                            {
                                phSendByEmail.Visible = true;

                                
                            }
                        }
                    }

                    BindTransmittal();
                  

                }

                BindJavaScripts();
                //#-----
                if (transmittalInfo.Contacts.Count > 0 && phSendByEmail.Visible == true)
                    ChkIndvidual.Enabled = true;
                else
                    ChkIndvidual.Enabled = false;
                //#----
                pnlMessage.Visible = false;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


        }







        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteTransmittal(transmittalInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + transmittalInfo.Project.IdStr);
        }

        protected void cmdCopy_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().CopyTransmittal(transmittalInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr);
        }

        protected void cmdSendByEmail_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                transmittalInfo.SentDate = DateTime.Now;

                //#----Start--// Store all contacts in the new list and clears it from transmittals list and calls SendTransmittal function for all contacts



                transmittalInfo.CreatedBy= Utils.GetCurrentUser().Id;// It will sends email from the person who clicked on the email  button rather than who created the transmittals 

                #region send individually
                if (ChkIndvidual.Checked && transmittalInfo.Contacts.Count > 0)
                {
                    ContactInfo Ctfo = new ContactInfo();
                    ContactInfo DummyContactuinfo = new ContactInfo();
                    List <ContactInfo> LstContacts= new List<ContactInfo>();
                    int? parentTransmittalId=null;
                    DateTime? transmissiondate = null;
                    TransmittalInfo ParentTransmittal = new TransmittalInfo();

                    foreach (ContactInfo contactInfo in transmittalInfo.Contacts)
                        LstContacts.Add(contactInfo);

                    Ctfo = transmittalInfo.Contact;
                        //----LstContacts.Add(transmittalInfo.Contact);

                        transmittalInfo.Contacts.Clear();
                        projectsController.SendTransmittal(transmittalInfo);// to send transmittals to by dafault selected Contractors

                        ParentTransmittal = transmittalInfo;
                        parentTransmittalId = transmittalInfo.Id;// to store original transmittal Id as parent
                        transmissiondate = transmittalInfo.TransmissionDate;


                   
                        foreach (ContactInfo contactInfo in LstContacts)// to send transmittals to other selected Contractors individually
                        {                        
                            projectsController.CopyTransmittal(transmittalInfo);

                            transmittalInfo.Contact=contactInfo;
                            transmittalInfo.SentDate = DateTime.Now;
                            transmittalInfo.TransmissionDate = transmissiondate;

                            projectsController.AddUpdateTransmittal(transmittalInfo);

                            transmittalInfo.Project.ClientContact.Email = DummyContactuinfo.Email;
                            transmittalInfo.Project.ClientContact1.Email = DummyContactuinfo.Email;
                            transmittalInfo.Project.ClientContact2.Email = DummyContactuinfo.Email;
                            transmittalInfo.Project.ClientContact1.Email = DummyContactuinfo.Email;
                            transmittalInfo.Project.Superintendent.Email = DummyContactuinfo.Email;
                            transmittalInfo.Project.QuantitySurveyor.Email = DummyContactuinfo.Email;

                            transmittalInfo.TransmittalNumber = transmittalInfo.TransmittalNumber + 1;
                            projectsController.SendTransmittal(transmittalInfo);
                        
                        }

                    transmittalInfo = projectsController.GetDeepTransmittal(parentTransmittalId);
                    //if(transmittalInfo.Contacts!=null)// To clear all additional recepients from the Transmittal and update in database
                    //transmittalInfo.Contacts.Clear();
                    foreach(ContactInfo contact in transmittalInfo.Contacts)
                    projectsController.DeleteTransmittalContact(transmittalInfo,contact);

                }
                else
                {
                    projectsController.SendTransmittal(transmittalInfo);
                }

                #endregion


                //#----End


                //#----projectsController.SendTransmittal(transmittalInfo);


                transmittalInfo.SentDate = DateTime.Now;
                projectsController.UpdateTransmittalSentDate(transmittalInfo);

                BindSentDate();
                BindTree();

                pnlMessage.Visible = true;

                //#-- To refresh the page itself

                // Response.Redirect(Request.RawUrl);
              
                //#-- To refresh the page itself

               
               
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvRecipients_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            try
            {
                int contactId = (int)gvRecipients.DataKeys[e.RowIndex].Value;
                projectsController.DeleteTransmittalContact(transmittalInfo, new ContactInfo(contactId));
                transmittalInfo.Contacts = projectsController.GetTransmittalContacts(transmittalInfo);
                
                BindRecipients();
                BindTree();

                if (transmittalInfo.Contacts.Count > 0 && phSendByEmail.Visible==true)
                    ChkIndvidual.Enabled = true;
                else
                    ChkIndvidual.Enabled = false;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddRecipient_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            PeopleController peopleController = PeopleController.GetInstance();
            ContactInfo contactInfo;

            try
            {
                if (txtRecipientId.Value != String.Empty)
                {
                    contactInfo = (ContactInfo)peopleController.GetPersonById(Int32.Parse(txtRecipientId.Value));

                    if (transmittalInfo.Contacts == null || transmittalInfo.Contacts.Find(delegate(ContactInfo contactInfoInList) { return contactInfoInList.Equals(contactInfo); }) == null)
                    {
                        projectsController.AddTransmittalContact(transmittalInfo, contactInfo);
                        transmittalInfo.Contacts = projectsController.GetTransmittalContacts(transmittalInfo);

                        BindRecipients();
                        BindTree();
                    }

                    txtRecipientId.Value = String.Empty;
                }
                if (transmittalInfo.Contacts.Count > 0 && phSendByEmail.Visible == true)
                    ChkIndvidual.Enabled = true;
                else
                    ChkIndvidual.Enabled = false;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdSaveDrawing_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TransmittalRevisionInfo transmittalRevisionInfo = new TransmittalRevisionInfo();

            try
            {
                if (ddlDrawings.SelectedValue != String.Empty)
                {
                    transmittalRevisionInfo.Transmittal = transmittalInfo;
                    transmittalRevisionInfo.Revision = new DrawingRevisionInfo(Int32.Parse(ddlDrawings.SelectedValue));

                    if (UI.Utils.GetFormInteger(txtCopies.Text) == null)
                        transmittalRevisionInfo.NumCopies = 1;
                    else
                        transmittalRevisionInfo.NumCopies = UI.Utils.GetFormInteger(txtCopies.Text);

                    projectsController.AddTransmittalRevision(transmittalRevisionInfo);
                    transmittalInfo.TransmittalRevisions = projectsController.GetTransmittalRevisions(transmittalInfo);

                    BindRevisions();
                    BindTree();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void repDrawings_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TransmittalRevisionInfo transmittalRevisionInfo = new TransmittalRevisionInfo();

            try
            {
                if (e.CommandName == "Delete")
                {
                    transmittalRevisionInfo.Transmittal = transmittalInfo;
                    transmittalRevisionInfo.Revision = new DrawingRevisionInfo(Int32.Parse(e.CommandArgument.ToString()));
                    projectsController.DeleteTransmittalRevision(transmittalRevisionInfo);
                    transmittalInfo.TransmittalRevisions = projectsController.GetTransmittalRevisions(transmittalInfo);

                    BindRevisions();
                    BindTree();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
    
    }
}
