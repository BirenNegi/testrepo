using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowContractPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ContractInfo contractInfo = null;
            String parameterContractId;
            Byte[] Rptpdf = null;
            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameterWithSection("ContractId");
                contractInfo = contractsController.GetContractWithModifications(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");

                if (!contractInfo.IsApproved)
                    throw new Exception("The contract can not be printed. It has has not been approved.");

                //#-----

                Rptpdf = UI.Utils.HtmlToPDF(contractsController.BuildContractWithModifications(contractInfo), contractsController.GetTemplateFooterText(contractInfo.Template));


                //#------
            }
            catch (Exception Ex)
            {


                Response.Write("#-------------------");
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(Rptpdf, "Contract");
          
           //#------ Utils.SendPdfData(UI.Utils.HtmlToPDF(contractsController.BuildContractWithModifications(contractInfo), contractsController.GetTemplateFooterText(contractInfo.Template)), "Contract");


        }
#endregion
        
    }
}