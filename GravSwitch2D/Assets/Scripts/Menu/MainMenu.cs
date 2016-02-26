using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public float m_ButtonSpacing;

    public GameObject[] m_Buttons;
    public Text m_CoinsText;

    public float m_DistanceBeforeMovement;
    public float m_MovementSpeed;
    public float m_Acceleration;

    public float m_CurrentMovementSpeed;
    private float m_StartingPosX;
    private bool m_Moving;
    private float m_PreviousMousePosX;

//Variables for dynamic button scaling based on position relative to center of screen.
    public float m_MaxButtonSize;
    public float m_MaxDistanceFromCenter;
    public float m_MinButtonSize;   
    
    public GameObject m_CurrentButton;
    public GameObject m_BuyButton;

    public int Coins;



	// Use this for initialization
	void Start ()
    {

        Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToPortrait = false;

        for(int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].transform.position = new Vector3(i * m_ButtonSpacing * Screen.width + (Screen.width * 0.5f), m_Buttons[i].transform.position.y, m_Buttons[i].transform.position.z);

            if (PlayerPrefs.GetInt(m_Buttons[i].GetComponentInChildren<Text>().text) < 0.5f)
            {
                if (i > 1)
                {
                    m_Buttons[i].GetComponent<Button>().interactable = false;
                }
            }

            Debug.Log(m_Buttons[i].GetComponentInChildren<Text>().text);
        }

        m_CurrentButton = m_Buttons[0];

        Coins = PlayerPrefs.GetInt("Coins");
	}
	
	// Update is called once per frame
	void Update () 
    {

        m_CoinsText.text = Coins.ToString(); 

        //
        if(Input.GetMouseButtonDown(0))
        {
            m_StartingPosX = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_Moving = false;
        }

        //check if fingure is down. 
        if(Input.GetMouseButton(0))
        { 
            float DistanceMoved = Mathf.Abs(m_StartingPosX - Input.mousePosition.x);
            if (DistanceMoved > m_DistanceBeforeMovement)
            {
                m_Moving = true;
            }
        }

        if (m_Moving)
        {
            float MouseDeltaPosX = Input.mousePosition.x - m_PreviousMousePosX;
            float ButtonMove = CalcButtonVelocity(MouseDeltaPosX);
            MoveButtons(ButtonMove);
        }
        else
        {
            float ButtonMove = CalcButtonVelocity(0);
            MoveButtons(ButtonMove);
        }

        if(Input.GetMouseButton(0))
        {
            m_PreviousMousePosX = Input.mousePosition.x;
        }

        UpdateBuyButton();    

	}

    void UpdateBuyButton()
    {
        int HasBeenBought = PlayerPrefs.GetInt(m_CurrentButton.GetComponentInChildren<Text>().text);
        int CostOfCharacter = PlayerPrefs.GetInt(m_CurrentButton.GetComponentInChildren<Text>().text + "Cost");
                
        if (HasBeenBought < 0.5f && Coins > CostOfCharacter)
        {
            m_BuyButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_BuyButton.GetComponent<Button>().interactable = false;
        }

        m_BuyButton.GetComponentInChildren<Text>().text = "Buy: " + CostOfCharacter.ToString(); 
    }

    void ResizeButton()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            //Store our button x position
            float buttonXPos = m_Buttons[i].transform.position.x;

            buttonXPos = Mathf.Abs(buttonXPos);

            if(buttonXPos > m_MaxDistanceFromCenter)
            {
                /*
               //make button small
                Vector3 newScale =  

                m_Buttons[i].transform.localScale =
                 */
            }
            else
            {
                //scale dynamicly
                
            }


        }
    }

    void MoveButtons(float Amount)
    {
        float m_MaxButtonPosx = m_Buttons.Length * m_ButtonSpacing;

    /*    if (m_Buttons[0].transform.position.x > Screen.width * 0.5f && Amount > 0)
        {
            return;
        }
    
        if (m_Buttons[m_Buttons.Length - 1].transform.position.x < Screen.width * 0.5f && Amount < 0)
        {
            return;
        }
    */
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].transform.position = new Vector3(m_Buttons[i].transform.position.x + Amount, m_Buttons[i].transform.position.y, m_Buttons[i].transform.position.z);

            float CurrentButtonDistanceFromCenter = Mathf.Abs(m_CurrentButton.transform.position.x - Screen.width * 0.5f);
            float ThisButtonDistance = Mathf.Abs(m_Buttons[i].transform.position.x - Screen.width * 0.5f);

            if( CurrentButtonDistanceFromCenter > ThisButtonDistance)
            {
                m_CurrentButton = m_Buttons[i];
            }
        }
    }

    float CalcButtonVelocity(float MouseDelta)
    {
        float VelocityDeltaThisFrame = MouseDelta * (m_MovementSpeed * Time.deltaTime);

//      float Acceleration = Mathf.MoveTowards(m_CurrentMovementSpeed, 0, m_Acceleration * Time.deltaTime);

        m_CurrentMovementSpeed = VelocityDeltaThisFrame; // + m_CurrentMovementSpeed -Acceleration;
        return m_CurrentMovementSpeed;
    }

    public void LoadLevel(string LevelToLoad)
    {
        if(m_Moving)
        {
            return;
        }

        Application.LoadLevel(LevelToLoad);
    }

    public void BuyCharacter()
    {
        PlayerPrefs.SetInt(m_CurrentButton.GetComponentInChildren<Text>().text, 1);
        m_CurrentButton.GetComponent<Button>().interactable = true;
        Coins -= PlayerPrefs.GetInt(m_CurrentButton.GetComponentInChildren<Text>().text + "Cost");
        PlayerPrefs.SetInt("Coins", Coins);
    }
}
