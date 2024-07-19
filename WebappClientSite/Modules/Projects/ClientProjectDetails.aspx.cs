using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using System.Drawing;

namespace SOS.Web
{
    public partial class DashBoardDetails : SOSPage
    {
        #region Members
        private ClientContactInfo clientContactInfo = null;
        private ProjectInfo projectInfo = null;
        #endregion

        #region privatemethods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = projectInfo.Name;


            return currentNode;
        }

        #endregion


        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            try { 
                    Security.CheckAccess(Security.userActions.ClientProjectDetails);
                    ProjectsController projectsController = ProjectsController.GetInstance();
                    clientContactInfo = ((ClientContactInfo)Web.Utils.GetCurrentUser());
                    int? projectId;

                        if (HttpContext.Current.Request.Params["ProjectId"] == null)
                        {
                            projectInfo = (ProjectInfo)HttpContext.Current.Session["CurrentProject"];
                            projectId = projectInfo.Id;
                            if (projectInfo == null)
                                return;
                        }
                        else
                         projectId = Data.Utils.GetDBInt32(Request.QueryString["ProjectId"]);

                    if (!IsPostBack)
                    { 
                                if (clientContactInfo != null)
                                        clientContactInfo.Projects = PeopleController.GetInstance().GetClientProjects(clientContactInfo.Id);


                                if (projectId != 0)
                                    projectInfo = ProjectsController.GetInstance().GetProject(projectId);

                                else if (clientContactInfo.Projects.Count == 1)
                                    projectInfo = clientContactInfo.Projects[0];


                                if (projectInfo !=null)
                                        HttpContext.Current.Session.Add("CurrentProject", projectInfo);

                   

                    hlProjectHeader.Text = projectInfo.Name + " | " + projectInfo.FullNumber;



                                clientContactInfo= PeopleController.GetInstance().GetClientAccessOnProject(clientContactInfo, projectInfo.Id);

                                if ((Boolean)clientContactInfo.WebAccessToDocs)
                                    lnkDrawingsTransmittals.NavigateUrl = "~/Modules/Projects/ListDrawingsTransmittals.aspx?ProjectId=" + projectInfo.IdStr;
                                else lnkDrawingsTransmittals.Visible = false;

                                if ((Boolean)clientContactInfo.WebAccessToCVs)
                                    lnkClientVariations.NavigateUrl = "~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + ClientVariationInfo.VariationTypeClient;
                                else lnkClientVariations.Visible = false;

                                if ((Boolean)clientContactInfo.WebAccessToSAs)
                                    lnkSeparateAccounts.NavigateUrl = "~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + ClientVariationInfo.VariationTypeSeparateAccounts;
                                else lnkSeparateAccounts.Visible = false;

                                if ((Boolean)clientContactInfo.WebAccessToClaims)
                                    lnkClaims.NavigateUrl = "~/Modules/Projects/ListClaims.aspx?ProjectId=" + projectInfo.IdStr;
                                else lnkClaims.Visible = false;
                                 //--To hide progress claim under Overview scection 
                                if (!lnkClaims.Visible)
                                    TblClaim.Attributes.Add("style","display:none");


                                if ((Boolean)clientContactInfo.WebAccessToPhotos)
                                    lnkGallery.NavigateUrl = "~/Modules/Projects/Photogallery.aspx?ProjectId=" + projectInfo.IdStr;
                                else lnkGallery.Visible = false;


                                if ((Boolean)clientContactInfo.WebAccessToRFIs)
                                    lnkRFIs.NavigateUrl = "~/Modules/Projects/ListRFIs.aspx?ProjectId=" + projectInfo.IdStr;
                                else lnkRFIs.Visible = false;
                                if ((Boolean)clientContactInfo.WebAccessToEOTs)
                                    lnkEOTs.NavigateUrl = "~/Modules/Projects/ListEOTs.aspx?ProjectId=" + projectInfo.IdStr;
                                else lnkEOTs.Visible = false;
                                //link to meeting minutes
                                lnkMeetingminutes.NavigateUrl= "~/Modules/Projects/ListMeetingMinutes.aspx?ProjectId=" + projectInfo.IdStr;
                }



                if (projectId == 0)
                    projectInfo =(ProjectInfo)HttpContext.Current.Session["CurrentProject"];
                if (projectInfo == null)
                    return;
                else
                   SqlDataSource1.SelectCommand = @"SELECT  [Number], [Name], [Street], [Locality], [State], [PostalCode],[DefectsLiability],[SiteAllowances],[LiquidatedDamages],[RetentionToCertification], [Retention],[RetentionToDLP],convert(varchar(11),[CompletionDate],103) as [CompletionDate], convert(varchar(11),[CommencementDate],103) as CommencementDate 
                                                    ,CAPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=CAPeopleId)as CAName
                                                    ,PMPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=PMPeopleId)as PMName
                                                    ,CMPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=CMPeopleId)as CMName
                                                    ,DMPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=DMPeopleId)as DMName
                                                    ,COPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=COPeopleId)as COMName
                                                    ,FCPeopleId,(select Firstname+' '+ LastName  from People where PeopleId=FCPeopleId)as FCName
                                                    ,ProjectId
                                                    FROM [Projects] WHERE ([ProjectId] = " + projectInfo.Id + ")";
                    DataList1.DataSource = SqlDataSource1;
                    DataList1.DataBind();

                    DataList2.DataSource = SqlDataSource1;
                    DataList2.DataBind();


                projectInfo.EOTs = projectsController.GetEOTs(projectInfo);
                projectInfo.ClaimedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithClaimedEOTs(projectInfo);
                projectInfo.ApprovedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithApprovedEOTs(projectInfo);
                //----EOTs-----------------
                LblCommencementDate.Text= UI.Utils.SetFormDate(projectInfo.CommencementDate); 
                LblCompletionDate.Text= UI.Utils.SetFormDate(projectInfo.CompletionDate);
                LblClaimedEots.Text = UI.Utils.SetFormDate(projectInfo.ClaimedEOTsCompletionDate);
                LblApprovedEots.Text = UI.Utils.SetFormDate(projectInfo.ApprovedEOTsCompletionDate);

                //-------Claims--------------
                Decimal? totalClientVariations = 0;
                Decimal? totalClaimsApproved = 0;
                int i=1;
                projectInfo = projectsController.GetProjectWithClientVariations(projectId,"CV");

                projectInfo.Claims = projectsController.GetClaimsWithTradesAndVariations(projectInfo);

                projectInfo.RFIs = projectsController.GetRFIs(projectInfo);

                if (projectInfo.ClientVariations != null)
                    foreach (ClientVariationInfo clientVariation in projectInfo.ClientVariations)
                    {
                         if (clientVariation != null)
                            if (clientVariation.TotalAmount != null && clientVariation.IsApproved)
                                totalClientVariations += (Decimal)clientVariation.TotalAmount;
                    }

                if (projectInfo.Claims.Count != 0)
                    foreach (ClaimInfo claimInfo in projectInfo.Claims)
                    {
                        if (projectInfo.Claims[projectInfo.Claims.Count - i].IsApproved)
                        {
                            totalClaimsApproved = projectInfo.Claims[projectInfo.Claims.Count - i].Total;
                            break;
                        }
                        i++;
                    }

                LblContractAmt.Text= UI.Utils.SetFormDecimal(projectInfo.ContractAmount);
                LblVariationAmount.Text = UI.Utils.SetFormDecimal(totalClientVariations);
                LblRevisedContractSum.Text = UI.Utils.SetFormDecimal(projectInfo.ContractAmount + totalClientVariations);
                LblTotalLastClaimed.Text = UI.Utils.SetFormDecimal(totalClaimsApproved);
                //----------RFIs--------

                LblTotalRFIs.Text = projectInfo.RFIs.Count.ToString();
                LblRepliedRFIs.Text = projectInfo.RFIs.FindAll(x => x.Status == "R").Count.ToString();
                LblPendingRFIs.Text = projectInfo.RFIs.FindAll(x => x.Status != "R").Count.ToString();


            }
            
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

    //Label1.Width = 150;
    //Label1.Text = "4";
    //Label1.BackColor = Color.Red;

    //Label2.Width = 50;
    //Label2.Text = "3";
    //Label2.BackColor = Color.Orange;

    //Label3.Width = 300;
    //Label3.Text = "6";
    //Label3.BackColor = Color.Green;


    //Label4.Width = 250;
    //Label4.Text = "7";
    //Label4.BackColor = Color.Red;

    //Label5.Width = 150;
    //Label5.Text = "3";
    //Label5.BackColor = Color.Orange;

    //Label6.Width = 100;
    //Label6.Text = "1";
    //Label6.BackColor = Color.Green;


    //Label7.Width = 0;
    //Label7.Text = "";
    //Label7.BackColor = Color.Red;

    //Label8.Width = 150;
    //Label8.Text = "3";
    //Label8.BackColor = Color.Orange;

    //Label9.Width = 350;
    //Label9.Text = "5";
    //Label9.BackColor = Color.Green;



}


        #endregion




    }
}