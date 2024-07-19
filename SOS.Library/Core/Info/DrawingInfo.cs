using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class DrawingInfo : Info
    {

#region Constants
        public const String DeepZoomCodeLocal = "Local";
        public const String DeepZoomCodeRemote = "Remote";
#endregion

#region Private Members
        private String name;
        private String description;
        private DrawingTypeInfo drawingType;
        private ProjectInfo project;
        private List<TradeInfo> trades;
        private List<DrawingRevisionInfo> drawingRevisions;
        private List<TransmittalInfo> transmittals;
#endregion

#region Constructors
        public DrawingInfo() 
        {
        }

        public DrawingInfo(int? drawingId)
        {
            Id = drawingId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public DrawingTypeInfo DrawingType
        {
            get { return drawingType; }
            set { drawingType = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public List<TradeInfo> Trades
        {
            get { return trades; }
            set { trades = value; }
        }

        public List<DrawingRevisionInfo> DrawingRevisions
        {
            get { return drawingRevisions; }
            set { drawingRevisions = value; }
        }

        public List<TransmittalInfo> Transmittals
        {
            get { return transmittals; }
            set { transmittals = value; }
        }

        public DrawingRevisionInfo LastRevision
        {
            get
            {
                if (DrawingRevisions != null)
                    if (DrawingRevisions.Count > 0)
                        return DrawingRevisions[0];

                return null;
            }
        }

        public String LastRevisionIdStr
        {
            get
            {
                if (LastRevision != null)
                    return LastRevision.IdStr;

                return null;
            }
        }

        public String LastRevisionNumber
        {
            get
            {
                if (LastRevision != null)
                    return LastRevision.Number;

                return null;
            }
        }

        public DateTime? LastRevisionDate
        {
            get
            {
                if (LastRevision != null)
                    return LastRevision.RevisionDate;

                return null;
            }
        }

        public String LastRevisionFile
        {
            get
            {
                if (LastRevision != null)
                    return LastRevision.File;

                return null;
            }
        }

        public int? NumRevisions
        {
            get
            {
                if (DrawingRevisions != null)
                    return DrawingRevisions.Count;

                return null;                   
            }
        }

        public int? NumTrades
        {
            get
            {
                if (Trades != null)
                    return Trades.Count;

                return null;
            }
        }

        public String Summary
        {
            get
            {
                return LastRevision != null ? Name + "-" + Description + " " + LastRevisionNumber : null;
            }
        }
#endregion

    }
}
