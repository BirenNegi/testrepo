using System;

namespace SOS.Core
{
    public static class Security
    {

#region Constants
        public enum userActions
        {
            //Pending Tasks
            ViewPendingTasks,
            ViewPendingTasksIndividuals,
            //Contacts
            SearchContacts,
            SelectContact,
            ViewContac,
            EditContact,
            CreateContactAccount,

             // #---
             //Contract Approval
              EditContractApprovalLimit,
              KPIRangeEdit,
              
            // #-----

            //Employees
            SearchEmployees,
            SelectEmployee,
            ViewEmployee,
            EditEmployee,
            CreateEmployeeAccount,

            //Client Contacts--#
            EditClientAccess,
            CreateClientContactAccount,
            ClientProjects,
            ClientProjectDetails,
            Photogallery,
            //#---




          


            //SubContractors
            SearchSubContractors,
            SelectSubContractor,
            ViewSubContractor,
            EditSubContractor,

            //Job Types
            ListJobTypes,
            EditJobType,

            //Business Units
            ListBusinessUnits,
            EditBusinessUnit,

            //Drawing Types
            ListDrawingTypes,
            EditDrawingType,

            //Client Trade Types
            ListClientTradeTypes,
            EditClientTradeType,

            //Holidays
            ListHolidays,
            EditHolidays,

            //Holidays
            ListRDOs,
            EditRDOs,

            //Trade Templates
            ListTradeTemplates,
            ViewTradeTemplate,
            EditTradeTemplate,

            //Contract Templates
            ListContractTemplates,
            ViewContractTemplate,
            EditContractTemplate,

            //Minutes templates
            ListMinutesTemplates,
            ViewMinutesTemplate,
            EditMinutesTemplate,

            //Invitation templates
            ListInvitationTemplates,
            ViewInvitationTemplate,
            EditInvitationTemplate,

            //Drawings
            ListDrawings,
            ViewDrawing,
            EditDrawing,

            //Transmittals
            ViewTransmittal,
            EditTransmittal,

            //Contracts
            ListContracts,
            ViewContract,
            EditContract,

            //Trades
            ViewTrade,
            EditTrade,
            UpdateTargetDates,

            //Projects
            SearchProjects,
            ViewProject,
            EditProject,
            CreateProject,
            ViewProjectsDrawings,

            //Client Variations
            ListClientVariations,
            ViewClientVariation,
            EditClientVariation,

			//Claims
			ListClaims,
			ViewClaim,
			EditClaim,

            //RFIs
            ListRFIs,
            ViewRFI,
            EditRFI,

            //EOTs
            ListEOTs,
            ViewEOT,
            EditEOT,

            //Meeting minutes---#
            ListMeetingMinutes,
            ViewMeetingMinutes,
            EditMeetingMinutes,


            //Participations
            ViewParticipation,
            EditParticipation,
            SearchParticipationsSubContractor,
            ViewParticipationSubContractor,
            EditParticipationSubContractor,

            //Addendums
            ViewAddendum,
            EditAddendum,

            //Change Password
            ChangePassword,

            //Select file
            SelectFile,
            SelectFolder,

            //Help
            ViewHelp,
            ViewSubContractorHelp,

            //Reports
            ViewReports,
            ViewAdminReports,
            ViewBudgetReports,

            //UpdateLinks
            UpdateLinks,

            //Site Orders
            SearchSiteOrders,
            EditSiteOrder,
            EditSiteOrderNS,
            ViewSiteOrder,
            SearchSiteOrderDetail,
            EditSiteOrderDetail,
            SearchSiteOrderDocs,
            EditSiteOrderDocs,
            EditSiteOrderApprovals,
            EditSiteOrderApprovalsAll,
            EditSiteOrderHire,
            EditSiteOrderHireNS,
            ViewSiteOrderHire,
            SearchSiteOrderDetailHire,
            EditSiteOrderDetailHire,
            EditSiteOrderInst,
            EditSiteOrderInstNS,
            ViewSiteOrderInst,
            SearchSiteOrderDetailInst,
            EditSiteOrderDetailInst,
            SelectSiteOrderSub,

        }
        #endregion

        #region Public Static Methods
        // Checks if the current user is an admin  and User is Employee
        public static bool IsAdmin()
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user != null)
                return user.UserType == EmployeeInfo.TypeAdmin;
            else
                return false;
        }


        //#---13/01/2020--  Checks if the current user is an Subcontractor contact and is an Admin
        public static bool IsSubcontractorAdmin()
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user != null)
                return (user.Type==PeopleInfo.PeopleTypeContact && user.UserType == ContactInfo.SubcontractorAdmin);
            else
                return false;
        }

        //#--- 13/01/2020



        /// <summary>
        /// Verifies if there is an existing user logged in and if the user has permission to execute 
        /// the action.
        /// Exceptions:
        ///    SessionException  : No current user
        ///    SecurityException : The current user does not have credentials to perform the action
        /// </summary>
        /// <param name="action">The action to be performed</param>
        public static bool CheckAccess(userActions action, bool viewOnly)
        {
            bool allow = false;
            PeopleInfo user = Web.Utils.GetCurrentUser();
 
            if (user == null) throw new SessionException();

            {
                switch (action)
                {
                    // Pending Tasks
                    case userActions.ViewPendingTasks:
                        allow = user is EmployeeInfo; break; //&& (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector) ;

                    case userActions.ViewPendingTasksIndividuals:
                        allow = user is EmployeeInfo; break;

                    // Contacts
                    case userActions.SearchContacts:
                        //#-- allow = user is EmployeeInfo; break;
                        allow = user is EmployeeInfo|| ((user is ContactInfo) && (user.UserType == ContactInfo.SubcontractorAdmin));  break;//#---

                    case userActions.SelectContact:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewContac:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditContact:
                        //#--- allow = user is EmployeeInfo ; break
                        allow = (user is EmployeeInfo) || ((user is ContactInfo) && (user.UserType==ContactInfo.SubcontractorAdmin)); break;
                        //#---

                    case userActions.CreateContactAccount:
                        //#--allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;
                        allow = ((user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin))||((user is ContactInfo)&&(user.UserType==ContactInfo.SubcontractorAdmin)); break;
                    // Employees
                    case userActions.SearchEmployees:
                        allow = user is EmployeeInfo; break;
                    case userActions.SelectEmployee:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewEmployee:
                        allow = user is EmployeeInfo || user is ClientContactInfo;  break;//--#
                    case userActions.EditEmployee:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;
                    case userActions.CreateEmployeeAccount:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    //Client Contacts--#--
                    case userActions.EditClientAccess:
                        allow = user is EmployeeInfo; break;
                    case userActions.CreateClientContactAccount:
                        allow = user is EmployeeInfo; break;
                    case userActions.ClientProjects:
                        allow = user is ClientContactInfo;break;
                    case userActions.ClientProjectDetails:
                        allow = user is ClientContactInfo; break;
                    case userActions.Photogallery:
                        allow = user is ClientContactInfo; break;
                    //--san



                    // SubContractors
                    case userActions.SearchSubContractors:
                        allow = user is EmployeeInfo; break;
                    case userActions.SelectSubContractor:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewSubContractor:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditSubContractor:
                        allow = (user is EmployeeInfo); break;

                    // JobTypes
                    case userActions.ListJobTypes:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditJobType:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Business Units
                    case userActions.ListBusinessUnits:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditBusinessUnit:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    //#--
                    //Contract Approval Limit
                    case userActions.EditContractApprovalLimit:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;
                    //#


                    // Drawing Types
                    case userActions.ListDrawingTypes:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditDrawingType:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Client Trade Types
                    case userActions.ListClientTradeTypes:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditClientTradeType:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Holidays
                    case userActions.ListHolidays:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditHolidays:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // RDOs
                    case userActions.ListRDOs:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditRDOs:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Trade Templates
                    case userActions.ListTradeTemplates:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewTradeTemplate:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditTradeTemplate:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Contract Templates
                    case userActions.ListContractTemplates:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewContractTemplate:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditContractTemplate:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Minutes Templates
                    case userActions.ListMinutesTemplates:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewMinutesTemplate:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditMinutesTemplate:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Invitation Templates
                    case userActions.ListInvitationTemplates:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewInvitationTemplate:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditInvitationTemplate:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Drawings
                    case userActions.ListDrawings:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewDrawing:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditDrawing:
                        allow = user is EmployeeInfo; break;

                    // Transmittals
                    case userActions.ViewTransmittal:
                        allow = user is EmployeeInfo || user is ContactInfo ||user is ClientContactInfo; break; //#---
                    case userActions.EditTransmittal:
                        allow = user is EmployeeInfo; break;

                    // Contracts
                    case userActions.ListContracts:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewContract:
                        allow = user is EmployeeInfo || user is ContactInfo; break;
                    case userActions.EditContract:
                        allow = user is EmployeeInfo; break;

                    // Trades
                    case userActions.ViewTrade:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditTrade:
                        allow = user is EmployeeInfo; break;
                    case userActions.UpdateTargetDates:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector || user.UserType == EmployeeInfo.TypeConstructionsManager || user.UserType == EmployeeInfo.TypeProjectManager); break;

                    // Projects
                    case userActions.SearchProjects:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewProject:
                        allow = user is EmployeeInfo|| user is ClientContactInfo; break;//---#
                    case userActions.EditProject:
                        allow = (user is EmployeeInfo); break;
                    case userActions.CreateProject:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector || user.UserType == EmployeeInfo.TypeConstructionsManager); break;
                    case userActions.ViewProjectsDrawings:
                        allow = user is EmployeeInfo || user is ContactInfo|| user is ClientContactInfo; break;//--#

                    // ClientVariations
                    case userActions.ListClientVariations:
                        //#--allow = user is EmployeeInfo; break;
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    case userActions.ViewClientVariation:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;//--#
                    case userActions.EditClientVariation:
                        allow = user is EmployeeInfo; break;

                    // Claims
                    case userActions.ListClaims:
                        //#--allow = user is EmployeeInfo; break;
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    case userActions.ViewClaim:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;//--san
					case userActions.EditClaim:
                        allow = user is EmployeeInfo; break;

                    // RFIs
                    case userActions.ListRFIs:
                        //#--allow = user is EmployeeInfo; break;
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    case userActions.ViewRFI:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;//---#
                    case userActions.EditRFI:
                        allow = user is EmployeeInfo; break;

                    // EOTs
                    case userActions.ListEOTs:
                        //#--allow = user is EmployeeInfo; break;
                        allow = user is EmployeeInfo || user is ClientContactInfo;break;
                    case userActions.ViewEOT:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;//--san
                    case userActions.EditEOT:
                        allow = user is EmployeeInfo; break;


                    // MeetingMinutes ---#
                    case userActions.ListMeetingMinutes:
                         allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    case userActions.ViewMeetingMinutes:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    case userActions.EditMeetingMinutes:
                        allow = user is EmployeeInfo; break;


                    case userActions.KPIRangeEdit:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector ); break;


                    //---#

                    // Participations
                    case userActions.ViewParticipation:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditParticipation:
                        allow = user is EmployeeInfo; break;
                    case userActions.SearchParticipationsSubContractor:
                        allow = user is ContactInfo; break;
                    case userActions.ViewParticipationSubContractor:
                        allow = user is ContactInfo; break;
                    case userActions.EditParticipationSubContractor:
                        allow = user is ContactInfo; break;

                    // Addendums
                    case userActions.ViewAddendum:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditAddendum:
                        allow = user is EmployeeInfo; break;

                    // Participations
                    case userActions.ChangePassword:
                        //#--allow = user is EmployeeInfo || user is ContactInfo; break;
                        allow = user is EmployeeInfo || user is ContactInfo|| user is ClientContactInfo ; break;//#---
                    
                        
                        // Files
                    case userActions.SelectFile:
                        allow = user is EmployeeInfo; break;
                    // Folders
                    case userActions.SelectFolder:
                        allow = user is EmployeeInfo; break;

                    // Help
                    case userActions.ViewHelp:
                        allow = user is EmployeeInfo; break;

                    // Help
                    case userActions.ViewSubContractorHelp:
                        //#---allow = user is ContactInfo; break;
                        allow = user is ContactInfo || user is ClientContactInfo; break;
                    // Reports
                    case userActions.ViewReports:
                        allow = user is EmployeeInfo; break;
                    case userActions.ViewAdminReports:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector || user.UserType == EmployeeInfo.TypeConstructionsManager); break;
                    case userActions.ViewBudgetReports:
                        //#--allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector || user.UserType == EmployeeInfo.TypeConstructionsManager || user.UserType == EmployeeInfo.TypeBudgetAdministrator);break;
                          allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin || user.UserType == EmployeeInfo.TypeManagingDirector || user.UserType == EmployeeInfo.TypeConstructionsManager || user.UserType == EmployeeInfo.TypeBudgetAdministrator || user.UserType==EmployeeInfo.TypeProjectManager); break;
                        //#---
                    // Update links
                    case userActions.UpdateLinks:
                        allow = (user is EmployeeInfo) && (user.UserType == EmployeeInfo.TypeAdmin); break;

                    // Site Orders ---#
                    case userActions.SearchSiteOrders:
                        allow = user is EmployeeInfo || user is ClientContactInfo; break;
                    //case userActions.SelectSubContractor:
                    //    allow = user is EmployeeInfo; break;
                    case userActions.EditSiteOrder:
                        allow = (user is EmployeeInfo); break;
                    case userActions.ViewSiteOrder:
                        allow = user is EmployeeInfo; break;
                    case userActions.SearchSiteOrderDetail:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditSiteOrderDetail:
                        allow = user is EmployeeInfo; break;
                    case userActions.SearchSiteOrderDocs:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditSiteOrderDocs:
                        allow = user is EmployeeInfo; break;
                    case userActions.EditSiteOrderApprovals:
                        allow = (user is EmployeeInfo); break;
                    case userActions.EditSiteOrderApprovalsAll:
                        allow = (user is EmployeeInfo); break;

                }
            }

            if (!viewOnly && !allow)
                throw new SecurityException();

            return allow;
        }

        public static void CheckAccess(userActions action)
        {
            CheckAccess(action, false);
        }

        public static bool ViewAccess(userActions action)
        {
            return CheckAccess(action, true);
        }

        public static String GeneratePassword()
        {
            return System.Guid.NewGuid().ToString().Substring(0,8);
        }
#endregion

    }
}
