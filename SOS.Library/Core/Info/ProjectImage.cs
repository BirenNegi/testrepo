using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
   public class ProjectImage :Info
    {
        #region Constructors
        public ProjectImage()
        {
        }

        public ProjectImage(int? ProjectImageId)
        {
            Id = ProjectImageId;
        }
        #endregion

        #region Public properties
        public int ProjectId { get; set; }
        public String ProjectName { get; set; }
        public String ParentFolder { get; set; }
        public String FolderName { get; set; }
        public String ImageName { get; set; }
        public byte[] ImageData { get; set; }

        #endregion

    }
}
   
