using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelComponents : MonoBehaviour {


    public Text PanelNameText;
    public Slider PanelHelthSlider;
    public Slider PanelHungerSlider;

    public Image PanelCharacterUIImage;

    public GameObject MyCharacter;


    private void Update()
    {
        if(MyCharacter != null)
        {
           PanelNameText.text = MyCharacter.GetComponent<CharacterStats>().Name;
           PanelHelthSlider.value = MyCharacter.GetComponent<CharacterStats>().Health /100;
           
           PanelHungerSlider.value =  MyCharacter.GetComponent<CharacterStats>().Food /100;
        }
        
    }
}
