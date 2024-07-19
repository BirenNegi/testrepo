function ReceiveServerDataWithConfigutation(eventArgument, context, endProcessCode, separator, pollingInterval, redirectTime, progressControl, urlControl)
{
   var endTagIndex = eventArgument.indexOf(endProcessCode);
   var eventsInfo = "";
   var strUrl = "";

   if (endTagIndex != -1)
   {
      strUrl = eventArgument.substring(endTagIndex + endProcessCode.length);
      if (endTagIndex != 0)
      {
         eventsInfo = eventArgument.substring(0, endTagIndex);
      }
   }
   else
   {
      eventsInfo = eventArgument;
   }

   if (eventsInfo != "")
   {
      var tokens = eventsInfo.split(separator);
      var txtControl = document.getElementById(progressControl);
      for (var i=0; i<tokens.length; i++)
      {
         txtControl.value = txtControl.value + tokens[i] + "\n";
         txtControl.scrollTop = txtControl.scrollHeight;
      }
   }

   if (endTagIndex != -1)
   {
      if (strUrl != "")
      {
         document.getElementById(urlControl).innerText = "Redirecting to view project page...";
         setTimeout("window.location = '" + strUrl + "';", redirectTime);
      }
   }
   else
   {
      setTimeout("CallServer()", pollingInterval);
   }
}
