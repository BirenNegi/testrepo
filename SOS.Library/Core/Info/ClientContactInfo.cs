using System;
using System.Collections.Generic;

namespace SOS.Core
{ 
    [Serializable]
    public class ClientContactInfo : PeopleInfo
    {

#region Private Members
        private String companyName;
        //#----
        private Boolean? sendEots;
        private Boolean? sendRFIs;
        private Boolean? sendClaims;
        private Boolean? sendSAs;
        private Boolean? sendCVs;
        private Boolean? attentionEots;
        private Boolean? attentionRFIs;
        private Boolean? attentionClaims;

        private Boolean? sendTransmittals;

        private Boolean? accessEots;
        private Boolean? accessRFIs;
        private Boolean? accessCliams;
        private Boolean? accessSAs;
        private Boolean? accessCVs;
        private Boolean? accessDocs;
        private Boolean? accessPhotos;

        private List<ProjectInfo> projects;

        //#-----

        #endregion

        #region Constructors
        public ClientContactInfo()
        {
            Type = PeopleInfo.PeopleTypeClientContact;
        }

        public ClientContactInfo(int? clientContactId)
        {
            Type = PeopleInfo.PeopleTypeClientContact;
            Id = clientContactId;
        }
#endregion

#region Public properties
        public String CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        //#----

        public Boolean? SendEOTs
        {   get { return sendEots; }
            set { sendEots = value; }
        }
        public Boolean?  SendRFIs
        {
            get { return sendRFIs; }
            set { sendRFIs = value; }
        }
        public Boolean? SendClaims
        {
            get { return sendClaims; }
            set { sendClaims = value; }
        }
        public Boolean? SendSAs
        {
            get { return sendSAs; }
            set { sendSAs = value; }
        }
        public Boolean? SendCVs
        {
            get { return sendCVs; }
            set { sendCVs = value; }
        }
        public Boolean? AttentionToEots
        {
            get { return attentionEots; }
            set { attentionEots = value; }
        }
        public Boolean? AttentionToRFIs
        {
            get { return attentionRFIs; }
            set { attentionRFIs = value; }
        }
        public Boolean? AttentionToClaims
        {
            get { return attentionClaims; }
            set { attentionClaims = value; }
        }
        public Boolean? SendTransmittals
        {
            get { return sendTransmittals; }
            set { sendTransmittals = value; }
        }
        public Boolean? WebAccessToEOTs
        {
            get { return accessEots; }
            set { accessEots = value; }
        }
        public Boolean? WebAccessToRFIs
        {
            get { return accessRFIs; }
            set { accessRFIs = value; }
        }
        public Boolean? WebAccessToClaims
        {
            get { return accessCliams; }
            set { accessCliams = value; }
        }

        public Boolean? WebAccessToSAs
        {
            get { return accessSAs; }
            set { accessSAs = value; }
        }
        
        public Boolean? WebAccessToCVs
        {
            get { return accessCVs; }
            set { accessCVs = value; }
        }
        public Boolean? WebAccessToDocs
        {
            get { return accessDocs; }
            set { accessDocs = value; }
        }
        public Boolean? WebAccessToPhotos
        {
            get { return accessPhotos; }
            set { accessPhotos = value; }
        }

        public List<ProjectInfo> Projects
        {
            get { return projects; }
            set { projects = value; }
        }
        //#----


        #endregion

    }
}
