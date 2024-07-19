<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewHelpPage" Title="Help" Codebehind="Help.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="User Guide" />

<!-- Intro  SOS.Web.ViewHelpPage -->
<table>
    <tr>
        <td><asp:Image ID="imgIntro" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Introduction</td>
    </tr>
</table>

<asp:Panel ID="pnlIntro" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            SOS (Subcontractor Order System) manages Purchasing and Contracts Administration from tender stage 
                            through to contract letting and subcontractor's variations. At tender stage the system generates 
                            the Purchasing Schedule importing Rating A subcontractors, generates the invitations to tender and 
                            checklists based on a pre-selected subcontractor and drawings.<br />
                            <br />
                            Following the tender process the system creates the Standard Comparison for the Trade selected 
                            and manages the comparison and contract approval process.<br />
                            <br />
                            At the end of the process the system distributes the contract that has been created based on 
                            the trade selected; the items are included in the comparison and standard contracts and trade scopes.<br />
                            <br />
                            The System also provides an interface for the Subcontractors to download projects Drawings.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>




<!-- Login -->
<table>
    <tr>
        <td><asp:Image ID="imgLogin" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Login</td>
    </tr>
</table>

<asp:Panel ID="pnlLogin" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Access accounts are created by the system administrator. After loging in you can change your password 
                            clicking on your name at the top right of the screen. The minimum password length is 
                            4 characters. If you forget your password you can use the password reminder screen in the login 
                            page, the system will send it to your email account. If your session is inactive for a long 
                            time the system will log you out but it is recommended that you log out when you finish working 
                            with the application. Keep your password confidential and change it frequently.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>

<!-- Navigation -->
<table>
    <tr>
        <td><asp:Image ID="imgNavigation" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Navigation</td>
    </tr>
</table>

<asp:Panel ID="pnlNavigation" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            The system has a main menu on top, visible in all the screens,
                            plus a bread-crumbs navigation bar below it, also a lot of screens 
                            provide links or shortcuts to other screens.<br />
                            <br />
                            The bread-crumbs navigation bar is useful to know where you are and allows you to go back to previous screens. 
                            The example below shows you are inside a 
                            Variation and provides link to go back to: the Variation Order, the list of all Variation Orders, 
                            the Trade Contract, the Trade, the project, the list of projects and finally the initial 
                            or home screen.
                        </td>
                    </tr>
                </table>
                <br />
                
                 <table>
                    <tr>
                        <td align="center"><img alt="Navigation" src="../../Modules/Help/Images/Navigation.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Navigation</td>
                    </tr>
                 </table>
                 <br />
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>

<!-- Subcontractors -->
<table>
    <tr>
        <td><asp:Image ID="imgSubcontractors" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Subcontractors and Contacts</td>
    </tr>
</table>

<asp:Panel ID="pnlSubcontractors" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            You can have access to the Subcontractor's database through the main menu. The system 
                            shows a list of the current Subcontractors, you can filter the list by business unit 
                            and search by name, short name, suburb or business number, all the searches can be full 
                            or partial. The list can be sorted ascending or descending by clicking on the first row 
                            underlined labels.
                        </td>
                    </tr>
                 </table>
                 <br />
                 
                 <table>
                    <tr>
                        <td align="center"><img alt="Subcontractors" src="../../Modules/Help/Images/Subcontractors.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Subcontractors</td>
                    </tr>
                 </table>
                 <br />
                 
                 <table width="720">
                    <tr>
                        <td class="hlpText">
                            Every Subcontractor can have any number of contacts. To create a new contact use the 
                            option Contacts under Subcontractors in the main menu. The list of existing contacts 
                            is displayed; you can search by first name, last name or company name. The list can 
                            be sorted ascending or descending clicking on the first row underlined labels.
                        </td>
                    </tr>
                </table>
                <br />
                
                 <table>
                    <tr>
                        <td align="center"><img alt="Contacts" src="../../Modules/Help/Images/Contacts.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Contacts</td>
                    </tr>
                 </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>

<!-- Staff -->
<table>
    <tr>
        <td><asp:Image ID="imgStaff" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Staff</td>
    </tr>
</table>

<asp:Panel ID="pnlStaff" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Staff members can only be created by the administrator. In the main menu you can access the 
                            staff members list, search by first name or last name and sort by name. 
                            The administrator sees last login date and time for the users that have an access account.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Task List" src="../../Modules/Help/Images/Staff.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Staff</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>

<!-- Task List -->
<table>
    <tr>
        <td><asp:Image ID="imgTaskList" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Task List and Approval Process</td>
    </tr>
</table>

<asp:Panel ID="pnlTaskList" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            A list of pending tasks is displayed after you log in. you can see not only your tasks 
                            but also the ones from any subordinate role in any active project you are participating.<br />
                            <br />
                            Each pending tasks has a link to the corresponding approval process: a Comparison, 
                            Contract o Variation Order. The Comparison and Contract Approval Processes are located 
                            in the Project Trade Screen and the Variations Orders approval process in each Variation 
                            Order screen.
                        </td>
                    </tr>
                </table>
                <br />
                
                <table>
                    <tr>
                        <td align="center"><img alt="Task List" src="../../Modules/Help/Images/TaskList.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Task List</td>
                    </tr>
                </table>
                <br />
                
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            When a Trade is added to a project or a Contract or Variation Order is created the system 
                            assigns Approval Processes to them and if the project has a commencement date it 
                            calculates approval target dates for every step of the process.<br />
                            <br />
                            The process steps can be approved not only by the assigned role but also by another one with superior 
                            rights within the project. The table below shows which assignments (tasks) can be approved by the user or other roles.<br />
                            <br />
                            <table border="1" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td style="background-color:#FFCC00; font-weight:bold;">Assigned to</td>
                                    <td style="background-color:#FFCC00; font-weight:bold;">Can be approved by</td>
                                </tr>
                                <tr>
                                    <td align="center">MD</td>
                                    <td align="center">MD</td>
                                </tr>
                                <tr>
                                    <td align="center">CM</td>
                                    <td align="center">CM, MD</td>
                                </tr>
                                <tr>
                                    <td align="center">DM</td>
                                    <td align="center">DM, MD</td>
                                </tr>
                                <tr>
                                    <td align="center">PM</td>
                                    <td align="center">PM, CM, MD</td>
                                </tr>
                                <tr>
                                    <td align="center">DC</td>
                                    <td align="center">DC, DM, MD</td>
                                </tr>
                                <tr>
                                    <td align="center">CA</td>
                                    <td align="center">CA, PM, CM, MD</td>
                                </tr>
                            </table>
                            <br />
                            <table cellpadding="1" cellspacing="0">
                                <tr>
                                    <td><b>MD</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Managing Director</td>
                                </tr>
                                <tr>
                                    <td><b>CM</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Constructions Manager</td>
                                </tr>
                                <tr>
                                    <td><b>PM</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Project Manager</td>
                                </tr>
                                <tr>
                                    <td><b>CA</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Contracts Administrator</td>
                                </tr>
                                <tr>
                                    <td><b>DM</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Design Manager</td>
                                </tr>
                                <tr>
                                    <td><b>DC</b></td>
                                    <td>:&nbsp;</td>
                                    <td>Design Coordinator</td>
                                </tr>
                            </table>
                            <br />
                            After you execute a step the system will not allow you to modify the information 
                            any more so it will show to the next person in the process the way it was when 
                            you approved it. After approval, a notification via email is sent to the next 
                            person in the approval process. For Contracts and Variations Orders, after executing 
                            the last step of the approval process the contract is sent to the Subcontractor, site and Accounts 
                            via email and/or fax.<br />
                            <br />
                            If you need to move the process one step back you can do so clicking on the Reverse 
                            Button on the Approval Process Manager. Not always a process can be reversed, for example 
                            when the last step executed was a Final approval or a Create work order. When you reverse 
                            a process you can provide a comment that will be displayed on the Approval Process Manager 
                            in the Step that has to be re-executed.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>

<!-- Projects -->
<table>
    <tr>
        <td><asp:Image ID="imgProject" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Projects</td>
    </tr>
</table>

<asp:Panel ID="pnlProject" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <!-- Mange Projects -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgManageProjects" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Manage Projects</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlManageProjects" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The first option in the menu displays a list of active projects, the list can be filtered by business 
                                unit or project status, it is also searchable by project name and address.
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Projects List" src="../../Modules/Help/Images/Projects.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Projects List</td>
                        </tr>
                    </table>
                    <br />

                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                A new project can be created using the Add icon
                                <img alt="Add" src="../../Images/IconAdd.gif" />.
                                This can only be done by the roles MD, CM or the System Administrator. When a new project is created the following actions take place:
                                <ul>
                                    <li>All the Standard Trades are added to the project creating the Purchasing Schedule</li>
                                    <li>All the Rating A Subcontractors are added to the Trades Tender Invitation List</li>
                                    <li>The default Drawing types related to each Trade are imported</li>
                                    <li>Approval Processes are created for each Trade based on its business unit and job type</li>
                                    <li>If the project commencement date -PCD- is specified, the Approval Process target dates are calculated for all the Trades that have a default number of days from PCD</li>
                                </ul>
                                Once the project is created it can be managed in the Project View Screen where you can:
                                <ul>
                                    <li>Update project general information <img alt="View" src="../../Images/IconEdit.gif" /></li>
                                    <li>Use Contract Manager</li>
                                    <li>Use Purchasing Schedule</li>
                                    <li>Add/remove/update Project Trades</li>
                                    <li>Manage Drawings</li>
                                    <li>Manage Transmittals</li>            
                                </ul>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <!-- Contract Manager -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgContractManager" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Contract Manager</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlContractManager" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                Below the General Information Section in the View Project Screen is the
                                Contract Manager. This module contains an updated list of all the Trades 
                                for which a Subcontractor has been already assigned.
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                For each Trade/Contract the system shows the following:
                                <ul>
                                    <li><b>Trade name and code</b>. Use <img alt="Open" src="../../Images/IconView.gif" /> to go to View Trade Screen</li>
                                    <li><b>Subcontractor</b>. Selected Subcontractor (Rank no. 1)</li>
                                    <li><b>Status Date</b>. Current Trade status and its date</li>
                                    <li><b>Contract due date</b>. Estimated date when the Contract should be completed</li>
                                    <li><b>Due days</b>. Number of days remaining to complete the Contract. When the current date is before the target date a negative number is displayed. If the current date is after the target date a positive number in red color is displayed indicating how many days have passed after the target date</li>
                                    <li><b>Comparison Approved</b>. Indicates with a check if the Comparison has already been fully approved for the Trade.</li>
                                    <li><b>Contract</b>. If the Contract has been created and fully approved it can be seen clicking in the document icon <img alt="Document" src="../../Images/IconDocument.gif" /></li>
                                    <li><b># Variation Orders</b>. Number of Variation Orders.</li>
                                </ul>
                            </td>
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td align="center"><img alt="Contract Manager" src="../../Modules/Help/Images/ContractManager.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Contract Manager</td>
                        </tr>
                    </table>
                    <br />

                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The contracts are created as part of the Trade Process; such process normally 
                                includes work order number generation and Contract drafting. 
                                The Contract is created based on the corresponding template according to the 
                                business unit and the job type. If any of the variables defined in the template 
                                does not have a value a list of errors will be displayed; such as subcontractor's 
                                contact details or ABN Number, Trade commencement and completion dates, Project 
                                details, etc.<br />
                                <br />
                                All those values have to be filled in to be able to draft the Contract. 
                                After the Contract has been created its approval process starts, the target 
                                approval dates are calculated and the edit Contract screen is available. 
                                You can make changes to any editable section of the Contract; the system will 
                                keep track of all the changes indicating the date and time of the change and 
                                the person who made it. After you approve the Contract you will not be able 
                                to make any changes to it.
                            </td>
                        </tr>
                    </table>
                    <br />
                </asp:Panel>
               
                <!-- Purchasing Schedule -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgPurchasingSchedule" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Purchasing Schedule</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlPurchasingSchedule" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The Purchasing Schedule is situated below the Contract Manager section in the view project screen. It shows all the project 
                                Trades that are in tender process for which a Subcontractor has not been assigned. The Trades List can be 
                                filter by job type and shows the following columns.
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <table>
                        <tr>
                            <td align="center"><img alt="Purchasing Schedule" src="../../Modules/Help/Images/PurchasingSchedule.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Purchasing Schedule</td>
                        </tr>
                    </table>
                    <br />
                    
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                <ul>
                                    <li><b>Trade name and code</b>. The open icon <img alt="View" src="../../Images/IconView.gif" /> takes you to the View Trade Screen</li>
                                    <li><b>Invitation date</b>. Estimated Trade Invitation Date, this is the date when all the invitations to tender must be sent by and also determines the tender process start date.</li>
                                    <li><b>Closing date</b>. Estimated date for quotes submission by subcontractors.</li>
                                    <li><b>Status Date</b>. The current Trade status and its date</li>
                                    <li><b>Comparison due date</b>. Estimated Comparison completion date</li>
                                    <li><b>Due days</b>. Number of days remaining to complete the Comparison. When the current date is before the target date a negative number is displayed. If the current date is higher than the target date a positive number in red color in displayed indicating how many days the comparison is overdue.</li>
                                    <li><b>Subbie 1..n</b>. Names of all the Subcontractors invited to participate in the tender process.</li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <!-- Drawing Register -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgDrawingRegister" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Drawing Register</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlDrawingRegister" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                On the View Project screen and below the Purchasing Schedule is the Drawing Register, 
                                this module shows all the Drawings and revisions related to the project grouped by Drawing Type, 
                                from here you can:
                                <ul>
                                    <li><b>List Drawings by type</b>. In this screen you can go to any of the Drawings Screens or add a new Drawing. The list shows the Drawing name and description, last revision with date, total number of revisions and the number of Trades the Drawing is related to.</li>
                                    <li><b>View Drawing</b>. Here you see Drawing information including all its revisions sorted by date and a list of Trades the Drawing is related too; you can also edit, update or delete the Drawing and add or remove revision.</li>
                                    <li><b>View Revision</b>. Shows the specific revision information.</li>
                                </ul>
                                A list of all Drawings and last revisions can be obtained clicking on the printing icon <img alt="Print" src="../../Images/IconPrint.gif" />.
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <table>
                        <tr>
                            <td align="center"><img alt="Drawing Register" src="../../Modules/Help/Images/DrawingManager.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Drawing Register</td>
                        </tr>
                    </table>
                    <br />
                </asp:Panel>

                <!-- Transmittals -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgTransmittals" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Transmittals</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlTransmittals" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                Drawings Transmittals can be created using the Transmittals module at the bottom of the view 
                                project screen. A new Transmittal can be added clicking the Add button 
                                <img alt="Add" src="../../Images/IconAdd.gif" />, 
                                to modify and existing one you can select it doing click on its name under the corresponding 
                                Subcontractor.
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <table>
                        <tr>
                            <td align="center"><img alt="Transmittals" src="../../Modules/Help/Images/Transmittals.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Transmittals</td>
                        </tr>
                    </table>
                    <br />
                    
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                Once in the adding or updating screen you can include as many Drawings as required, 
                                the system will use the last revision.
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    <table>
                        <tr>
                            <td align="center"><img alt="Adding Transmittals" src="../../Modules/Help/Images/Transmittal.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Edit Transmittals</td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Project Trades -->
<table>
    <tr>
        <td><asp:Image ID="imgProjectTrade" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Project Trade</td>
    </tr>
</table>

<asp:Panel ID="pnlProjectTrade" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            From both Contracts Manager and Purchasing Schedule you can go to the View Trade Screen where the 
                            following elements are present.
                        </td>
                    </tr>
                </table>
                <br />
 
                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">General Trade Information</td>
                    </tr>
                    <tr>
                        <td class="hlpText">Update Trade general information. Initially the Trade has the information coming from the template.</td>
                    </tr>
                </table>
                <br />

                <!-- Trade categories -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgItemCategories" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Trade Categories and Items</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlItemCategories" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                If the Trade requires specific categories for the project or some categories do not apply you 
                                can adjust the Trade accordingly adding, removing, updating or reordering Trade categories and/or 
                                items.
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Trade Categories" src="../../Modules/Help/Images/ItemCategories.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Trade Categories</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">Order Letting Minutes</td>
                    </tr>
                    <tr>
                        <td class="hlpText">
                            Print the Order Letting Minutes. This link is displayed after the work order 
                            number has been generated. All the variables defined in the template must be 
                            filled in to be able to print it.
                        </td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">Contract</td>
                    </tr>
                    <tr>
                        <td class="hlpText">View Contract. This link is displayed after the Contract is created.</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">Variation Orders</td>
                    </tr>
                    <tr>
                        <td class="hlpText">Go to Variation Orders screen. This link is displayed after the Contract has been approved.</td>
                    </tr>
                </table>
                <br />

                <!-- Approval Process Manager -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgApproval" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Approval Process Manager</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlApproval" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">View process information and execute approval tasks.</td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Trade Categories" src="../../Modules/Help/Images/ApprovalManager.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Approval Process Manager</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <!-- Tender Invitation List -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgSubbies" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Tender Invitation List</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlSubbies" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The invitation list shows the tender process participants. Here you can add or remove 
                                Subcontractors from the Trade. For each Subcontractor you can specify its status, 
                                quote amount, invitation date, due date, quote date, contact person and the rank within 
                                the tender process. The contact person in assigned automatically if the Subcontractor 
                                has only one contact registered in the system.<br />
                                <br />
                                In order to print the invitation to tender all the variables defined in the template 
                                must be filled in.
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Trade Categories" src="../../Modules/Help/Images/InvitationList.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Tender Invitation List</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                
                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">Comparison</td>
                    </tr>
                    <tr>
                        <td class="hlpText">Go to Comparison.</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpSubTitle">Check List</td>
                    </tr>
                    <tr>
                        <td class="hlpText">Print the Trade Check List.</td>
                    </tr>
                </table>
                <br />

                <!-- Drawing Types -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgDrawingTypes" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Drawing Types</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlDrawingTypes" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">Add or remove Trade Drawing Types.</td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Drawing Types" src="../../Modules/Help/Images/DrawingTypes.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Drawing Types</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <!-- Drawings -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgDrawings" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Drawings</td>
                    </tr>
                </table>
                
                <asp:Panel ID="pnlDrawings" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">Add or remove Trade Drawings.</td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Drawings" src="../../Modules/Help/Images/TradeDrawings.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Trade Drawings</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Comparison -->
<table>
    <tr>
        <td><asp:Image ID="imgComparison" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Comparison</td>
    </tr>
</table>

<asp:Panel ID="pnlComparison" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            The Comparison module can be accessed from the Project Trade Screen 
                            using the link located above the Tender Invitation List. The system does an 
                            automatic comparison checking every time the view comparison screen 
                            is displayed.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Comparison Checking" src="../../Modules/Help/Images/ComparisonCheck.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Comparison Checking</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Most of the labels in the comparison checking are links to the corresponding 
                            screen where the error can be fixed.<br />
                            <br />
                            The comparison can be updated one Subcontractor at a time entering into the 
                            edit mode. To do so select the Subcontractor in the dropdown 
                            list and click the edit button <img alt="Edit" src="../../Images/IconEdit.gif" />.
                            Once on the edit screen make sure 
                            you provide an answer to every Y/N questions. If there are Assumed or 
                            confirmed values they can be added either in the edit comparison screen or 
                            in the comparison item screen which you can access from the comparison view 
                            screen clicking on any item in the Y/N column for any Subcontractor. 
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Comparison" src="../../Modules/Help/Images/Comparison.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Comparison</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Variation Orders -->
<table>
    <tr>
        <td><asp:Image ID="imgVariations" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Variations</td>
    </tr>
</table>

<asp:Panel ID="pnlVariations" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Variation Orders can be created for any Trade that has an approved Contract. 
                            As was mentioned above a link to the variation orders is displayed on the Trade 
                            Screen once the Contract is approved. 
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Variation Orders" src="../../Modules/Help/Images/Variations.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Variation Orders</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            The system shows a list of existing Variation Orders with their current approval status. 
                            A new Variation Order can be created using the add icon <img alt="Add" src="../../Images/IconAdd.gif" />. 
                            If the Variation Order has been approved an icon that allows to print it is displayed <img alt="Print" src="../../Images/IconPrint.gif" />. 
                            All the variables defined in the template must be filled in to be able to print it.<br />
                            <br />
                            A Variation Order consist of a general information section and a list of Variations.
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>


<!-- Client Trades -->
<table>
    <tr>
        <td><asp:Image ID="imgClientTrades" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Client Trades</td>
    </tr>
</table>

<asp:Panel ID="pnlClientTrades" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Client trades can be added using the corresponding link on the View Project page.
                            As client trades are added the system calculates their total and compares it to the contract amount,
                            only when they math, Claims can be created.<br />
                            <br />
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Client Trades" src="../../Modules/Help/Images/ClientTrades.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Client Trades</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Client trades can be added and updated at any moment but they cannot be deleted or reordered if they are already used by Claim.
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Client Variations and Separate Accounts -->
<table>
    <tr>
        <td><asp:Image ID="imgCVs" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Client Variations and Separate Accounts</td>
    </tr>
</table>

<asp:Panel ID="pnlCVs" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Client Variations CV and Separate Accounts SA can be added using the corresponding link on the View Project page.
                            The system shows the list of existing CV and SA indicating its process dates, approval state and total amount.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Client Variations" src="../../Modules/Help/Images/ClientVariations.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Client Variations</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Selecting a CV or SA from the lists takes you to the View Client Variation screen where you can update
                            its information and perform approvals. Any number of Items and Trades can be added. 
                            SOS verifies that the Items total correspond to the Trades total, if not, a warning message is displayed.<br />
                            <br />
                            Revisions can be created for a CV and SA after its internal approval.
                            When a revision is created the approval process starts over. 
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Claims -->
<table>
    <tr>
        <td><asp:Image ID="imgClaims" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Claims</td>
    </tr>
</table>

<asp:Panel ID="pnlClaims" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Claims can be created once the Client Trades are added to the project and they match the total
                            contract amount specified in the Project General Information page.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Claims" src="../../Modules/Help/Images/Claims.gif" /></td>
                    </tr>                    
                    <tr>
                        <td class="hlpFigure" align="center">Claims</td>
                    </tr>
                </table>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            If the project has First Claim Due Date, Claim Frequency and Payment Terms filled out then Claim Issue Date,
                            Due Date and approval process Dates will be calculated automatically.<br />
                            <br />
                            When new claims are created the system uses the last claim to get the list of Trades and Variations that 
                            will be included, then the user can specified the new percentages or total amounts for them.
                            Trades or Variations that have been included in previous Claims cannot be removed in future Claims;
                            for others they can be excluded leaving both the percentage and amount text boxes empty.
                            If a Trade or Variation needs to be included but not specifying any amount that can be done entering
                            cero (0) as the amount.<br />
                            <br />
                            SOS will automatically generate the Final Claim when all the Trades and Variations have been claimed.
                            At that point no more Claims can be added.
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Request for Information (RFI)  -->
<table>
    <tr>
        <td><asp:Image ID="imgRFIs" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Requests for Information - RFIs</td>
    </tr>
</table>

<asp:Panel ID="pnlRFIs" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            RFIs can be created from the Project View Page. In addition to the general information the RFI includes
                            a reference file which will be attached to the emails when the RFI is sent to the distribution list.<br />
                            <br />
                            When an RFI is sent to the distribution list its status changes to <i>Sent</i>, to send it again Status
                            has to be manually set to <i>New</i>.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Requests for Information - RFIs" src="../../Modules/Help/Images/RFIs.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Requests for Information - RFIs</td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Extension of Time (EOT) -->
<table>
    <tr>
        <td><asp:Image ID="imgEOTs" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Extensions of Time - EOTs</td>
    </tr>
</table>

<asp:Panel ID="pnlEOTs" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            EOTs can be created from the Project View Page. Only when all the information is complete SOS
                            allows to send or print the EOT document.
                        </td>
                    </tr>
                </table>
                <br />

                <table>
                    <tr>
                        <td align="center"><img alt="Extensions of Time - EOTs" src="../../Modules/Help/Images/EOTs.gif" /></td>
                    </tr>
                    <tr>
                        <td class="hlpFigure" align="center">Extensions of Time - EOTs</td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Admin functions -->
<table>
    <tr>
        <td><asp:Image ID="imgAdmin" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Admin Functions</td>
    </tr>
</table>

<asp:Panel ID="pnlAdmin" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <!-- Trade Templates -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgTradeTemplates" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Trade Templates</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlTradeTemplates" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                Manages the Trades that will be available to use in the projects. In the Trade definition the 
                                following information is specified:
                                <ul>
                                    <li><b>Name</b></li>
                                    <li><b>Code</b></li>
                                    <li><b>Description</b></li>
                                    <li><b>Job Type</b></li>
                                    <li><b>Days from PCD</b>. Number of business days from or to the project commencement date used to calculate all the targets dates for the Trade in a project such as Invitation due date, comparison due date, Contract due date, etc.</li>
                                    <li><b>Standard Check</b>. The Standard Trades will be included automatically in new projects</li>
                                    <li><b>Scope Header and Footer</b>. These texts will be used to create the scope of works in the contracts.</li>
                                    <li><b>Item Categories</b>. List items grouped by categories that will be used to create the Comparison and Check List and also to include in the Scope of Works.</li>
                                    <li><b>Drawing Types</b>. List of default Drawing Types that will be associated with the Trade within a project.</li>
                                    <li><b>Rating A Subcontractors</b>. List of Subcontractors that will be invited by default to participate in this Trade within a projects.</li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Trade Templates" src="../../Modules/Help/Images/TradeTemplates.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Trade Templates</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <!-- Contract Templates -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgContractTemplates" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Contract Templates</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlContractTemplates" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                Manages the Contract templates that are used to create the contracts and Variation Orders for the Trades in a project. 
                                Three different templates can be created for every combination of business unit and job type: standard, 
                                simplified and Variations.
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Contract Templates" src="../../Modules/Help/Images/ContractTemplates.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Contract Templates</td>
                        </tr>
                    </table>
                    <br />

                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The templates are created in HTML plus additional specific tags that are used by the system to replace by variables 
                                or create editable sections when using the template to create a Contract.<br />
                                <br />
                                <b>Editable sections</b><br />
                                In order to create an editable section the following syntax is used:<br />
                                <b>[SOS:Editable:</b><i>SectionName</i><b> Title:</b><i>Title to display</i><b>]</b><i>Default content</i><b>[/SOS:Editable]</b><br />
                                <br />
                                For example:<br />
                                [SOS:Editable:InsuranceWorkers Title:Insurance – Workers Compensation]$ 10,000,000 (Ten million dollars)[/SOS:Editable]<br />
                                <br />
                                The Section Name is used by the system to store the changes to the editable section and must be unique 
                                within the template. The Title to display is what the user sees when working with a template. The example above would look like:<br />
                                <br />
                                <img alt="Editable Section" src="../../Modules/Help/Images/TemplateSection.gif" />
                                <br />
                                <br />
                                <b>Variables</b><br />
                                There are a lot of system variables that can be used in a template, when the template is used to create a 
                                Contract these variables will be replaced by editable sections using the current value of the variable 
                                as a default. To define a variable within a template use the following syntax:<br />
                                <br />
                                <b>[SOS:Variable:</b><i>Variable Name</i><b> Title:</b><i>Title to display</i><b>/]</b><br />
                                <br />
                                For example:<br />
                                [SOS:Variable:SubcontractorName Title:Subcontractor Name/]<br />
                                <br />
                                The variables that can be used in templates are:
                                <ul>
                                    <li>CAEmail</li>
                                    <li>CAFax</li>
                                    <li>CAName</li>
                                    <li>CAPhone</li>
                                    <li>CASignature</li>
                                    <li>CMName</li>
                                    <li>CMSignature</li>
                                    <li>CommencementDate</li>
                                    <li>CompletionDate</li>
                                    <li>ContractDate</li>
                                    <li>ContractGST</li>
                                    <li>ContractTotal</li>
                                    <li>ContractValue</li>
                                    <li>ContratTotalWords</li>
                                    <li>DCName</li>
                                    <li>DCSignature</li>
                                    <li>DefectsLiability</li>
                                    <li>DMName</li>
                                    <li>DMSignature</li>
                                    <li>Drawings</li>
                                    <li>DrawingsSummary</li>
                                    <li>DueDate</li>
                                    <li>Interest</li>
                                    <li>JobNumber</li>
                                    <li>LiquidatedDamages</li>
                                    <li>OrderNumber</li>
                                    <li>ParticipationDueDate</li>
                                    <li>PMName</li>
                                    <li>PMSignature</li>
                                    <li>PreLettingDate</li>
                                    <li>Principal</li>
                                    <li>ProjectCommencementDate</li>
                                    <li>ProjectCompletionDate</li>
                                    <li>ProjectName</li>
                                    <li>ProjectNumber</li>
                                    <li>ProjectSuburb</li>
                                    <li>ProjectYear</li>
                                    <li>Retention</li>
                                    <li>RetentionToCertification</li>
                                    <li>RetentionToLdp</li>
                                    <li>ScopeOfWorks</li>
                                    <li>SiteAddress</li>
                                    <li>SiteAllowances</li>
                                    <li>SiteInstruction</li>
                                    <li>SiteInstructionDate</li>
                                    <li>SiteManager</li>
                                    <li>SitePhone</li>
                                    <li>SubcontractorABN</li>
                                    <li>SubcontractorAddress</li>
                                    <li>SubcontractorContact</li>
                                    <li>SubcontractorContactEmail</li>
                                    <li>SubcontractorContactMobile</li>
                                    <li>SubcontractorContactPhone</li>
                                    <li>SubcontractorFax</li>
                                    <li>SubcontractorName</li>
                                    <li>SubcontractorPhone</li>
                                    <li>SubcontractorReference</li>
                                    <li>SubcontractorReferenceDate</li>
                                    <li>Today</li>
                                    <li>TradeCode</li>
                                    <li>TradeName</li>
                                    <li>VariationOrder</li>
                                    <li>VariationOrderTitle</li>
                                    <li>Variations</li>
                                    <li>VariationsGST</li>
                                    <li>VariationsTotal</li>
                                    <li>VariationsTotalPlusGST</li>
                                    <li>VariationsTrades</li>
                                </ul>
                                There are three sections that are used for specific purposes:<br />
                                <br />
                                <b>[SOS:Footer]</b><br />
                                <i>Footer</i><br />
                                <b>[/SOS:Footer]</b><br />
                                <br />
                                <b>[SOS:Financial]</b><br />
                                <i>Finantial information</i><br />
                                <b>[/SOS:Financial]</b><br />
                                <br />
                                <b>[SOS:Terms]</b><br />
                                <i>Contract Terms</i><br />
                                <b>[/SOS:Terms]</b><br />
                           </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            In addition to the contracts, templates can also be created for the Order Letting Minutes and Invitation to Tender.
                        </td>
                    </tr>
                </table>
                <br />           
            
                <!-- Business Units -->
                <table>
                    <tr>
                        <td><asp:Image ID="imgBusinessUnits" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                        <td class="hlpSubTitle">Business Units</td>
                    </tr>
                </table>

                <asp:Panel ID="pnlBusinessUnits" runat="server" CssClass="collapsePanel" Height="0">
                    <table width="720">
                        <tr>
                            <td class="hlpText">
                                The system allows you to create any number of business units each one of them with its own Approval Processes 
                                for Trades and contracts.
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td align="center"><img alt="Business Units" src="../../Modules/Help/Images/BusinessUnits.gif" /></td>
                        </tr>
                        <tr>
                            <td class="hlpFigure" align="center">Business Units</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <!-- Job Types -->
                <table>
                    <tr>
                        <td class="hlpSubTitle">Job Types</td>
                    </tr>
                </table>

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            The system Can have any number of job types defined. They are used to classify the Trades and also to 
                            create different Contract templates.
                        </td>
                    </tr>
                </table>
                <br />

                <!-- Drawings Types -->
                <table>
                    <tr>
                        <td class="hlpSubTitle">Drawing Types</td>
                    </tr>
                </table>

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            Manages the Drawings Types that can be used in the project.
                        </td>
                    </tr>
                </table>
                <br />

                <!-- Holidays -->
                <table>
                    <tr>
                        <td class="hlpSubTitle">Holidays</td>
                    </tr>
                </table>

                <table width="720">
                    <tr>
                        <td class="hlpText">
                            This is a list of all the holidays and no work days used to calculate target dates 
                            for Trade and Contract processes. It is important to include all the holidays for every 
                            year so these dates can be calculated accurately.
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

<!-- Instalation -->
<table>
    <tr>
        <td><asp:Image ID="imgInstallation" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="hlpTitle">Installation</td>
    </tr>
</table>

<asp:Panel ID="pnlInstallation" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table width="720">
                    <tr>
                        <td class="hlpText">
                            The system requires the following server components:
                            <ul>
                                <li>Internet Information Server IIS 5.1 or higher</li>
                                <li>.NET framework 2.0 or higher</li>
                                <li>SQL Server 2005</li>
                                <li>ASP .NET AJAX Extensions</li>
                                <li>Report Viewer Redistributable</li>
                                <li>Mail Server access</li>
                            </ul>
                            To install the system you have to:
                            <ul>
                                <li>Copy the folder \SOS from the installation CD to your server.</li>
                                <li>Restore the Database Backup from the installation CD in SQL Server 2005.</li>
                                <li>Create a virtual folder for the \SOS folder in IIS making sure .NET version 2.0 is selected.</li>
                                <li>Configure the parameters in the Web.config file.</li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <td class="hlpSubTitle">Configuration</td>
                    </tr>
                    <tr>
                        <td class="hlpText">
                            Once the system is installed the following parameters must be configured:
                            <ul>
                                <li><b>ConnectionString</b>: Database access connection string for SQL Server 2005. <i>Server</i>;Database=<i>Database</i>;uid=<i>UserId</i>;pwd=<i>Password</i></li>
                                <li><b>BaseUrl</b>: URL Base. i.e. <i>http://myintranet/SOS</i></li>
                                <li><b>EmailFrom</b>: Email address used by the system to send emails, this mailbox must exist in the mail server i.e. <i>sos@mydomain.com</i></li>
                                <li><b>EmailAccounts</b>: Mailbox for accounts where copies of the contracts will be sent  i.e. <i>accounts@mydomain.com</i></li>
                                <li><b>EmailFax</b>: Mailbox used to send fax i.e. <i>myfax@mydomain.com</i></li>
                                <li><b>DocumentsFolder</b>: Folder name where the documents are stored. i.e. <i>G:\</i></li>
                            </ul>
                        </td>
                    </tr>                
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<hr />

<table>
    <tr>
        <td class="hlpTitle" colspan="2">Documents</td>
    </tr>
    <tr>
        <td style="width:25px;"></td>
        <td><asp:HyperLink ID="lnkPurchasingProcess" NavigateUrl="~/Modules/Help/Docs/PurchasingProcess.pdf" runat="server" CssClass="frmLink" Target="_blank">Purchasing Process</asp:HyperLink></td>
    </tr>
</table>
<br />
    <%-- San   ----  G-->\\vc-exchange-03\GROUPDATA--------   G:\Design\SOS Development 2016\Videos\Vaughan_Final_Videos_x4\Vaughan - Video 1 - #8450121 - Ver06 - 27-09-16 - Final.mp4--%>

    <table>
    <tr>
        <td class="hlpTitle" colspan="2">Training Videos<br />
        </td>
    </tr>

        <tr>
        <td style="width:25px;">&nbsp;</td>
        <td><li style="font-weight: 700">General System Over view</li></td>
    </tr>

        <tr>
        <td style="width:25px;"></td>
        <td style="padding-left:30px"><asp:HyperLink ID="HyperLink4" NavigateUrl="~/Modules/Help/Docs/SOS General Overview.mp4" runat="server" CssClass="frmLink" Target="_blank">SOS General Overview</asp:HyperLink></td>
    </tr>

    <tr>
        <td style="width:25px;">&nbsp;</td>
        <td><li style="font-weight: 700"> Purchasing process</li></td>
    </tr>

    <tr>
        <td style="width:25px;"></td>
        <td style="padding-left:30px"><asp:HyperLink ID="HyperLink1" NavigateUrl="~/Modules/Help/Docs/Purchasing Process Video 1 of 3.mp4" runat="server" CssClass="frmLink" Target="_blank">Purchasing Process Video 1 of 3</asp:HyperLink></td>
    </tr>
 <tr>
        <td style="width:25px;"></td>
        <td style="padding-left:30px"><asp:HyperLink ID="HyperLink2" NavigateUrl="~/Modules/Help/Docs/Purchasing Process Video 2 of 3.mp4" runat="server" CssClass="frmLink" Target="_blank">Purchasing Process Video 2 of 3</asp:HyperLink></td>
    </tr>

 <tr>
        <td style="width:25px;"></td>
        <td style="padding-left:30px"><asp:HyperLink ID="HyperLink3" NavigateUrl="~/Modules/Help/Docs/Purchasing Process Video 3 of 3.mp4" runat="server" CssClass="frmLink" Target="_blank">Purchasing Process Video 3 of 3</asp:HyperLink></td>
    </tr>
</table>
    <%-- San --%>
<br />

<act:CollapsiblePanelExtender ID="cpeIntro" runat="Server" TargetControlID="pnlIntro" ExpandControlID="imgIntro" CollapseControlID="imgIntro" ImageControlID="imgIntro" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeLogin" runat="Server" TargetControlID="pnlLogin" ExpandControlID="imgLogin" CollapseControlID="imgLogin" ImageControlID="imgLogin" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeNavigation" runat="Server" TargetControlID="pnlNavigation" ExpandControlID="imgNavigation" CollapseControlID="imgNavigation" ImageControlID="imgNavigation" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeSubcontractors" runat="Server" TargetControlID="pnlSubcontractors" ExpandControlID="imgSubcontractors" CollapseControlID="imgSubcontractors" ImageControlID="imgSubcontractors" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeStaff" runat="Server" TargetControlID="pnlStaff" ExpandControlID="imgStaff" CollapseControlID="imgStaff" ImageControlID="imgStaff" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeTaskList" runat="Server" TargetControlID="pnlTaskList" ExpandControlID="imgTaskList" CollapseControlID="imgTaskList" ImageControlID="imgTaskList" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender ID="cpeProject" runat="Server" TargetControlID="pnlProject" ExpandControlID="imgProject" CollapseControlID="imgProject" ImageControlID="imgProject" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeManageProjects" runat="Server" TargetControlID="pnlManageProjects" ExpandControlID="imgManageProjects" CollapseControlID="imgManageProjects" ImageControlID="imgManageProjects" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeContractManager" runat="Server" TargetControlID="pnlContractManager" ExpandControlID="imgContractManager" CollapseControlID="imgContractManager" ImageControlID="imgContractManager" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpePurchasingSchedule" runat="Server" TargetControlID="pnlPurchasingSchedule" ExpandControlID="imgPurchasingSchedule" CollapseControlID="imgPurchasingSchedule" ImageControlID="imgPurchasingSchedule" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeDrawingRegister" runat="Server" TargetControlID="pnlDrawingRegister" ExpandControlID="imgDrawingRegister" CollapseControlID="imgDrawingRegister" ImageControlID="imgDrawingRegister" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeTransmittals" runat="Server" TargetControlID="pnlTransmittals" ExpandControlID="imgTransmittals" CollapseControlID="imgTransmittals" ImageControlID="imgTransmittals" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender ID="cpeProjectTrade" runat="Server" TargetControlID="pnlProjectTrade" ExpandControlID="imgProjectTrade" CollapseControlID="imgProjectTrade" ImageControlID="imgProjectTrade" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeItemCategories" runat="Server" TargetControlID="pnlItemCategories" ExpandControlID="imgItemCategories" CollapseControlID="imgItemCategories" ImageControlID="imgItemCategories" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeApproval" runat="Server" TargetControlID="pnlApproval" ExpandControlID="imgApproval" CollapseControlID="imgApproval" ImageControlID="imgApproval" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeSubbies" runat="Server" TargetControlID="pnlSubbies" ExpandControlID="imgSubbies" CollapseControlID="imgSubbies" ImageControlID="imgSubbies" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeDrawingTypes" runat="Server" TargetControlID="pnlDrawingTypes" ExpandControlID="imgDrawingTypes" CollapseControlID="imgDrawingTypes" ImageControlID="imgDrawingTypes" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender ID="cpeDrawings" runat="Server" TargetControlID="pnlDrawings" ExpandControlID="imgDrawings" CollapseControlID="imgDrawings" ImageControlID="imgDrawings" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender ID="cpeComparison" runat="Server" TargetControlID="pnlComparison" ExpandControlID="imgComparison" CollapseControlID="imgComparison" ImageControlID="imgComparison" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeVariations" runat="Server" TargetControlID="pnlVariations" ExpandControlID="imgVariations" CollapseControlID="imgVariations" ImageControlID="imgVariations" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeClientTrades" runat="Server" TargetControlID="pnlClientTrades" ExpandControlID="imgClientTrades" CollapseControlID="imgClientTrades" ImageControlID="imgClientTrades" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeCVs" runat="Server" TargetControlID="pnlCVs" ExpandControlID="imgCVs" CollapseControlID="imgCVs" ImageControlID="imgCVs" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeClaims" runat="Server" TargetControlID="pnlClaims" ExpandControlID="imgClaims" CollapseControlID="imgClaims" ImageControlID="imgClaims" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeRFIs" runat="Server" TargetControlID="pnlRFIs" ExpandControlID="imgRFIs" CollapseControlID="imgRFIs" ImageControlID="imgRFIs" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeEOTs" runat="Server" TargetControlID="pnlEOTs" ExpandControlID="imgEOTs" CollapseControlID="imgEOTs" ImageControlID="imgEOTs" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeAdmin" runat="Server" TargetControlID="pnlAdmin" ExpandControlID="imgAdmin" CollapseControlID="imgAdmin" ImageControlID="imgAdmin" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeTradeTemplates" runat="Server" TargetControlID="pnlTradeTemplates" ExpandControlID="imgTradeTemplates" CollapseControlID="imgTradeTemplates" ImageControlID="imgTradeTemplates" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeContractTemplates" runat="Server" TargetControlID="pnlContractTemplates" ExpandControlID="imgContractTemplates" CollapseControlID="imgContractTemplates" ImageControlID="imgContractTemplates" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeBusinessUnits" runat="Server" TargetControlID="pnlBusinessUnits" ExpandControlID="imgBusinessUnits" CollapseControlID="imgBusinessUnits" ImageControlID="imgBusinessUnits" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="cpeInstallation" runat="Server" TargetControlID="pnlInstallation" ExpandControlID="imgInstallation" CollapseControlID="imgInstallation" ImageControlID="imgInstallation" ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical" Collapsed="True" CollapsedSize="0"></act:CollapsiblePanelExtender>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
   
</asp:Content>

