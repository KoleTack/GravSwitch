using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Charcaters
{ 
    Sanic
}

public class LevelMenu : MonoBehaviour {

    public bool m_LevelOver;

    public float m_MoveSpeed;
    private float m_ScreenWidth;
    private float m_PercentMoved;

    public GameObject[] m_GameOverObjects;

    public Text m_CurrentScore;
    public Text m_HighScore;

    private float[] m_GameOverStartingOffset;

    public float[] m_GameOverObjectStartTimerOffset;

	// Use this for initialization
	void Start () 
    {
        if(m_GameOverObjectStartTimerOffset.Length != m_GameOverObjects.Length)
        {
            m_GameOverObjectStartTimerOffset = new float[m_GameOverObjects.Length];

            for(int i = 0; i < m_GameOverObjects.Length; i++)
            {
                m_GameOverObjectStartTimerOffset[i] = 0;
            }
        }

        m_GameOverStartingOffset = new float[m_GameOverObjects.Length];

        for(int i = 0; i < m_GameOverObjects.Length; i++)
        {
            m_GameOverStartingOffset[i] = m_GameOverObjects[i].transform.position.x - Screen.width; //Non responsive;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(m_LevelOver)
        {
            MoveGameOverScreen();   
        }
	}

    public void GameOver()
    {
        m_LevelOver = true;
        SaveHighScore();
    }

    void MoveGameOverScreen()
    {
        for (int i = 0; i < m_GameOverObjects.Length; i++)
        {
            if(m_GameOverObjectStartTimerOffset[i] > 0)
            {
                m_GameOverObjectStartTimerOffset[i] -= Time.deltaTime;
                continue;
            }

            Vector3 DesiredPos = new Vector3(m_GameOverStartingOffset[i], m_GameOverObjects[i].transform.position.y, m_GameOverObjects[i].transform.position.z);
            m_GameOverObjects[i].transform.position = Vector3.MoveTowards(m_GameOverObjects[i].transform.position, DesiredPos, m_MoveSpeed * Time.deltaTime);
        }      
    }

    public void PlayAgain()
    {
        Application.LoadLevel(Application.loadedLevel);    
    }

    public void CharacterSelect()
    { 
        //Load charcater select scene
        Application.LoadLevel("CharacterSelectScreen");    
    }

    void SaveHighScore()
    {
        float currentScore = GetCurrentScore();
        float HighScore = PlayerPrefs.GetFloat("HighScore");

        m_CurrentScore.text = currentScore.ToString();
        m_HighScore.text = HighScore.ToString();

        if(HighScore < currentScore)
        {
            //New High Score!
            PlayerPrefs.SetFloat("HighScore", currentScore);   
        }
    }

    public void LoadCharacter(Charcaters characterToLoad)
    { 
    
    }

    float GetCurrentScore()
    {
        return GameObject.Find("Hud").GetComponent<Hud>().GetCurrentScore();
    }    
}
