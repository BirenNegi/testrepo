using System;

namespace SOS.Core
{
    [Serializable]
    public class EmployeeInfo : PeopleInfo
    {

#region Constants
        public const String TypeAdmin = "AD";
        public const String TypeManagingDirector = "MD";
        public const String TypeConstructionsManager = "CM";
        public const String TypeContractsAdministrator = "CA";
        public const String TypeProjectManager = "PM";
        public const String TypeDesignCoordinator = "DC";
        public const String TypeDesignManager = "DM";
        public const String TypeSecretary = "RO";
        public const String TypeFinancialController = "FC";
        public const String TypeDirectorAuthorizacion = "DA";
        public const String TypeUnitManager = "UM";
        public const String TypeBudgetAdministrator = "BA";
        //#---
        public const String TypeCommercialManager = "CO";
        public const String TypeJuniorContractsAdministrator = "JC";
        public const String TypeContractsAdministrator3 = "CA3";
        public const String TypeContractsAdministrator4 = "CA4";
        public const String TypeContractsAdministrator5 = "CA5";
        public const String TypeContractsAdministrator6 = "CA6";
        //#---
        public const Int16 LengthRoleName = 2;
#endregion

#region Private Members
        private String position;
        private String role;
        private BusinessUnitInfo businessUnit;
#endregion

#region Constructors
        public EmployeeInfo()
        {
            Type = PeopleInfo.PeopleTypeEmployee;
        }

        public EmployeeInfo(int? employeeId)
        {
            Type = PeopleInfo.PeopleTypeEmployee;
            Id = employeeId;
        }
#endregion

#region Public properties
        public String Position
        {
            get { return position; }
            set { position = value; }
        }

        public String Role
        {
            get { return role; }
            set { role = value; }
        }

        public BusinessUnitInfo BusinessUnit
        {
            get { return businessUnit; }
            set { businessUnit = value; }
        }

        public String BusinessUnitName
        {
            get { return BusinessUnit != null ? BusinessUnit.Name : "All"; }
        }

        public String NameAndRole
        {
            get
            {
                if (Name != null)
                    if (Role != null)
                        return Role + " - " + Name;
                    else
                        return Name;
                else
                    if (Role != null)
                        return Role;
                    else
                        return String.Empty;
            }
        }
#endregion

    }
}
