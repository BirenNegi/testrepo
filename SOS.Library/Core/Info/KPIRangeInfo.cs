using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
  public class KPIRangeInfo:Info
    {

        #region Construction
        public KPIRangeInfo()
        { }
        public KPIRangeInfo(int? KPIRangeId)
        {
            Id = KPIRangeId;

        }
        #endregion


        #region Public Properties

        public String KPI { get; set; }
        public String KPIDisplay { get; set; }

        public int TargetValue { get; set; }

        public int MinRange { get; set; }

        public int MaxRange { get; set; }

        public String KPIRange {
         get { return MinRange.ToString() + " - " + MaxRange.ToString(); }
        } 

        public string Color{ get; set; }

        //public int ColorPoint{ get; set; }



        #endregion






    }



    [Serializable]
    public class KPIPointsInfo : Info
    {

        #region Construction
        public KPIPointsInfo()
        { }
        public KPIPointsInfo(int? KPIPointsId)
        {
            Id = KPIPointsId;

        }
        #endregion


        #region Public Properties

        public String Color { get; set; }
        public int minPoints { get; set; }
        public int Points { get; set; }
      


        #endregion






    }


}
