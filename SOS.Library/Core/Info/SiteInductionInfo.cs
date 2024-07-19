using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Core
{
    

    [Serializable]
    public class SiteInductionInfo: GeneralInductionInfo 
    {
        #region Properties

        public ProjectInfo Project { get; set; }


        #endregion

        #region Constructor
        public SiteInductionInfo()
        {
            Type = GeneralInductionInfo.SiteInduction;
        }

        #endregion


      
    }


}
