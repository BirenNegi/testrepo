using System;
// DS20230821
namespace SOS.Core
{
    [Serializable]
    public class ProjectTradesInfo : Info
    {
        #region Public properties
        public int ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        #endregion
        public ProjectTradesInfo()
        {
        }
        #region Constructors
        #endregion
    }
}
