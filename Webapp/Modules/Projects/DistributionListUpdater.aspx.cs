using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace SOS.Web
{
    public partial class DistributionListUpdater : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //To Get only Active Projects
            List<ProjectInfo> ProjectList = ProjectsController.GetInstance().ListActiveProjects();
            ProjectInfo Pjct = null;
            ClientContactInfo clientContactInfo = null;
            int? Id = 0;
            try
            {
                foreach (ProjectInfo Project in ProjectList)
                {
                    Pjct = ProjectsController.GetInstance().GetProject(Project.Id);

                    //To create Client Contact
                    if (Pjct.ClientContact.FirstName != null && Pjct.ClientContact.LastName != null)
                    {
                        Pjct.ClientContact.Position = "Contact";
                        Id = PeopleController.GetInstance().AddUpdatePerson(Pjct.ClientContact);
                        //To Add Client Access
                        PeopleController.GetInstance().AddClientAccess(Id.Value, Project.Id.Value);
                        //To Update CLient Access
                        clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPersonById(Id.Value);
                        //clientContactInfo.Id = Id.Value;
                        clientContactInfo.SendEOTs = Pjct.SendEOTToClientContact;
                        clientContactInfo.SendRFIs = Pjct.SendRFIToClientContact;
                        clientContactInfo.SendClaims = Pjct.SendPCToClientContact;
                        clientContactInfo.SendSAs = Pjct.SendSAToClientContact;
                        clientContactInfo.SendCVs = Pjct.SendCVToClientContact;
                        clientContactInfo.AttentionToEots = true;
                        clientContactInfo.AttentionToRFIs = true;
                        clientContactInfo.AttentionToClaims = true;
                        PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo, Project.Id.Value);

                        updateTransmittalsClientContacts(Project.Id.Value, Id, "sendClientContact");


                        //PeopleController.GetInstance().UpdateClientDistAccess(Id.Value, Project.Id.Value,
                        //                                                Pjct.SendEOTToClientContact,
                        //                                                Pjct.SendRFIToClientContact,
                        //                                                Pjct.SendPCToClientContact,
                        //                                                Pjct.SendSAToClientContact,
                        //                                                Pjct.SendCVToClientContact, true, true, true);

                    }



                    //To create Client Contact1
                    if (Pjct.ClientContact1.FirstName != null && Pjct.ClientContact1.LastName != null)
                    {
                        Pjct.ClientContact1.Position = "Contact";

                        Id = PeopleController.GetInstance().AddUpdatePerson(Pjct.ClientContact1);
                        //To Add Client Access
                        PeopleController.GetInstance().AddClientAccess(Id.Value, Project.Id.Value);
                        //To Update CLient Access
                        clientContactInfo.Id = Id.Value;
                        clientContactInfo.SendEOTs = Pjct.SendEOTToClientContact1;
                        clientContactInfo.SendRFIs = Pjct.SendRFIToClientContact1;
                        clientContactInfo.SendClaims = Pjct.SendPCToClientContact1;
                        clientContactInfo.SendSAs = Pjct.SendSAToClientContact1;
                        clientContactInfo.SendCVs = Pjct.SendCVToClientContact1;
                        clientContactInfo.AttentionToEots = false;
                        clientContactInfo.AttentionToRFIs = false;
                        clientContactInfo.AttentionToClaims = false;
                        PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo, Project.Id.Value);

                        updateTransmittalsClientContacts(Project.Id.Value, Id, "sendClientContact1");
                        //PeopleController.GetInstance().UpdateClientDistAccess(Id.Value, Project.Id.Value,
                        //                                                Pjct.SendEOTToClientContact1,
                        //                                                Pjct.SendRFIToClientContact1,
                        //                                                Pjct.SendPCToClientContact1,
                        //                                                Pjct.SendSAToClientContact1,
                        //                                                Pjct.SendCVToClientContact1, false, false, false);
                    }

                    //To create Client Contact2
                    if (Pjct.ClientContact2.FirstName != null && Pjct.ClientContact2.LastName != null)
                    {
                        Pjct.ClientContact2.Position = "Contact";
                        Id = PeopleController.GetInstance().AddUpdatePerson(Pjct.ClientContact2);
                        //To Add Client Access
                        PeopleController.GetInstance().AddClientAccess(Id.Value, Project.Id.Value);
                        //To Update CLient Access
                        clientContactInfo.Id = Id.Value;
                        clientContactInfo.SendEOTs = Pjct.SendEOTToClientContact2;
                        clientContactInfo.SendRFIs = Pjct.SendRFIToClientContact2;
                        clientContactInfo.SendClaims = Pjct.SendPCToClientContact2;
                        clientContactInfo.SendSAs = Pjct.SendSAToClientContact2;
                        clientContactInfo.SendCVs = Pjct.SendCVToClientContact2;
                        clientContactInfo.AttentionToEots = false;
                        clientContactInfo.AttentionToRFIs = false;
                        clientContactInfo.AttentionToClaims = false;
                        PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo, Project.Id.Value);

                        updateTransmittalsClientContacts(Project.Id.Value, Id, "sendClientContact2");


                        //PeopleController.GetInstance().UpdateClientDistAccess(Id.Value, Project.Id.Value,
                        //                                                Pjct.SendEOTToClientContact2,
                        //                                                Pjct.SendRFIToClientContact2,
                        //                                                Pjct.SendPCToClientContact2,
                        //                                                Pjct.SendSAToClientContact2,
                        //                                                Pjct.SendCVToClientContact2, false, false, false);

                    }

                    //To create Client Superintendent
                    if (Pjct.Superintendent.FirstName != null && Pjct.Superintendent.LastName != null)
                    {
                        Pjct.Superintendent.Position = "Superintendent";
                        Id = PeopleController.GetInstance().AddUpdatePerson(Pjct.Superintendent);
                        //To Add Client Access
                        PeopleController.GetInstance().AddClientAccess(Id.Value, Project.Id.Value);
                        //To Update CLient Access
                        clientContactInfo.Id = Id.Value;
                        clientContactInfo.SendEOTs = Pjct.SendEOTToSuperintendent;
                        clientContactInfo.SendRFIs = Pjct.SendRFIToSuperintendent;
                        clientContactInfo.SendClaims = Pjct.SendPCToSuperintendent;
                        clientContactInfo.SendSAs = Pjct.SendSAToSuperintendent;
                        clientContactInfo.SendCVs = Pjct.SendCVToSuperintendent;
                        clientContactInfo.AttentionToEots = false;
                        clientContactInfo.AttentionToRFIs = false;
                        clientContactInfo.AttentionToClaims = false;
                        PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo, Project.Id.Value);

                        updateTransmittalsClientContacts(Project.Id.Value, Id, "sendSuperintendent");

                        //PeopleController.GetInstance().UpdateClientDistAccess(Id.Value, Project.Id.Value,
                        //                                                Pjct.SendEOTToSuperintendent,
                        //                                                Pjct.SendRFIToSuperintendent,
                        //                                                Pjct.SendPCToSuperintendent,
                        //                                                Pjct.SendSAToSuperintendent,
                        //                                                Pjct.SendCVToSuperintendent, false, false, false);

                    }

                    //To create Client Surveyor
                    if (Pjct.QuantitySurveyor.FirstName != null && Pjct.QuantitySurveyor.LastName != null)
                    {
                        Pjct.QuantitySurveyor.Position = "Surveyor";
                        Id = PeopleController.GetInstance().AddUpdatePerson(Pjct.QuantitySurveyor);
                        //To Add Client Access
                        PeopleController.GetInstance().AddClientAccess(Id.Value, Project.Id.Value);
                        //To Update CLient Access
                        clientContactInfo.Id = Id.Value;
                        clientContactInfo.SendEOTs = Pjct.SendEOTToQuantitySurveyor;
                        clientContactInfo.SendRFIs = Pjct.SendRFIToQuantitySurveyor;
                        clientContactInfo.SendClaims = Pjct.SendPCToQuantitySurveyor;
                        clientContactInfo.SendSAs = Pjct.SendSAToQuantitySurveyor;
                        clientContactInfo.SendCVs = Pjct.SendCVToQuantitySurveyor;
                        clientContactInfo.AttentionToEots = false;
                        clientContactInfo.AttentionToRFIs = false;
                        clientContactInfo.AttentionToClaims = false;
                        PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo, Project.Id.Value);

                        updateTransmittalsClientContacts(Project.Id.Value, Id, "sendQuantitySurveyor");

                        //PeopleController.GetInstance().UpdateClientDistAccess(Id.Value, Project.Id.Value,
                        //                                                Pjct.SendEOTToQuantitySurveyor,
                        //                                                Pjct.SendRFIToQuantitySurveyor,
                        //                                                Pjct.SendPCToQuantitySurveyor,
                        //                                                Pjct.SendSAToQuantitySurveyor,
                        //                                                Pjct.SendCVToQuantitySurveyor, false, false, false);

                    }

                }
            }
            catch (Exception ex)
            {

                string X = ex.ToString();

            }




        }



        private void updateTransmittalsClientContacts(int ProjectId, int? Id, string clintcontact)
        {
            string strSql = ConfigurationManager.ConnectionStrings["SOSTestConnectionString"].ToString();
            SqlConnection SQlCon = new SqlConnection(strSql);
            try
            {
                SQlCon.Open();
                string strSQL = @"Insert into [TransmittalsClientContact](TransmittalId,ProjectId,PeopleId)
                                  SELECT TransmittalId,ProjectId,'" + Id + @"'
                                  FROM Transmittals where projectid='" + ProjectId + @"' and " + clintcontact + @"=1
                                 ";
                SqlCommand SqlCom = new SqlCommand(strSQL, SQlCon);
                SqlCom.ExecuteNonQuery();


            }

            catch (Exception ex)
            {

                throw new Exception("Error on   -->" + ex.ToString());
            }
            finally
            {
                if (SQlCon.State == ConnectionState.Open)
                {
                    SQlCon.Close();
                }

            }

        }




    }
}