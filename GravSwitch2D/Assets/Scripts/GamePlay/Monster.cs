using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour 
{
    //If player enters our trigger, kill him
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        { 
            //GameOver
            GameObject.Find("LevelMenu").GetComponent<LevelMenu>().GameOver();
        }
    }	
}
