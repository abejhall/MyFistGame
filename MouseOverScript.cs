using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScript : MonoBehaviour {

    //bool _mouseOverButton;
	// Use this for initialization
	void Start () {
     //   _mouseOverButton = SelectionManager.Instance.mouseOverButton;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseButton()
    {
        SimplePool.Despawn(this.gameObject);
        Debug.Log("despawning red marker");
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
        JobManager.Instance.DisplayPanel.SetActive( true);
    }
    private void OnMouseExit()
    {
        JobManager.Instance.DisplayPanel.SetActive(false);
        MouseOFfButton();
    }
    private void OnMouseDown()
    {
        CloseButton();
    }
}
