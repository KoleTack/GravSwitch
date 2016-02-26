using UnityEngine;
using System.Collections;

public class SetPlayerPrefsForTesting : MonoBehaviour {

    public string[] m_Characters;
    public int[] m_CharacterCost;
    public int m_Coins;

	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < m_Characters.Length; i++)
        {
            PlayerPrefs.SetInt(m_Characters[i], 0);
            PlayerPrefs.SetInt(m_Characters[i] + "Cost", m_CharacterCost[i]);
        }

        PlayerPrefs.SetInt("Coins", m_Coins);
	}
}
