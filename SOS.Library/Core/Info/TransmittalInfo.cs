using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TransmittalInfo : Info
    {

#region Constants
        public const String DeliveryMethodEmail = "EM";
        //#--
        public const String DeliveryMethodDownloadFromSOS = "DLS";//#---
#endregion

#region Private Members
        private int? transmittalNumber;
        private DateTime? transmissionDate;
        private String deliveryMethod;
        private String transmittalType;
        private String requiredAction;
        private String transmittalTypeOther;
        private String requiredActionOther;
        private String comments;
        private bool? sendClientContact;
        private bool? sendClientContact1;
        private bool? sendClientContact2;
        private bool? sendSuperintendent;
        private bool? sendQuantitySurveyor;
        private DateTime? sentDate;

        private ContactInfo contact;
        private ProjectInfo project;
        
        private List<ContactInfo> contacts;
        private List<TransmittalRevisionInfo> transmittalRevisions;

        //#---
        private List<ClientContactInfo> clientContacts;

        #endregion

        #region Constructors
        public TransmittalInfo() 
        {
        }

        public TransmittalInfo(int? transmittalInfoId)
        {
            Id = transmittalInfoId;
        }
#endregion

#region Public properties
        public int? TransmittalNumber
        {
            get { return transmittalNumber; }
            set { transmittalNumber = value; }
        }

        public DateTime? TransmissionDate
        {
            get { return transmissionDate; }
            set { transmissionDate = value; }
        }

        public String DeliveryMethod
        {
            get { return deliveryMethod; }
            set { deliveryMethod = value; }
        }

        public String TransmittalType
        {
            get { return transmittalType; }
            set { transmittalType = value; }
        }

        public String RequiredAction
        {
            get { return requiredAction; }
            set { requiredAction = value; }
        }

        public String TransmittalTypeOther
        {
            get { return transmittalTypeOther; }
            set { transmittalTypeOther = value; }
        }

        public String RequiredActionOther
        {
            get { return requiredActionOther; }
            set { requiredActionOther = value; }
        }

        public String Comments
        {
            get { return  comments; }
            set { comments = value; }
        }

        public bool? SendClientContact
        {
            get { return sendClientContact; }
            set { sendClientContact = value; }
        }

        public bool? SendClientContact1
        {
            get { return sendClientContact1; }
            set { sendClientContact1 = value; }
        }

        public bool? SendClientContact2
        {
            get { return sendClientContact2; }
            set { sendClientContact2 = value; }
        }

        public bool? SendSuperintendent
        {
            get { return sendSuperintendent; }
            set { sendSuperintendent = value; }
        }

        public bool? SendQuantitySurveyor
        {
            get { return sendQuantitySurveyor; }
            set { sendQuantitySurveyor = value; }
        }

        public DateTime? SentDate
        {
            get { return sentDate; }
            set { sentDate = value; }
        }

        public ContactInfo Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public List<ContactInfo> Contacts
        {
            get { return contacts; }
            set { contacts = value; }
        }

        public List<TransmittalRevisionInfo> TransmittalRevisions
        {
            get { return transmittalRevisions; }
            set { transmittalRevisions = value; }
        }

//#--
        public List<ClientContactInfo> ClientContacts
        {
            get { return clientContacts; }
            set { clientContacts = value; }
        }
//#--


        public SubContractorInfo SubContractor
        {
            get { return Contact != null ? Contact.SubContractor : null; }
        }

        public String Name
        {
            get
            {
                String typeStr = TransmittalType != null ? Web.Utils.GetConfigListItemNameAndOther("Transmittals", "TransmittalType", TransmittalType, TransmittalTypeOther) : "No Type";
                String numberStr = TransmittalNumber != null ? UI.Utils.SetFormInteger(TransmittalNumber) : "No Number";
                String dateStr = TransmissionDate != null ? UI.Utils.SetFormDate(TransmissionDate) : "No Date";

                return typeStr + " - (No. " + numberStr + ")" + " - " + dateStr;
            }
        }

        public String SubContractorName
        {
            get { return SubContractor != null ? SubContractor.Name : null; }
        }

        public String SubContractorShortName
        {
            get { return SubContractor != null ? SubContractor.ShortName : null; }
        }

        public String ContactName
        {
            get { return Contact != null ? Contact.Name : null; }
        }

        public String FirstRevisionNumber
        {
            get
            {
                return TransmittalRevisions != null && TransmittalRevisions.Count > 0 && TransmittalRevisions[0].Revision != null ? TransmittalRevisions[0].Revision.Number : null;
            }
        }

        public Boolean IsSent
        {
            get { return SentDate != null; }
        }

        public Int32? NumDrawings
        {
            get
            {
                if (TransmittalRevisions != null)
                    return TransmittalRevisions.Count;

                return null;
            }
        }

        public String TransmittalNumberStr
        {
            get { return TransmittalNumber.HasValue ? Convert.ToString(TransmittalNumber.Value) : null; }
        }

            

        #endregion

    }
}
