using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using System.Data; //#---

using SOS.Core;

namespace SOS.Web
{
    public partial class EditTransmittalPage : SOSPage
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

        private HtmlTableCell CellText(String className, String strColSpan, String innerText)
        {
            HtmlTableCell cell = new HtmlTableCell();

            cell.Attributes.Add("class", className);

            if (strColSpan != "1")
                cell.Attributes.Add("colspan", strColSpan);

            cell.InnerText = innerText;

            return cell;
        }

        private HtmlTableCell CellTextCenter(String className, String strColSpan, String innerText)
        {
            HtmlTableCell cell = CellText(className, strColSpan, innerText);
            cell.Align = "Center";
            return cell;
        }

        private HtmlTableCell CellCheckBox(ref CheckBox checkBox, String className, String controlName)
        {
            HtmlTableCell cell = new HtmlTableCell();
            checkBox = new CheckBox();

            checkBox.ID = controlName;

            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            cell.Controls.Add(checkBox);

            return cell;
        }

        private HtmlTableCell CellTextBox(ref TextBox textBox, String className, String controlName)
        {
            HtmlTableCell cell = new HtmlTableCell();
            CompareValidator compareValidator = new CompareValidator();

            textBox = new TextBox();
            textBox.ID = controlName;
            textBox.Width = new Unit("24px");
            textBox.Height = new Unit("16px");
            textBox.MaxLength = 2;

            compareValidator.ControlToValidate = textBox.ClientID;
            compareValidator.SetFocusOnError = true;
            compareValidator.ErrorMessage = "Invalid number!<br />";
            compareValidator.CssClass = "frmError";
            compareValidator.Display = ValidatorDisplay.Dynamic;
            compareValidator.Type = ValidationDataType.Integer;
            compareValidator.Operator = ValidationCompareOperator.GreaterThan;
            compareValidator.ValueToCompare = "0";
            
            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            cell.Controls.Add(compareValidator);
            cell.Controls.Add(textBox);

            return cell;
        }

        private void CreateForm()
        {
            TradesController tradesController = TradesController.GetInstance();
            List<DrawingTypeInfo> drawingTypeInfoList = tradesController.GetDrawingTypes();


            //#------


            //////TransmittalRevisionInfo transmittalRevisionInfo = null;
            //////Boolean drawingTypeUsed;
            //////HtmlTableRow row;
            //////HtmlTable table;
            //////CheckBox checkBoxParent = null;
            //////CheckBox checkBoxOld = null;
            //////CheckBox checkBox = null;
            //////TextBox textBox = null;
            //////String currStyle;
            //////String childrenControlsIds;
            //////String oldRevisionsControlsIds = "";


            GvDrawingsSortExpression = "NumberOfCopies";//LastRevisionDate
            GvDrawingsSortDireccion = SortDirection.Descending;
            BindDrawings();

            
            #region Old Htmltable  

             /*----san---

            
            table = new HtmlTable();
            table.CellPadding = 4;
            table.CellSpacing = 1;
            phDrawings.Controls.Add(table);

            if (transmittalInfo.Project.Drawings != null) 
            {
                row = new HtmlTableRow();
                row.Cells.Add(CellText("lstHeader", "2", "Select"));
                row.Cells.Add(CellText("lstHeader", "1", "Copies"));
                row.Cells.Add(CellText("lstHeader", "1", "Drawing"));
                row.Cells.Add(CellText("lstHeader", "1", "Revision"));
               row.Cells.Add(CellText("lstHeader", "1", "Description"));
                table.Rows.Add(row);

                foreach (DrawingTypeInfo drawingTypeInfo in drawingTypeInfoList)
                {
                    drawingTypeUsed = false;
                    foreach (DrawingInfo drawingInfo in transmittalInfo.Project.Drawings)
                        if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                        {
                            drawingTypeUsed = true;
                            break;
                        }

                    if (drawingTypeUsed)
                    {
                        row = new HtmlTableRow();
                        row.Cells.Add(CellCheckBox(ref checkBoxParent, "lstHeaderTop", "drawingTypeId_" + drawingTypeInfo.IdStr));
                        row.Cells.Add(CellText("lstHeaderTop", "5", drawingTypeInfo.Name));
                        table.Rows.Add(row);

                        currStyle = String.Empty;
                        childrenControlsIds = String.Empty;
                        oldRevisionsControlsIds = String.Empty;

                        foreach (DrawingInfo drawingInfo in transmittalInfo.Project.Drawings)
                        {
                            if (drawingInfo.DrawingType.Equals(drawingTypeInfo) && drawingInfo.LastRevision != null)
                            {
                                currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                                row = new HtmlTableRow();
                                row.Cells.Add(CellText("", "1", ""));
                                row.Cells.Add(CellCheckBox(ref checkBox, currStyle, "drawingRevisionId_" + drawingInfo.LastRevision.IdStr));
                                row.Cells.Add(CellTextBox(ref textBox, currStyle, "drawingRevisionNumCopies_" + drawingInfo.LastRevision.IdStr));
                                row.Cells.Add(CellText(currStyle, "1", drawingInfo.Name));
                                row.Cells.Add(CellTextCenter(currStyle, "1", drawingInfo.LastRevisionNumber));
                                row.Cells.Add(CellText(currStyle, "1", drawingInfo.Description));
                                table.Rows.Add(row);

                                childrenControlsIds = childrenControlsIds + "'" + checkBox.ClientID + "',";

                                // if a revision different than the last one is being used display also that version
                                if (transmittalInfo.TransmittalRevisions != null)
                                    transmittalRevisionInfo = transmittalInfo.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo transmittalRevisionInfoInList) { return drawingInfo.Equals(transmittalRevisionInfoInList.Drawing); });

                                if (transmittalRevisionInfo != null && !transmittalRevisionInfo.Revision.Equals(drawingInfo.LastRevision))
                                {
                                    row = new HtmlTableRow();
                                    row.Cells.Add(CellText("", "1", ""));
                                    row.Cells.Add(CellCheckBox(ref checkBoxOld, currStyle, "drawingRevisionId_" + transmittalRevisionInfo.Revision.IdStr));
                                    row.Cells.Add(CellTextBox(ref textBox, currStyle, "drawingRevisionNumCopies_" + transmittalRevisionInfo.Revision.IdStr));
                                    row.Cells.Add(CellText(currStyle, "1", ""));
                                    row.Cells.Add(CellTextCenter("lstErrItem", "1", transmittalRevisionInfo.Revision.Number));
                                    row.Cells.Add(CellText(currStyle, "1", ""));
                                    table.Rows.Add(row);

                                    checkBox.Attributes["onClick"] = "CheckUncheck('" + checkBox.ClientID + "','" + checkBoxOld.ClientID + "');";
                                    checkBoxOld.Attributes["onClick"] = "CheckUncheck('" + checkBoxOld.ClientID + "','" + checkBox.ClientID + "');";

                                    oldRevisionsControlsIds = oldRevisionsControlsIds + "'" + checkBoxOld.ClientID + "',";
                                    childrenControlsIds = childrenControlsIds + "'" + checkBoxOld.ClientID + "',";
                                }
                            }
                        }

                        if (childrenControlsIds != String.Empty)
                        {
                            if (oldRevisionsControlsIds != String.Empty)
                                oldRevisionsControlsIds = "[" + oldRevisionsControlsIds.Substring(0, oldRevisionsControlsIds.Length - 1) + "]";
                            else
                                oldRevisionsControlsIds = "null";

                            checkBoxParent.Attributes["onClick"] = "CheckUncheckAll('" + checkBoxParent.ClientID + "',[" + childrenControlsIds.Substring(0, childrenControlsIds.Length - 1) + "]," + oldRevisionsControlsIds + ");";
                        }
                    }
                }
            }
 
            ----san---*/
            #endregion


           //#--------


        }

        private void ObjectsToForm()
        {
            //CheckBox checkBox;
            //TextBox textBox;

            if (transmittalInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            if (transmittalInfo.Id == null)
            {
                TitleBar.Title = "Adding Transmittal";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
                TitleBar.Title = "Updating Transmittal";

            sdrDate.Date = transmittalInfo.TransmissionDate;
            sdrSentDate.Date = transmittalInfo.SentDate;
            txtComments.Text = UI.Utils.SetFormString(transmittalInfo.Comments);
            ctrType.TextOther = transmittalInfo.TransmittalTypeOther;
            ctrAction.TextOther = transmittalInfo.RequiredActionOther;
            // ---#---- 
            #region Old
            /*
                lblClientContactCompany.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact.CompanyName);
                lblClientContactName.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact.Name);
                lblClientContactEmail.Text = transmittalInfo.Project.ClientContact.Email;

                lblClientContact1Company.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact1.CompanyName);
                lblClientContact1Name.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact1.Name);
                lblClientContact1Email.Text = transmittalInfo.Project.ClientContact1.Email;

                lblClientContact2Company.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact2.CompanyName);
                lblClientContact2Name.Text = UI.Utils.SetFormString(transmittalInfo.Project.ClientContact2.Name);
                lblClientContact2Email.Text = transmittalInfo.Project.ClientContact2.Email;

                lblSuperintendentCompany.Text = UI.Utils.SetFormString(transmittalInfo.Project.Superintendent.CompanyName);
                lblSuperintendentName.Text = UI.Utils.SetFormString(transmittalInfo.Project.Superintendent.Name);
                lblSuperintendentEmail.Text = transmittalInfo.Project.Superintendent.Email;

                lblQuantitySurveyorCompany.Text = UI.Utils.SetFormString(transmittalInfo.Project.QuantitySurveyor.CompanyName);
                lblQuantitySurveyorName.Text = UI.Utils.SetFormString(transmittalInfo.Project.QuantitySurveyor.Name);
                lblQuantitySurveyorEmail.Text = transmittalInfo.Project.QuantitySurveyor.Email;

                chkClientContact.Checked = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact);
                chkClientContact1.Checked = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact1);
                chkClientContact2.Checked = UI.Utils.SetFormBoolean(transmittalInfo.SendClientContact2);
                chkSuperintendent.Checked = UI.Utils.SetFormBoolean(transmittalInfo.SendSuperintendent);
                chkQuantitySurveyor.Checked = UI.Utils.SetFormBoolean(transmittalInfo.SendQuantitySurveyor);

               */
            #endregion
            //#--
                      
            if (transmittalInfo.Contact != null)
            {
                txtContactId.Value = transmittalInfo.Contact.IdStr;
                txtContactName.Text = transmittalInfo.Contact.Name;
            }

            Utils.GetConfigListAddEmptyAddOther("Transmittals", "TransmittalType", ctrType.dropDownList, transmittalInfo.TransmittalType);
            Utils.GetConfigListAddEmpty("Transmittals", "DeliveryMethod", ddlDevilvery, transmittalInfo.DeliveryMethod);
            Utils.GetConfigListAddEmptyAddOther("Transmittals", "RequiredAction", ctrAction.dropDownList, transmittalInfo.RequiredAction);

            cmdSelContact.NavigateUrl = Utils.PopupPeople(this, txtContactId.ClientID, txtContactName.ClientID, PeopleInfo.PeopleTypeContact, transmittalInfo.Project.BusinessUnit);
           
            //#-----
            #region oldHtmlTable
            /*
            if (transmittalInfo.TransmittalRevisions != null)
                foreach (TransmittalRevisionInfo transmittalRevisionInfo in transmittalInfo.TransmittalRevisions)
                    if (transmittalRevisionInfo.Revision != null)
                    {
                        checkBox = (CheckBox)Utils.FindControlRecursive(phDrawings, "drawingRevisionId_" + transmittalRevisionInfo.Revision.IdStr);
                        checkBox.Checked = true;

                        textBox = (TextBox)Utils.FindControlRecursive(phDrawings, "drawingRevisionNumCopies_" + transmittalRevisionInfo.Revision.IdStr);
                        textBox.Text = UI.Utils.SetFormInteger(transmittalRevisionInfo.NumCopies);
                    }
            */
            #endregion
            //#----
        }

        private void FormToObjects()
        {
            PeopleController peopleController = PeopleController.GetInstance();
            TransmittalRevisionInfo transmittalRevisionInfo;
            CheckBox checkBox;
            TextBox textBox;

            transmittalInfo.Contact = txtContactId.Value != "" ? (ContactInfo)peopleController.GetPersonById(Int32.Parse(txtContactId.Value)) : null;

            transmittalInfo.TransmittalType = UI.Utils.GetFormString(ctrType.SelectedValue);
            transmittalInfo.TransmittalTypeOther = UI.Utils.GetFormString(ctrType.TextOther);
            transmittalInfo.DeliveryMethod = UI.Utils.GetFormString(ddlDevilvery.SelectedValue);
            transmittalInfo.RequiredAction = UI.Utils.GetFormString(ctrAction.SelectedValue);
            transmittalInfo.RequiredActionOther = UI.Utils.GetFormString(ctrAction.TextOther);
            transmittalInfo.TransmissionDate = sdrDate.Date;
            transmittalInfo.SentDate = sdrSentDate.Date;
            transmittalInfo.Comments = UI.Utils.GetFormString(txtComments.Text);


            //#---
            #region old
            /*
            transmittalInfo.SendClientContact = chkClientContact.Checked;
            transmittalInfo.SendClientContact1 = chkClientContact1.Checked;
            transmittalInfo.SendClientContact2 = chkClientContact2.Checked;
            transmittalInfo.SendSuperintendent = chkSuperintendent.Checked;
            transmittalInfo.SendQuantitySurveyor = chkQuantitySurveyor.Checked;
            */
            #endregion

            foreach (GridViewRow gRow in GvDistributionList.Rows)
            {
                ClientContactInfo clientContactInfo=null;
                int ClientId=0;
                if (gRow.RowType == DataControlRowType.DataRow)
                {
                    ClientId =int.Parse(gRow.Cells[0].Text);
                    clientContactInfo = transmittalInfo.Project.ClientContactList.Find(x=>x.Id==ClientId );
                    clientContactInfo.SendTransmittals = ((CheckBox)gRow.Cells[5].Controls[1]).Checked;
                }
            }

             //#---
                   transmittalInfo.TransmittalRevisions = new List<TransmittalRevisionInfo>();
            
            //#-----------
            #region Old
            ////foreach (DrawingInfo drawingInfo in transmittalInfo.Project.Drawings)
            ////    foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
            ////    {
            ////        checkBox = (CheckBox)Utils.FindControlRecursive(phDrawings, "drawingRevisionId_" + drawingRevisionInfo.IdStr);
            ////        if (checkBox != null && checkBox.Checked)
            ////        {
            ////            textBox = (TextBox)Utils.FindControlRecursive(phDrawings, "drawingRevisionNumCopies_" + drawingRevisionInfo.IdStr);

            ////            transmittalRevisionInfo = new TransmittalRevisionInfo();

            ////            transmittalRevisionInfo.Transmittal = transmittalInfo;
            ////            transmittalRevisionInfo.Revision = drawingRevisionInfo;
            ////            transmittalRevisionInfo.NumCopies = UI.Utils.GetFormInteger(textBox.Text) != null ? UI.Utils.GetFormInteger(textBox.Text) : 1;

            ////           // transmittalInfo.TransmittalRevisions.Add(transmittalRevisionInfo);
            ////        }
            ////    }
            #endregion

            CheckBox chk = null;
            TextBox txtBx = null; TextBox txtBxDrawingId = null; TextBox txtBxDrawingRevisonId = null;
            List<DrawingInfo> lstdrwInfo = null;
            DrawingInfo drwinfo = null;
            DrawingRevisionInfo drwRevisionInfo = null;
          

            if (transmittalInfo.Project.Drawings != null)
                lstdrwInfo = transmittalInfo.Project.Drawings;

            foreach (GridViewRow Row in gvDrawings.Rows)
            {
                 chk = Row.Cells[0].Controls[1] as CheckBox;

                if (chk.Checked)
                {

                   
                    txtBx = Row.Cells[1].FindControl("TxtNumberOfCopies") as TextBox;
                    txtBxDrawingId = Row.Cells[0].FindControl("TxtDrawingId") as TextBox;
                    txtBxDrawingRevisonId= Row.Cells[0].FindControl("TxtDrawingRevisionId") as TextBox;

                    drwinfo = lstdrwInfo.Find(x => x.IdStr == txtBxDrawingId.Text);
                    drwRevisionInfo= drwinfo.DrawingRevisions.Find(y => y.IdStr == txtBxDrawingRevisonId.Text);


                    transmittalRevisionInfo = new TransmittalRevisionInfo();
                    transmittalRevisionInfo.Transmittal = transmittalInfo;
                    transmittalRevisionInfo.Revision = drwRevisionInfo;
                    transmittalRevisionInfo.NumCopies = UI.Utils.GetFormInteger(txtBx.Text) != null ? UI.Utils.GetFormInteger(txtBx.Text) : 1;
                    transmittalInfo.TransmittalRevisions.Add(transmittalRevisionInfo);
                }

            }
//#---------
        }
#endregion

#region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            String parameterTransmittalId;

            try
            {
                Security.CheckAccess(Security.userActions.EditTransmittal);
                parameterTransmittalId = Request.Params["TransmittalId"];
                if (parameterTransmittalId == null)
                {
                    transmittalInfo = new TransmittalInfo();
                    String parameterTransmittalType = Request.Params["Type"];
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    transmittalInfo.Type = UI.Utils.GetFormString(parameterTransmittalType);

                    if (transmittalInfo.IsActive)
                        transmittalInfo.Project = projectsController.GetProjectWithDrawingsActive(Int32.Parse(parameterProjectId));
                    else if (transmittalInfo.IsProposal)
                        transmittalInfo.Project = projectsController.GetProjectWithDrawingsProposal(Int32.Parse(parameterProjectId));
                    else
                        throw new Exception("Invalid transmittal type");

                    Core.Utils.CheckNullObject(transmittalInfo.Project, parameterProjectId, "Project");
                    //#--
                    //GvDistributionList.DataSource = transmittalInfo.Project.ClientContactList;
                    //GvDistributionList.DataBind();
                    SqlDataSource1.SelectCommand = @"SELECT People.PeopleId, ClientAccess.ProjectId as TransmittalId, People.FirstName+' '+People.LastName as FirstName, People.EmployeePosition,
                                                     People.Email, ClientAccess.ProjectId,SendTransmittals=0 
                                                     FROM People INNER JOIN ClientAccess ON People.PeopleId = ClientAccess.PeopleId 
                                                     WHERE (ClientAccess.ProjectId='" + parameterProjectId + "')";
                    GvDistributionList.DataSource = SqlDataSource1;
                    GvDistributionList.DataBind();
                   // #--
                }
                else
                {
                    transmittalInfo = projectsController.GetTransmittalWithRevisions(Int32.Parse(parameterTransmittalId));
                    Core.Utils.CheckNullObject(transmittalInfo, parameterTransmittalId, "Transmittal");

                    if (transmittalInfo.IsActive)  
                        transmittalInfo.Project = projectsController.GetProjectWithDrawingsActive(transmittalInfo.Project.Id);
                   
                    else if (transmittalInfo.IsProposal)
                        transmittalInfo.Project = projectsController.GetProjectWithDrawingsProposal(transmittalInfo.Project.Id);
                    else
                        throw new Exception("Invalid transmittal type");

                    foreach (TransmittalRevisionInfo transmittalRevisionInfo in transmittalInfo.TransmittalRevisions)
                        if (transmittalRevisionInfo.Revision != null)
                            transmittalRevisionInfo.Revision.Drawing = transmittalInfo.Project.Drawings.Find(delegate(DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(transmittalRevisionInfo.Revision.Drawing); });
                    //#--
                    SqlDataSource1.SelectCommand = @"SELECT People.PeopleId, ClientAccess.ProjectId as TransmittalId, People.FirstName+ ' '+People.LastName as FirstName, People.EmployeePosition,
                                            People.Email, ClientAccess.ProjectId,SendTransmittals=Case When TransmittalsClientContact.TransmittalId is Null Then 0 Else 1 End 
                                            FROM People INNER JOIN ClientAccess ON People.PeopleId = ClientAccess.PeopleId 
                                            left outer JOIN TransmittalsClientContact ON People.PeopleId = TransmittalsClientContact.PeopleId and TransmittalsClientContact.TransmittalId='" + transmittalInfo.Id+@"'
                                            WHERE (ClientAccess.ProjectId='"+transmittalInfo.Project.Id+"')";
                    GvDistributionList.DataSource = SqlDataSource1;
                    GvDistributionList.DataBind();
                    //#--

                }

                processController.CheckUpdateDrawingsCurrentUser(transmittalInfo.Project);

                CreateForm();
                


            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }

                cmdSelContact.NavigateUrl = Utils.PopupPeople(this, txtContactId.ClientID, txtContactName.ClientID, PeopleInfo.PeopleTypeContact, transmittalInfo.Project.BusinessUnit);
                

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            try
            {
                //if (ddlDevilvery.SelectedItem.Value == "DLS" || ddlDevilvery.SelectedItem.Value == "EM")
                //{ ClearDistributionList(); }

                FormToObjects();
                transmittalInfo.Id = ProjectsController.GetInstance().AddUpdateTransmittal(transmittalInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

             Response.Redirect("~/Modules/Projects/ViewTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (transmittalInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + transmittalInfo.Project.IdStr);
            else
                Response.Redirect("~/Modules/Projects/ViewTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr);
        }


        //#-----------------For Gridview----------------------------------------

        #region SanGridView Methods


        protected String GvDrawingsSortExpression
        {
            get { return (String)ViewState["GvDrawingsSortExpression"]; }
            set { ViewState["GvDrawingsSortExpression"] = value; }
        }

        protected SortDirection GvDrawingsSortDireccion
        {
              get { return (SortDirection)ViewState["GvDrawingsSortDirection"]; }
                set { ViewState["GvDrawingsSortDirection"] = value; }
        }


        private void BindDrawings()
        {
            DataTable dataTable=null;
            DataRow dataRow=null;
            List<TransmittalRevisionInfo> LsttransmittalRevisionInfo = null;
            List<DrawingInfo> LstdrawingInfo = null;
            TransmittalRevisionInfo transmittalRevisionInfo = null;
            ProjectsController projectController = ProjectsController.GetInstance();

            String sortExpresion = GvDrawingsSortExpression;
            SortDirection sortDirection = GvDrawingsSortDireccion;

            if (transmittalInfo.Project.Drawings != null)
            {
                dataTable = new DataTable();
                dataTable.Columns.Add("IdStr");
                dataTable.Columns.Add("DrawingType");
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("LastRevisionIdStr");
                dataTable.Columns.Add("LastRevisionNumber");
                dataTable.Columns.Add("LastRevisionDate",typeof(DateTime));
                dataTable.Columns.Add("NumberOfCopies");
                dataTable.Columns.Add("FileSize");
                dataTable.Columns.Add("CssClass");
                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };

                LsttransmittalRevisionInfo = projectController.GetTransmittalRevisions(transmittalInfo);
                LstdrawingInfo = transmittalInfo.Project.Drawings;




                foreach (DrawingInfo drwInfo in transmittalInfo.Project.Drawings)
                {
                   

                    dataRow = dataTable.NewRow();


                    dataRow["IdStr"] = drwInfo.IdStr;
                    dataRow["DrawingType"] = drwInfo.DrawingType.Name;
                    dataRow["Name"] = drwInfo.Name;
                    dataRow["LastRevisionIdStr"] = drwInfo.LastRevisionIdStr;
                    dataRow["LastRevisionNumber"] = drwInfo.LastRevisionNumber;
                    if(drwInfo.LastRevisionDate!=null)//#---
                    dataRow["LastRevisionDate"] = drwInfo.LastRevisionDate;
                    if(LsttransmittalRevisionInfo!=null)
                    {
                        transmittalRevisionInfo = LsttransmittalRevisionInfo.Find(x => x.DrawingIdStr == drwInfo.IdStr);
                        if(transmittalRevisionInfo!=null)
                        dataRow["NumberOfCopies"] = transmittalRevisionInfo.NumCopies;
                    }
                    



                    dataRow["FileSize"] = "";
                    dataRow["CssClass"] = "lstTextNormal";
                    dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };

                    dataTable.Rows.Add(dataRow);
                }

                DataView dataView = new DataView(dataTable);
                gvDrawings.DataSource = dataView;
                gvDrawings.DataBind();
                Utils.BindSortedGrid(gvDrawings, dataTable, sortExpresion, sortDirection);

                 //--- RegisterScriptDrawings(dataTable);

                }



        }

        protected void gvDrawings_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDrawings.PageIndex = e.NewPageIndex;
            BindDrawings();
        }

        protected void gvDrawings_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvDrawings.PageIndex = 0;

            if (GvDrawingsSortExpression == e.SortExpression)
                GvDrawingsSortDireccion = Utils.ChangeSortDirection(GvDrawingsSortDireccion);
            else
            {
                GvDrawingsSortExpression = e.SortExpression;
                GvDrawingsSortDireccion = GvDrawingsSortExpression == "LastRevisionDate" ? SortDirection.Descending : SortDirection.Ascending;
            }

            BindDrawings();
        }

        protected void gvDrawings_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            
            Utils.SortedGridSetOrderImage(gvDrawings, e, GvDrawingsSortExpression, GvDrawingsSortDireccion);
        }

        protected void gvDrawings_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            CheckBox chkParentSelect,chkSelect;
            TextBox TxtNumCopies;
            //DataRow dataRow;
            //string strcheckParentId, strchkSelect;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                chkParentSelect = e.Row.FindControl("chkSelectAll") as CheckBox;
                
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                chkSelect = e.Row.FindControl("chkSelect") as CheckBox;

                TxtNumCopies= e.Row.FindControl("TxtNumberOfCopies") as TextBox;
                if (chkSelect.Visible && TxtNumCopies.Text.Length>0)
                {
                    chkSelect.Checked = true;
                   
                }
               
            }

          
        }



        private void RegisterScriptDrawings( DataTable dt)
        {
            String strScript = String.Format(@"
                <script language=""javascript"" type=""text/javascript"">
                var maxSize = {0};
                var arrayLinks = new Array({1});
                var arraySizes = new Array({2});
                var arrayIds = new Array({3});
                var lnkDownloadId = ""#{4}"";
                var lblErrorId = ""#{5}"";
                var lblSelectedId = ""#{6}"";
                var lblSizeId = ""#{7}"";
                var checkAllId = ""#{8}"";
                var lnkDownloadURL = ""{9}"";
                var totalSize;
                var numFiles;
                var Ids;
                var IsChecked;

                $(lnkDownloadId).click(function() {{ alert(""Please wait while your download starts. It can take up to a minute after clicking Ok.""); }});

                function UpdateDownloadInfo()
                {{
                    totalSize = 0;
                    numFiles = 0;
                    Ids = """";
                    for (var i = 0; i < arrayLinks.length; i++)
                    {{
                        if($(arrayLinks[i]).prop(""checked""))
                        {{
                            Ids = Ids == """" ? arrayIds[i] : Ids + "","" + arrayIds[i];
                            totalSize = totalSize + arraySizes[i];
                            numFiles++;
                        }}
                    }}

                    if (numFiles > 1)
                    {{
                        if (totalSize <= maxSize)
                        {{
                            $(lnkDownloadId).prop(""href"", lnkDownloadURL + Ids);
                            $(lnkDownloadId).prop(""title"", ""Download"");
                            $(lnkDownloadId).show(""slow"")
                            $(lblErrorId).hide(""slow"");
                        }}
                        else
                        {{
                            $(lnkDownloadId).hide(""slow"");
                            $(lblErrorId).show(""slow"");
                        }}
                        
                        $(lblSelectedId).text(numFiles);
                        $(lblSizeId).text(FormatFileSize(totalSize));
                    }}
                    else
                    {{
                        $(lnkDownloadId).hide(""slow"");
                        $(lblErrorId).hide(""slow"");
                        $(lblSelectedId).text("""");
                        $(lblSizeId).text("""");
                        $(lnkDownloadId).prop(""href"", """");
                        $(lnkDownloadId).prop(""title"", """");
                    }}
                }}

                function SelectAllDrawings()
                {{
                    IsChecked = $(checkAllId).prop(""checked"");

                    for (var i = 0; i < arrayLinks.length; i++) 
                        $(arrayLinks[i]).prop(""checked"", IsChecked);

                    UpdateDownloadInfo();
                }}

                function FormatFileSize(fileSize)
                {{
                    var fileSizeKb = fileSize / 1024;
                    return fileSizeKb < 1024 ? fileSizeKb.toFixed(2) + "" KB"" : (fileSizeKb / 1024).toFixed(2) + "" MB"";
                }}

                function InitializeDownloadInfo()
                {{
                    $(lnkDownloadId).hide();
                    $(lblErrorId).hide();
                }}
                </script>", dt);
        }




        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)gvDrawings.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in gvDrawings.Rows)
            {
                ((CheckBox)row.FindControl("chkSelect")).Checked = ChkBoxHeader.Checked;
            }
        }


        #endregion


        //#---------------------------------------------------------





        #endregion

     

    }
}
