using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

using ProcessStatus = SOS.Core.ProcessStatus;

namespace SOS.Web
{
    public partial class UpdateFilesLinksPage : System.Web.UI.Page
    {

#region Members
        private ProjectInfo projectInfo = null;
        private int updatedFiles = 0;
#endregion

#region Private Methods
        private void BindForm()
        {
            List<ProjectInfo> projectInfoList = projectInfoList = ProjectsController.GetInstance().ListProjects(); ;

            ddlProjects.Items.Add(new ListItem(String.Empty, String.Empty));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

        }

        private void BindProject()
        {
            BindProject(null);
        }

        private void BindProject(String sortExpresion)
        {
            List<Info> infoList = ProjectsController.GetInstance().GetProjectFiles(projectInfo.Id);
            DataSet dataSet = new DataSet("Project");
            TradeInfo tradeInfo;
            ContractInfo contractInfo;
            ClientVariationInfo clientVariationInfo;
            DrawingRevisionInfo drawingRevisionInfo;
            RFIInfo rFIInfo;
            EOTInfo eOTInfo;
            Int32 currentRecord = 0;

            dataSet.Tables.Add("File");
            dataSet.Tables[0].Columns.Add("No", typeof(String));
            dataSet.Tables[0].Columns.Add("Entity", typeof(String));
            dataSet.Tables[0].Columns.Add("Attachment", typeof(String));
            dataSet.Tables[0].Columns.Add("File", typeof(String));

            foreach (Info info in infoList)
            {
                if (info is TradeInfo)
                {
                    tradeInfo = (TradeInfo)info;

                    if (tradeInfo.QuotesFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Trade", "Quotes file", tradeInfo.QuotesFile });
                    }

                    if (tradeInfo.PrelettingFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Trade", "Preletting file", tradeInfo.PrelettingFile });
                    }

                    //#-----Signed Contract File

                    if (tradeInfo.SignedContractFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Trade", "Signed Contract file", tradeInfo.SignedContractFile });
                    }

                    //#-----------------------

                }
                else if (info is ContractInfo)
                {
                    contractInfo = (ContractInfo)info;

                    if (contractInfo.QuotesFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Contract", "Quotes file", contractInfo.QuotesFile });
                    }
                }

                else if (info is ClientVariationInfo)
                {
                    clientVariationInfo = (ClientVariationInfo)info;

                    if (clientVariationInfo.QuotesFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Client variation", "Quotes file", clientVariationInfo.QuotesFile });
                    }

                    if (clientVariationInfo.BackupFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Client variation", "Backup file", clientVariationInfo.BackupFile });
                    }

                    if (clientVariationInfo.ClientApprovalFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Client variation", "Client approval file", clientVariationInfo.ClientApprovalFile });
                    }
                }

                else if (info is DrawingRevisionInfo)
                {
                    drawingRevisionInfo = (DrawingRevisionInfo)info;

                    if (drawingRevisionInfo.File != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "Drawing revision", "Graphic file", drawingRevisionInfo.File });
                    }
                }

                else if (info is RFIInfo)
                {
                    rFIInfo = (RFIInfo)info;

                    if (rFIInfo.ReferenceFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "RFI", "Reference file", rFIInfo.ReferenceFile });
                    }

                    if (rFIInfo.ClientResponseFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "RFI", "Client response file", rFIInfo.ClientResponseFile });
                    }
                }

                else if (info is EOTInfo)
                {
                    eOTInfo = (EOTInfo)info;

                    if (eOTInfo.ClientApprovalFile != null)
                    {
                        currentRecord++;
                        dataSet.Tables[0].Rows.Add(new Object[] { UI.Utils.SetFormInteger(currentRecord), "EOF", "Client approval file", eOTInfo.ClientApprovalFile });
                    }
                }
            }

            DataView dataView = new DataView(dataSet.Tables[0]);

            if (sortExpresion != null)
                dataView.Sort = sortExpresion;

            gvFiles.DataSource = dataView;
            gvFiles.DataBind();

            pnlFiles.Visible = true;
        }

        private ProcessStatus CheckProcessStatus()
        {
            ProcessStatus processStatus = null;

            if (Session["MethodCallInfo"] != null)
            {
                IAsyncResult iAsyncResult = (IAsyncResult)Session["MethodCallInfo"];
                ProcessStatus runningProcessStatus = (ProcessStatus)iAsyncResult.AsyncState;

                if (iAsyncResult.IsCompleted)
                {
                    processStatus = runningProcessStatus.GetStatusUpdate();

                    try
                    {
                        updatedFiles = ProjectsController.GetInstance().EndUpdateProjectFiles(iAsyncResult);
                    }
                    catch (Exception Ex)
                    {
                        Utils.ProcessPageLoadException(this, Ex);
                    }
                    finally
                    {
                        Session["MethodCallInfo"] = null;
                        processStatus.IsComplete = true;
                    }
                }
                else
                {
                    processStatus = runningProcessStatus.GetStatusUpdate();
                }
            }

            return processStatus;
        }

        private void UpdateProgressBar(int percentage)
        {
            divProgressText.InnerText = percentage.ToString() + "%";
            divProgressBar.Attributes.Add("style", "background-color:#00CC00; position:absolute; top:0; left:0; width:" + percentage.ToString() + "%;");
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.UpdateLinks);

                if (ddlProjects.SelectedValue != String.Empty)
                    projectInfo = new ProjectInfo(Int32.Parse(ddlProjects.SelectedValue));
                else
                    projectInfo = null;

                if (!Page.IsPostBack)
                    BindForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectInfo != null)
            {
                gvFiles.PageIndex = 0;
                pnlProgress.Visible = false;
                txtCurrentPath.Text = String.Empty;
                txtNewPath.Text = String.Empty;
                BindProject();
            }
            else
                pnlFiles.Visible = false;
        }

        protected void gvFiles_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFiles.PageIndex = e.NewPageIndex;
            BindProject();
        }

        protected void gvFiles_OnSorting(object sender, GridViewSortEventArgs e)
        {
            BindProject(e.SortExpression + (e.SortDirection == SortDirection.Descending ? " Desc" : String.Empty));
        }
        
        protected void cmdUpdatePath_Click(object sender, EventArgs e)
        {
            String oldPath = UI.Utils.GetFormString(txtCurrentPath.Text);
            String newPath = UI.Utils.GetFormString(txtNewPath.Text);

            try
            {
                if (projectInfo != null && oldPath != null)
                {
                    IAsyncResult iAsyncResult = ProjectsController.GetInstance().StartUpdateProjectFiles(projectInfo, oldPath, newPath);
                    Session["MethodCallInfo"] = iAsyncResult;
                    cmdUpdatePath.Enabled = false;
                    pnlProgress.Visible = true;
                    UpdateProgressBar(0);
                    tmrProgress.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void tmrProgress_Tick(object sender, EventArgs e)
        {
            ProcessStatus processStatus = CheckProcessStatus();

            if (processStatus != null)
            {
                UpdateProgressBar(processStatus.PercentageCompletion);

                if (processStatus.IsComplete)
                {
                    tmrProgress.Enabled = false;
                    BindProject();
                    divProgressText.InnerText = divProgressText.InnerText + ". " + UI.Utils.SetFormInteger(updatedFiles) + " " + (updatedFiles == 1 ? "file" : "files") + " updated.";
                    cmdUpdatePath.Enabled = true;
                }
            }
            else
            {
                tmrProgress.Enabled = false;
                BindProject();
                divProgressText.InnerText = "Unknown status.";
                cmdUpdatePath.Enabled = true;
            }
        }
#endregion

    }
}
