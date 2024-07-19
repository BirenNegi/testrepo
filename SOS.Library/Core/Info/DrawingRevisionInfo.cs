using System;

namespace SOS.Core
{
    [Serializable]
    public class DrawingRevisionInfo : Info
    {

#region Private Members
        private String number;
        private String comments;
        private String file;
        private DateTime? revisionDate;
        private DrawingInfo drawing;
#endregion

#region Constructors
        public DrawingRevisionInfo() 
        {
        }

        public DrawingRevisionInfo(int? drawingRevisionId)
        {
            Id = drawingRevisionId;
        }

        public DrawingRevisionInfo(DrawingInfo drawing)
        {
            Drawing = drawing;
        }
#endregion

#region Public properties
        public String Number
        {
            get { return number; }
            set { number = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public String File
        {
            get { return file; }
            set { file = value; }
        }

        public DateTime? RevisionDate
        {
            get { return revisionDate; }
            set { revisionDate = value; }
        }

        public DrawingInfo Drawing
        {
            get { return drawing; }
            set { drawing = value; }
        }

        public String DrawingName
        {
            get { return Drawing != null ? Drawing.Name : String.Empty; }
        }
#endregion

    }
}
