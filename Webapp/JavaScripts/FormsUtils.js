function CheckUncheckAll(parentControlId, childrenControlsIds, excludedControlsIds)
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


//--San
//function CheckAll(oCheckbox)
//{
//    var GridView2 = document.getElementById("<%=gvDrawings.ClientID %>");
//    for (i = 1; i < GridView2.rows.length; i++) {
//        GridView2.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
//    }
//}


//#---

