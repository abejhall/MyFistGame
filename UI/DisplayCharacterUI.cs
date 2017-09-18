using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharacterUI : MonoBehaviour {

    public GameObject PlayerUIPrefab;
    public GameObject ParentPanel;


    string PlayerNameTextBox;

    Sprite PlayerUISprite;
    Slider HealthUISlider;
    Slider HungerUISlider;
    List<GameObject> PlayerPaners;

   // CharacterStats characterStats; //removing cause it says i dont use
#region Singleton
    public static DisplayCharacterUI Instance;
    private void Awake()
    {
        Instance = this;
    }
#endregion

    // Use this for initialization
    void Start () {
        PlayerPaners = new List<GameObject>();
     //  Invoke("GetCharacterUIs",3f);

    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log("Number Of Characters Showing in list: " + WorldManager.Instance.PlayerCharacters.Count);
    }


   public void GetCharacterUIs()
    {
        if (PlayerPaners.Count > 0)
        {
            foreach (GameObject RGO in PlayerPaners)
            {
                SimplePool.Despawn(RGO);

            }
            PlayerPaners.Clear();
        }



        //Debug.Log("Number Of Characters Showing in list: " + WorldManager.Instance.PlayerCharacters.Count);
        foreach (GameObject go in WorldManager.Instance.PlayerCharacters)
        {
           
           
           GameObject PGO = SimplePool.Spawn(PlayerUIPrefab, ParentPanel.transform.position,ParentPanel.transform.rotation);
            PlayerPaners.Add(PGO);
            PGO.transform.SetParent(ParentPanel.transform);
            PGO.name = "Character:" +go.GetComponent<CharacterStats>().Name;
            PGO.GetComponent<PanelComponents>().MyCharacter = go;
            //   characterStats = go.GetComponent<CharacterStats>(); //removing cause it says i dont use
            // PGO.GetComponent<PanelComponents>().ThisCharactersStats = characterStats;
            // PGO.GetComponent<PanelComponents>().ThisCharactersStats.name = characterStats.name;
        }
    }
   

}
