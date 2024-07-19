using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListContractTemplatesPage : System.Web.UI.Page
    {

#region Private Methods
        private void bindContractTemplates()
        {
            HtmlTable theTable = new HtmlTable();
            HtmlTable aTable;
            HtmlTableRow aRow;
            HtmlTableCell aCell;
            HtmlTableRow buRow;
            HtmlAnchor lnkStandard;
            HtmlAnchor lnkSimplified;
            HtmlAnchor lnkVariation;
            String rowStyle;

            ContractTemplateInfo currContractTemplateInfo;
            ContractsController contractsController = ContractsController.GetInstance();

            List<ContractTemplateInfo> contractTemplateInfoList = contractsController.GetContractTemplates();
            List<BusinessUnitInfo> businessUnitInfoList = contractsController.GetBusinessUnits();
            List<JobTypeInfo> jobTypeInfoList = contractsController.GetJobTypes();

            aRow = new HtmlTableRow();
            aCell = new HtmlTableCell();
            aCell.InnerHtml = "&nbsp;";
            aRow.Cells.Add(aCell);
            rowStyle = "lstItem";
            foreach (JobTypeInfo jobTypeInfo in jobTypeInfoList)
            {
                aCell = new HtmlTableCell();
                aCell.Attributes.Add("Class", "lstHeader");
                aCell.Attributes.Add("Align", "Center");
                aCell.InnerText = UI.Utils.SetFormString(jobTypeInfo.Name);
                aRow.Cells.Add(aCell);
            }
            theTable.Rows.Add(aRow);
            
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
            {
                buRow = new HtmlTableRow();
                aCell = new HtmlTableCell();
                aCell.InnerText = UI.Utils.SetFormString(businessUnitInfo.Name);
                aCell.Attributes.Add("Class", "lstHeaderTop");
                aCell.Attributes.Add("Align", "Right");
                buRow.Cells.Add(aCell);

                foreach (JobTypeInfo jobTypeInfo in jobTypeInfoList)
                {
                    currContractTemplateInfo = null;
                    foreach (ContractTemplateInfo contractTemplateInfo in contractTemplateInfoList)
                    {
                        if (contractTemplateInfo.JobType != null && contractTemplateInfo.BusinessUnit != null) {
                            if (contractTemplateInfo.JobType.Id == jobTypeInfo.Id && contractTemplateInfo.BusinessUnit.Id == businessUnitInfo.Id)
                            {
                                currContractTemplateInfo = contractTemplateInfo;
                                break;
                            }
                        }
                    }

                    lnkStandard = new HtmlAnchor();
                    lnkSimplified = new HtmlAnchor();
                    lnkVariation = new HtmlAnchor();

                    if (currContractTemplateInfo != null)
                    {
                        lnkStandard.HRef = "~/Modules/Contracts/ViewContractTemplate.aspx?Type=Std&ContractTemplateId=" + currContractTemplateInfo.IdStr;
                        //lnkSimplified.HRef = "~/Modules/Contracts/ViewContractTemplate.aspx?Type=Sim&ContractTemplateId=" + currContractTemplateInfo.IdStr;  DS20230927
                        lnkVariation.HRef = "~/Modules/Contracts/ViewContractTemplate.aspx?Type=Var&ContractTemplateId=" + currContractTemplateInfo.IdStr;

                        lnkStandard.InnerText = currContractTemplateInfo.StandardTemplate != null ? "Std" : "Std(*)";
                        //lnkSimplified.InnerText = currContractTemplateInfo.SimplifiedTemplate != null ? "Sim" : "Sim(*)";  DS20230927
                        lnkVariation.InnerText = currContractTemplateInfo.VariationTemplate != null ? "Var" : "Var(*)";
                    }
                    else
                    {
                        lnkStandard.HRef = "~/Modules/Contracts/EditContractTemplate.aspx?Type=Std&BusinessUnitId=" + businessUnitInfo.IdStr + "&JobTypeId=" + jobTypeInfo.IdStr;
                        //lnkSimplified.HRef = "~/Modules/Contracts/EditContractTemplate.aspx?Type=Sim&BusinessUnitId=" + businessUnitInfo.IdStr + "&JobTypeId=" + jobTypeInfo.IdStr;  DS20230927
                        lnkVariation.HRef = "~/Modules/Contracts/EditContractTemplate.aspx?Type=Var&BusinessUnitId=" + businessUnitInfo.IdStr + "&JobTypeId=" + jobTypeInfo.IdStr;

                        lnkStandard.InnerText = "Std(?)";
                        //lnkSimplified.InnerText = "Sim(?)";
                        lnkVariation.InnerText = "Var(?)";
                    }

                    lnkStandard.Attributes.Add("Class", "frmLink");
                    //lnkSimplified.Attributes.Add("Class", "frmLink");  DS20230927
                    lnkVariation.Attributes.Add("Class", "frmLink");

                    aRow = new HtmlTableRow();

                    aCell = new HtmlTableCell();
                    aCell.Controls.Add(lnkStandard);
                    aRow.Cells.Add(aCell);
                    aCell = new HtmlTableCell();
                    aCell.InnerHtml = "&nbsp;";
                    aRow.Cells.Add(aCell);

                    aCell = new HtmlTableCell();
                    aCell.Controls.Add(lnkSimplified);
                    aRow.Cells.Add(aCell);
                    aCell = new HtmlTableCell();
                    aCell.InnerHtml = "&nbsp;";
                    aRow.Cells.Add(aCell);

                    aCell = new HtmlTableCell();
                    aCell.Controls.Add(lnkVariation);
                    aRow.Cells.Add(aCell);

                    aTable = new HtmlTable();
                    aTable.Rows.Add(aRow);
                    
                    aCell = new HtmlTableCell();
                    aCell.Attributes.Add("Class", rowStyle);
                    aCell.Controls.Add(aTable);

                    buRow.Cells.Add(aCell);
                }

                rowStyle = rowStyle == "lstItem" ? "lstAltItem" : "lstItem";
                theTable.Rows.Add(buRow);
            }

            theTable.Border = 1;
            theTable.CellPadding = 2;
            theTable.CellSpacing = 0;
            phContratTemplases.Controls.Add(theTable);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListContractTemplates);

                if (!Page.IsPostBack)
                    bindContractTemplates();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
    
    }
}