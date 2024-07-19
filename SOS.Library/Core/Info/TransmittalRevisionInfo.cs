using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TransmittalRevisionInfo : Info
    {

#region Private Members
        private int? numCopies;

        private DrawingRevisionInfo revision;
        private TransmittalInfo transmittal;
#endregion

#region Constructors
        public TransmittalRevisionInfo() 
        {
        }

        public TransmittalRevisionInfo(int? transmittalRevisionInfoId)
        {
            Id = transmittalRevisionInfoId;
        }

        public TransmittalRevisionInfo(int? numCopies, DrawingRevisionInfo revision)
        {
            NumCopies = numCopies;
            Revision = revision;
        }

        public TransmittalRevisionInfo(int? numCopies, DrawingRevisionInfo revision, TransmittalInfo transmittal )
        {
            NumCopies = numCopies;
            Revision = revision;
            Transmittal = transmittal;
        }
#endregion

#region Public properties
        public int? NumCopies
        {
            get { return numCopies; }
            set { numCopies = value; }
        }

        public DrawingRevisionInfo Revision
        {
            get { return revision; }
            set { revision = value; }
        }

        public TransmittalInfo Transmittal
        {
            get { return transmittal; }
            set { transmittal = value; }
        }

        public DrawingInfo Drawing
        {
            get
            {
                return Revision != null ? Revision.Drawing : null;
            }
        }

        public int? DrawingId
        {
            get
            {
                return Drawing != null ? Drawing.Id : null;
            }
        }

        public String DrawingIdStr
        {
            get
            {
                return Drawing != null ? Drawing.IdStr : null;
            }
        }

        public String DrawingName
        {
            get
            {
                return Drawing != null ? Drawing.Name : null;
            }
        }

        public String DrawingDescription
        {
            get
            {
                return Drawing != null ? Drawing.Description : null;
            }
        }

        public DrawingTypeInfo DrawingType
        {
            get
            {
                return Drawing != null ? Drawing.DrawingType : null;
            }
        }

        public DrawingRevisionInfo DrawingLastRevision
        {
            get
            {
                return Drawing != null ? Drawing.LastRevision : null;
            }
        }

        public int? RevisionId
        {
            get
            {
                return Revision != null ? Revision.Id : null;
            }
        }

        public String RevisionIdStr
        {
            get
            {
                return Revision != null ? Revision.IdStr : null;
            }
        }

        public String RevisionName
        {
            get
            {
                return Revision != null ? Revision.Number : null;
            }
        }

        public DateTime? RevisionDate
        {
            get
            {
                return Revision != null ? Revision.RevisionDate : null;
            }
        }

        public String RevisionFile
        {
            get
            {
                return Revision != null ? Revision.File : null;
            }
        }
#endregion

    }
}
