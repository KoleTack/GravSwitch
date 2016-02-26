using UnityEngine;
using System.Collections;

public class Obsticle : MonoBehaviour 
{
    private bool m_HasPassedPlayer;

    public bool PassedPlayer
    {
        get { return m_HasPassedPlayer; }
        set { m_HasPassedPlayer = value; }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Player hit us.
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().SetObsticleCollided();

            //Play obsticle hit animation.

            //Play player hit animation.
            other.GetComponent<GravitySwitch>().SetObsticleHit();
            other.GetComponent<AnimationSpeed>().SetObsticleHit();
        }
    }
}
