using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
  public class GeneralInductionInfo :Info
    {
        #region Constants

        public const String GeneralInduction = "GI";
        public const String SiteInduction = "SI";

        #endregion

        #region Constructor
        public GeneralInductionInfo()
        {
            Type = GeneralInduction;
        }

        #endregion


        #region Properties
        public List<InductionDocumentsInfo> Documents { get; set; }

        public List<OptionalQAInfo>OptinalQAs{ get; set; }

        public List<YesNoQAInfo> YesNoQAs { get; set; }

        public InductionNoteInfo Note { get; set;}
        #endregion
    }



    [Serializable]
   public class OptionalQAInfo:Info 
    {
        #region Constructor 

        public OptionalQAInfo()
        { }

        public OptionalQAInfo(int? questionId)
        {
            Id = questionId;
        }
        #endregion


        #region Properties
        public string Question { get; set; }
        public string Opt1 { get; set; }
        public string Opt2 { get; set; }
        public string Opt3 { get; set; }
        public string Opt4 { get; set; }

        public string RightAnswer { get; set; }

        new public bool? IsActive { get; set; }

        public int? projectId { get; set; }
        #endregion

    }


  
    [Serializable]
    public class YesNoQAInfo : Info
    {
        #region Constructor 

        public YesNoQAInfo()
        { }

        public YesNoQAInfo(int? questionId)
        {
            Id = questionId;
        }
        #endregion


        #region Properties
        public string Question { get; set; }
        public string Comments { get; set; }
        public int? projectId { get; set; }
        new public bool? IsActive { get; set; }

        #endregion

    }


    public class InductionDocumentsInfo : Info
    {
        #region Constructor 

        public InductionDocumentsInfo()
        { }

        public InductionDocumentsInfo(int? sharedDocumentId)
        {
            Id = sharedDocumentId;
        }
        #endregion


        #region Properties
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public int? projectId { get; set; }

        public string State { get; set; }

        public string Version { get; set; }

        new public bool? IsActive { get; set; }

        #endregion

    }


    public class InductionNoteInfo : Info
    {
        #region Constructor 

        public InductionNoteInfo()
        { }

        public InductionNoteInfo(int? noteId)
        {
            Id = noteId;
        }
        #endregion

        #region Properties
        public string Note { get; set; } 
        public int? projectId { get; set; }
        new public bool? IsActive { get; set; }

        #endregion


    }

    public class InductionResultInfo : Info
    {
        #region Constructor 

        public InductionResultInfo()
        { }

        public InductionResultInfo(int? resultId)
        {
            Id = resultId;
        }
        #endregion

        #region Properties
       
        public int? PeopleId { get; set; }
        public int? SubcontractorId { get; set; }
        public string Name { get; set; }
        public string SubcontractorName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string InductionType { get; set; }
        public DateTime ResultDate { get; set; }
        new public bool? IsActive { get; set; }

        #endregion

    }


}
