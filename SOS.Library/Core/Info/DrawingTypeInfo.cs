using System;

namespace SOS.Core
{
    [Serializable]
    public class DrawingTypeInfo : Info
    {

#region Private Members
        private String name;
        private int? numDrawings;
#endregion

#region Constructors
        public DrawingTypeInfo() 
        {
        }

        public DrawingTypeInfo(int? drawingTypeId)
        {
            Id = drawingTypeId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int? NumDrawings
        {
            get { return numDrawings; }
            set { numDrawings = value; }
        }
#endregion
    
    }
}
