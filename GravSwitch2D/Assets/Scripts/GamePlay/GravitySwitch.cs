using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]

public class GravitySwitch : MonoBehaviour {

    public float m_SwipeSize;

    public Text m_DebugText;
    public Text m_DebugText2;

    private Rigidbody2D m_Rigidbody;
    private bool m_ObsticleHit = false;

    private bool m_SwitchGravity = false;
    public float m_SwipeSizeNeeded;
    private bool m_SwipeDirection;
    
    private List<Vector2> m_TouchStartLocation = new List<Vector2>();

    private Touch m_InitialTouch = new Touch();

    // Use this for initialization
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        SwipeDetection();    
    }

	// Update is called once per frame
	void Update () 
    {
        if(m_ObsticleHit)
        {
            return;
        }

       

        if (m_SwitchGravity)
        {
            if (m_SwipeDirection)
            {
                m_Rigidbody.gravityScale = Mathf.Abs(m_Rigidbody.gravityScale) * -1;
            }
            else
            {
                m_Rigidbody.gravityScale = Mathf.Abs(m_Rigidbody.gravityScale);
            }

            m_SwitchGravity = false;
        }
        

        //If swipe, toggle gravity for plus or minus.
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_Rigidbody.gravityScale = -m_Rigidbody.gravityScale;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_Rigidbody.gravityScale = Mathf.Abs(m_Rigidbody.gravityScale);
        }
    }

    public void SetObsticleHit()
    { 
       m_ObsticleHit = true;
    }

    public void SwipeDetection()
    {

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                m_InitialTouch = t;   
            }
            else if (t.phase == TouchPhase.Moved)
            {
                float DistanceSwiped = t.position.y - m_InitialTouch.position.y;
                float DistanceSwipedAbs = Mathf.Abs(DistanceSwiped);

                if(DistanceSwipedAbs > m_SwipeSizeNeeded)
                { 
                    //Do stuff
                    m_SwitchGravity = true;

                    if (DistanceSwiped > 0)
                    {
                        m_SwipeDirection = true;
                    }
                    else
                    {
                        m_SwipeDirection = false;
                    }
                }
            }
            else if (t.phase == TouchPhase.Began)
            {
                m_InitialTouch = new Touch();
            }        
        
        }

        /*
        if (Input.touchCount < 1)
        {
            return;
        }
        else
        {
            m_DebugText2.text = Input.touchCount.ToString(); 
        }
// go
        Touch[] touches = Input.touches;

        for (int i = 0; i > touches.Length; i++)
        {
            switch (touches[i].phase)
            {
                case TouchPhase.Began:
                    m_TouchStartLocation[i] = touches[i].position;
                break;

                case TouchPhase.Ended:
                    //get the y distance to see if swipe was big enough.
                    float verticalFloatDist = touches[i].position.y - m_TouchStartLocation[i].y;


                    m_DebugText.text = "Swipe";

                    //Distanc could be negative, get absolute value.
                    if(Mathf.Abs(verticalFloatDist) > m_SwipeSizeNeeded)
                        { 
                            
                        //Get Swipe direction
                            if (verticalFloatDist > 0)
                            {
                                m_SwipeDirection = true;
                            }
                            else
                            {
                                m_SwipeDirection = false;
                            }
                        }                    
                break;

                default:

                break;
         * 
        
            }
         }
      */      
    }
}
