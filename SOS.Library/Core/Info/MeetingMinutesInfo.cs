using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
    public class MeetingMinutesInfo : Info
    {

        #region Constructors
        public MeetingMinutesInfo()
        {
        }

        public MeetingMinutesInfo(int? MeetingId)
        {
            Id = MeetingId;
        }
        #endregion


        #region Public properties
        public int? Number { get; set; }
        public String Subject { get; set; }
      
        public DateTime? MeetingDate { get; set; }
        public String MeetingTime { get; set; }
        public String  Location { get; set; }
        public String Attendees { get; set; }
        public String ReferenceFile { get; set; }
        public PeopleInfo Signer { get; set; }
        public ProjectInfo Project { get; set; }
        public int? TypeNumber { get; set; }

        #endregion



    }

}