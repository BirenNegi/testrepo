using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
   public class RFIsResponseInfo : Info
    {
        #region Constructors
        public RFIsResponseInfo()
        {
        }

        public  RFIsResponseInfo(int? ResponseId)
        {
            Id = ResponseId;
        }
        #endregion


        #region Public Properties
        public int? ResponseNumber{ get; set; }
        public String Responsemessage { get; set; }

        public String ResponseFolderPath { get; set;}

        public RFIInfo RFI { get; set; }

        public PeopleInfo ResponseFrom { get; set; }

        public List<RFIsResponseAttachmentInfo>ResponseAttachments{ get; set; }


        #endregion

    }




    [Serializable]
    public class RFIsResponseAttachmentInfo : Info
    {

      public  RFIsResponseAttachmentInfo() {

        }


        public RFIsResponseAttachmentInfo(int? AttachmentId)
        {
            Id = AttachmentId;

        }

        #region Properties

        public RFIInfo RFI { get; set; }
        public RFIsResponseInfo RFIsResponse { get; set; }

        public String FileName { get; set; }
        public String FileExtension { get; set; }

        public byte[] FileData { get; set; }
        #endregion


    }
}
