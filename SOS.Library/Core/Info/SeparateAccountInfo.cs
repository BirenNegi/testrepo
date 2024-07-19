using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class SeparateAccountInfo : ClientVariationInfo
    {

#region Constructors
        public SeparateAccountInfo() 
        {
            Type = ClientVariationInfo.VariationTypeSeparateAccounts;
        }

        public SeparateAccountInfo(int? separateAccountInfoId)
        {
            Type = ClientVariationInfo.VariationTypeSeparateAccounts;
            Id = separateAccountInfoId;
        }
#endregion

#region Public properties
        public int? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceSentDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public DateTime? InvoicePaidDate { get; set; }
        public DateTime? WorksCompletedDate { get; set; }
        public Boolean? UseSecondPrincipal { get; set; }

        public override String ProcessType
        {
            get { return ProcessTemplateInfo.ProcessTypeSeparateAccounts; }
        }

        public Boolean WorksCompleted
        {
            get { return WorksCompletedDate != null; }
        }

        public Boolean InvoiceSent
        {
            get { return InvoiceSentDate != null; }
        }

        public Boolean InvoicePaid
        {
            get { return InvoicePaidDate != null; }
        }

        public Boolean IsSecondPrincipal
        {
            get { return UseSecondPrincipal != null && UseSecondPrincipal.Value; }
        }

        public override String Status
        {
            get
            {
                if (IsCancel)
                    return StatusCancelled;
                else if (InvoicePaid)
                    return StatusPaid;
                else if (InvoiceSent)
                    return StatusInvoiced;
                else if (WorksCompleted)
                    return StatusWorksCompleted;
                else if (IsApproved)
                    return StatusApproved;
                else if (IsInternallyApproved)
                    return StatusToBeApproved;
                else
                    return StatusTobeIssued;
            }
        }

        public String Principal
        {
            get
            {
                return IsSecondPrincipal ? Project.Principal2 : Project.Principal;
            }
        }

        public String PrincipalABN
        {
            get
            {
                return IsSecondPrincipal ? Project.Principal2ABN : Project.PrincipalABN;
            }
        }

        public String ContactName
        {
            get
            {
                //#--  return IsSecondPrincipal ? Project.SecondPrincipal.Name : Project.ClientContact.Name;
                return IsSecondPrincipal ? Project.SecondPrincipal.Name : Project.ClientContactList[0].Name; //--#
            }
        }

        public String ContactStreet
        {
            get
            {
                //#-- return IsSecondPrincipal ? Project.SecondPrincipal.Street : Project.ClientContact.Street;

                return IsSecondPrincipal ? Project.SecondPrincipal.Street : Project.ClientContactList[0].Street;    //--#
            }
        }

        public String ContactLocality
        {
            get
            {
                //#-- return IsSecondPrincipal ? Project.SecondPrincipal.Locality : Project.ClientContact.Locality;
                return IsSecondPrincipal ? Project.SecondPrincipal.Locality : Project.ClientContactList[0].Locality;//--#
            }
        }

        public String ContactState
        {
            get
            {
                //#-- return IsSecondPrincipal ? Project.SecondPrincipal.State : Project.ClientContact.State;
                return IsSecondPrincipal ? Project.SecondPrincipal.State : Project.ClientContactList[0].State;  //#
            }
        }

        public String ContactPostalCode
        {
            get
            {
                //#--return IsSecondPrincipal ? Project.SecondPrincipal.PostalCode : Project.ClientContact.PostalCode;
                return IsSecondPrincipal ? Project.SecondPrincipal.PostalCode : Project.ClientContactList[0].PostalCode;//--#
            }
        }
#endregion

    }
}
