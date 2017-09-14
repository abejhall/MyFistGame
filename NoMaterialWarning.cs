using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaterialWarning : MonoBehaviour {

    public void CloseButton()
    {
        SimplePool.Despawn(this.gameObject);
        //   Debug.Log("despawning red marker");
        JobManager.Instance.DisplayPanel.SetActive(false);
        SelectionManager.Instance.mouseOverButton = false;
    }
    public void MouseOverButton()
    {
        SelectionManager.Instance.mouseOverButton = true;
    }

    public void MouseOFfButton()
    {
        SelectionManager.Instance.mouseOverButton = false;
    }


    private void OnMouseEnter()
    {
        MouseOverButton();
        ToolTipManager.Instance.MouseOver("I dont have materials to do this job right now.  Trying mining rocks, or chopping trees first.", "Click on Red Box TO CLOSE");
        // JobManager.Instance.DisplayPanel.SetActive( true);
    }
    private void OnMouseExit()
    {
        ToolTipManager.Instance.MouseLeavesTTZ();
        MouseOFfButton();
    }
    private void OnMouseDown()
    {
        CloseButton();
    }
}
