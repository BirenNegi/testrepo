﻿function CheckUncheckAll(parentControlId, childrenControlsIds, excludedControlsIds)
{
    var numChildren = childrenControlsIds.length;
    var numExcluded;
    var parentControl;
    var childControl;
    var isExcluded;
    var i;
    var j;
        
    parentControl = document.getElementById(parentControlId);
    
    if (excludedControlsIds != null)
        numExcluded = excludedControlsIds.length;
    
    for (i=0; i<numChildren; i++)
    {
        childControl = document.getElementById(childrenControlsIds[i]);
        
        isExcluded = false;
        if (excludedControlsIds != null)
            for (j=0; j<numExcluded; j++)
            {
               if (excludedControlsIds[j] == childrenControlsIds[i])
               {
                   isExcluded = true;
                   break;
               }
            }
                        
        if (!isExcluded)
            childControl.checked = parentControl.checked;
        else
            childControl.checked = false;
    }
}

function CheckUncheck(thisControlID, otherControlId)
{
    if (document.getElementById(thisControlID).checked)
       document.getElementById(otherControlId).checked = false;
}
