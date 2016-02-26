using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour {

    public float m_Offset;

//Old ui
    //GameOver variables
    public float m_TransitionSpeed;
    public bool m_GameOver;
    public string m_GameOverMessage;
    public Texture m_GameOverBackgroundImage;
 
//New ui

    //Score counter
    public Text m_ScoreText;

    public float m_CurrentWidth;

    //Score Tracking
    private short m_BoxesPassed = 0;
    

    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_ScoreText.text = m_BoxesPassed.ToString();
	}

    public void IncrementBoxCounter()
    {
        m_BoxesPassed++;
    }

    public float GetCurrentScore()
    {
        return m_BoxesPassed;
    }

    void OnGUI()
    {

    }
}
