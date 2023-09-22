using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOnePopup : Popup
{
    static OnlyOnePopup opendPopup;
    private void OnDestroy()
    {
        opendPopup = null;
    }
    new void Toggle()
    {
        base.Toggle();


        if (opendPopup != null)
        {
            opendPopup.Close();
        }
        opendPopup = isOpen ? this : null;
    }

}
