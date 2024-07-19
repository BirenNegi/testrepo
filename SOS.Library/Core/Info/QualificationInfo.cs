using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Core
{
    [Serializable]
   public class QualificationInfo:Info
    {

        #region Constructors
        public QualificationInfo()
        {
           
        }

        public QualificationInfo(int? qualificationId)
        {
             Id = qualificationId;
        }
        #endregion

        public ContactInfo contactInfo { get; set; }
        public String qualificationName { get; set; }
        public String cardNumber { get; set; }
        public DateTime? expiryDate { get; set; }
        public String imageName { get; set; }
        public String imagePath { get; set; }
         
        

    }
}
