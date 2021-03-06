﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{

    public GameObject PauseMenu;
    public bool PauseMenueOpen = false;

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenueOpen)
            {
                SelectionManager.Instance.mouseOverButton = false;
            }
            PauseMenueOpen = !PauseMenueOpen;
        }

      

        Time.timeScale = PauseMenueOpen ? 0 : 1;

        if (PauseMenueOpen)
        {
            PauseMenu.SetActive(true);
        }

        if (!PauseMenueOpen)
        {
            PauseMenu.SetActive(false);

        }

       
    }


    public void Resume()
    {
        PauseMenueOpen = false;
        SelectionManager.Instance.mouseOverButton = false;
    }

   public void QuitGame()
    {
        Application.Quit();
    }

}
