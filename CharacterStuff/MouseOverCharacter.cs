using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverCharacter : MonoBehaviour {

    public GameObject canvasHolder;
    public GameObject Character;
    public Text nameText;
    CharacterStats charstats;
    Color green = new Color32(0, 255, 0, 255);
    Color Yellow = new Color32(255, 255, 0, 255);
    Color red = new Color32(255, 0, 0, 255);
    void Start () {
        charstats = GetComponent<CharacterStats>();
        canvasHolder.SetActive(false);
       
	}

    private void Update()
    {
        UpDateTextColor();

        if (nameText.text != charstats.Name && charstats != null)
        {
            nameText.text = charstats.Name;
        }

        if(Character.transform.rotation.y == 180)
        {
            canvasHolder.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            canvasHolder.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnMouseOver()
    {
        canvasHolder.SetActive(true);
        SelectionManager.Instance.mouseOverButton = true;
    }

    private void OnMouseExit()
    {
        canvasHolder.SetActive(false);
        SelectionManager.Instance.mouseOverButton = false;
    }

    private void OnMouseDown()
    {
        TogglePannel.Instance.ToggleCharacterButtons();
    }


    void UpDateTextColor()
    {
        if(charstats.Health > 75f)
        {
            nameText.color = green;
        }
        if (charstats.Health > 35f && charstats.Health < 75f)
        {
            nameText.color = Yellow;
        }
        if (charstats.Health < 35f)
        {
            nameText.color = red;
        }

    }



}
