using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowForemanContract : System.Web.UI.Page
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


                String Foremancontract = contractsController.BuildContractWithModifications(contractInfo);
                Foremancontract= Foremancontract.Replace("<!-- Regarding -->", " ");
                int len = Foremancontract.IndexOf("Contract Sum");
                int len2 = Foremancontract.IndexOf("Regarding");
                string X = Foremancontract.Substring(0, len);
                string Y = Foremancontract.Substring(len2);

                string custom = @"</td><td></td></tr></tbody></table> </td><td></td></tr></tbody></table> </td></tr>   <tr><td><table><tbody><tr><td class='SupSection'>";

                string NewForemancontract = X + custom + Y;


                string Footer = contractsController.GetTemplateFooterText(contractInfo.Template);

                Rptpdf = UI.Utils.HtmlToPDF(NewForemancontract, contractsController.GetTemplateFooterText(contractInfo.Template));


            }
            catch (Exception Ex)
            {

                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(Rptpdf, "ForemanContract");

            

        }
        #endregion
    }
}