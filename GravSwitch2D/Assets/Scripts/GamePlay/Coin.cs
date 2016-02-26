using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        { 
            //Increment coins
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
            Destroy(this.gameObject);
        }
    }	
}
