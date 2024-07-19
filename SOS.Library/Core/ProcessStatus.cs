using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace SOS.Core
{
    public class ProcessStatus
    {

#region Constants
        private const int SIZE_DETAILS_RECORD = 80;
        public const String END_PROCESS_CODE = "|End|";
        public const String DETAILS_SEPARATOR = "|";
        public const int POLLING_INTERVAL = 250;
        public const int REDIRECT_TIME = 2500;
#endregion

#region Private Members
        private Int32 percentageCompletion = 0;
        private List<String> processDetails = new List<String>();
        private Object synchronizationObject = new Object();
        private Boolean isComplete = false;
#endregion

#region Constructors
        public ProcessStatus()
        {
        }

        public ProcessStatus(List<String> processDetails, Int32 percentageCompletion)
        {
            this.processDetails = processDetails;
            this.percentageCompletion = percentageCompletion;
        }
#endregion

#region Public properties
        /// <summary>
        /// Returns an string concatenating all the process information.
        /// </summary>
        public String ProcessDetails
        {
            get
            {
                int numberOfDetails = processDetails.Count;

                if (numberOfDetails > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder(numberOfDetails * SIZE_DETAILS_RECORD);

                    foreach (String str in processDetails)
                        stringBuilder.Append(str).Append(DETAILS_SEPARATOR);

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);

                    return stringBuilder.ToString();
                }
                else
                {
                    if (!isComplete)
                    {
                        return String.Empty;
                    }
                    else
                    {
                        return END_PROCESS_CODE;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating if the process is complete or not
        /// </summary>
        public Boolean IsComplete
        {
            get { return isComplete; }
            set { isComplete = value; }
        }
#endregion

#region Public methods
        /// <summary>
        /// Adds a new message to the list of status updates and update the percentageCompletion.
        /// This method is synchronized because the list can also be updated by the thread the Gets the status updates.
        /// </summary>
        /// <param name="statusInfo">The new status update to add to the list</param>
        /// <param name="percentageCompletion">The process percentage completion</param>
        public void AddProcessStatusInfo(String statusInfo, Int32 percentageCompletion)
        {
            lock (synchronizationObject)
            {
                processDetails.Add(statusInfo);
                this.percentageCompletion = percentageCompletion;
            }
        }

        public void AddProcessStatusInfo(String statusInfo)
        {
            lock (synchronizationObject)
            {
                processDetails.Add(statusInfo);
            }

            Thread.Sleep(0);
        }

        /// <summary>
        /// Get the current list of status updates and the percentage completion and copies it to a new ProcessStatus object.
        /// Then clears the list of process status.
        /// This method is synchronized because the list can also be updated by the thread the adds status updates.
        /// </summary>
        /// <returns>Returns a ProcessStatus object with the current list of process status updates and percentage completion.</returns>
        public ProcessStatus GetStatusUpdate()
        {
            List<String> processDetailsClone = new List<String>();
            ProcessStatus processStatus = null;

            lock (synchronizationObject)
            {
                if (processDetails.Count > 0)
                {
                    foreach (String processDetail in processDetails)
                        processDetailsClone.Add(processDetail);

                    processDetails.Clear();
                }

                processStatus = new ProcessStatus(processDetailsClone, percentageCompletion);
            }

            return processStatus;
        }

        /// <summary>
        /// Returns the percentage completion of the process
        /// </summary>
        public Int32 PercentageCompletion
        {
            get { return percentageCompletion; }
            set {
                percentageCompletion = value;
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// Completes process including a final message.
        /// </summary>
        public void CompleteProcess(String statusInfo)
        {
            lock (synchronizationObject)
            {
                processDetails.Add(statusInfo);
                PercentageCompletion = 100;
                IsComplete = true;
            }
        }
#endregion

    }
}
