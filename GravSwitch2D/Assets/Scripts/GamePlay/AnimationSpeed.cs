using UnityEngine;
using System.Collections;

/*
 * This class will be used to control the transitions in animation of our player. 
 */

public class AnimationSpeed : MonoBehaviour {

    Animator m_Anim;
    GameManager m_GameManager;
    BoxCollider2D m_Collider;

    bool m_ObsticleHit = false;

	// Use this for initialization
	void Start () 
    {
        m_Collider = GetComponent<BoxCollider2D>();
        m_Anim = GetComponent<Animator>();
        m_GameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        float PlayerSpeed =  m_GameManager.GetPlayerSpeed();
        
        m_Anim.SetFloat("Speed", PlayerSpeed);

        if (PlayerSpeed < 0.1)
        {
            m_Anim.speed = 1;
        }
        else
        {
            m_Anim.speed = (PlayerSpeed / 20) + 1;
        }

        //Check if touching ground or ceiling.
        //If ceiling, scale = negaitve 1;
        bool InAir = true;

        Vector2 Origin = new Vector2( transform.position.x, transform.position.y);
        Vector2 Direction = new Vector2 (0, 1);
        float Length = (m_Collider.size.y / 2) + 0.1f;
        
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, Length, 0));
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, -Length, 0));

        RaycastHit2D HitInfo = Physics2D.Raycast(Origin, Direction, Length);

        if(HitInfo.collider != null)
        {
            transform.localScale = new Vector3( 1.0f, -1.0f, 1.0f);
            InAir = false;
        }

        Direction.y = -1;
        HitInfo = Physics2D.Raycast(Origin, Direction, Length);

        if (HitInfo.collider != null)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            InAir = false;
        }

        m_Anim.SetBool("OnGround", InAir);
    }


    public void SetObsticleHit()
    { 
        m_ObsticleHit = true;
        m_Anim.SetBool("Dead", m_ObsticleHit);
    }
}
