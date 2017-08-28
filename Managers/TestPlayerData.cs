using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestPlayerData : MonoBehaviour {

   public string Name = "Bob";
   public int Health = 100;
    public Vector3[] location; 

    private void Start()
    {
        location = new Vector3[3];
        Vector3 bob = new Vector3(0, 0, 0);
        Vector3 jim = new Vector3(1, 1, 0);
        Vector3 bill = new Vector3(2, 2, 2);

        location[0] = bob;
        location[1] = jim;
        location[2] = bill;
        

        
    }


}
