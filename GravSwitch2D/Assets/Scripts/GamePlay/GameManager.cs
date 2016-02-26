using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//This class will move all the game objects

public class GameManager : MonoBehaviour {

    public float m_ObjectSpawnTime;
    public float m_ObjectSpawnDeduction;

    public float m_PlayerSpeedIncrease;
    public float m_PlayerSpeedDecrease;
    public float m_MonsterSpeed;
    public float m_MonsterSpeedIncrement;
    public float m_MaxMonsterDistance;
    public float m_MonsterDistance;
    public float m_DestroyObsticleDistance;

    public GameObject m_Monster;
    public GameObject m_Player;
    public GameObject m_ObsticlePrefab;
    public GameObject m_CoinPrefab;
    public Transform m_TopObsticleSpawn;
    public Transform m_BottomObsticleSpawn;
    public Transform m_PlayerStart;
    public Transform m_MonsterClose;
    public Transform m_MonsterFinal;

    public float m_ObjectSpawnTimer;
    public float m_CoinMinTimer;
    public float m_CoinMaxTimer;

    public float m_CoinSpawnBuffer;

    private float m_CoinSpawnTimer;

    public float m_CurrentPlayerSpeed;
    private List<GameObject> m_Obsticles;
    private List<GameObject> m_Coins;

    public Hud m_Hud;
    public Slider m_Slider;

    private bool m_ObsticleHit = false;

	// Use this for initialization
	void Start ()
    {
        m_CurrentPlayerSpeed = 0;
        m_ObjectSpawnTimer = m_ObjectSpawnTime;
        m_CoinSpawnTimer = m_CoinMaxTimer;
        m_Obsticles = new List<GameObject>();
        m_Coins = new List<GameObject>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
        //"Move" Player
        CalcSpeed();

        //Move our stuff;
        MoveObsticles();
        MoveCoins();
        MoveMonster();
        MovePlayer();

        //we want our objects to spawn after our player moves a distance rather then just time.
        m_ObjectSpawnTimer -= m_CurrentPlayerSpeed * Time.deltaTime;

        if(m_ObjectSpawnTimer < 0)
        {
            SpawnNewObsticle();
        }

        m_CoinSpawnTimer -= m_CurrentPlayerSpeed * Time.deltaTime;

        if (m_CoinSpawnTimer < 0)
        {
            SpawnCoin();
        }
	}

    void CalcSpeed()
    {
        if(m_ObsticleHit)
        {
            return;
        }

        //TODO: replace later with input manager.
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            m_CurrentPlayerSpeed += m_PlayerSpeedIncrease;
        }   
        else
        {
            //use time so we slow down equally on different devices.
            m_CurrentPlayerSpeed -= m_PlayerSpeedDecrease * Time.deltaTime;

            if (m_CurrentPlayerSpeed < 0)
            {
                m_CurrentPlayerSpeed = 0;
            }
        }
    }

    void SpawnNewObsticle()
    {
        float randNumber = Random.RandomRange(0.0f, 1.0f);

        if (randNumber > 0.5f)
        {
            //Spawn top
            GameObject newObsticle = (GameObject)Instantiate(m_ObsticlePrefab);
            newObsticle.transform.position = m_TopObsticleSpawn.position;
            m_Obsticles.Add(newObsticle);
        }
        else
        {
            //Spawn Bottom
            //Spawn top
            GameObject newObsticle = (GameObject)Instantiate(m_ObsticlePrefab);
            newObsticle.transform.position = m_BottomObsticleSpawn.position;
            m_Obsticles.Add(newObsticle);
        }

        //Reset timer
        m_ObjectSpawnTime -= m_ObjectSpawnDeduction;
        m_ObjectSpawnTimer = m_ObjectSpawnTime;
    }

    void SpawnCoin()
    {
        float randNumber = Random.RandomRange(0.0f, 1.0f);

        if (randNumber > 0.5f)
        {
            //Spawn top
            GameObject newCoins = (GameObject)Instantiate(m_CoinPrefab);
            newCoins.transform.position = m_TopObsticleSpawn.position;
            m_Coins.Add(newCoins);
        }
        else
        {
            //Spawn Bottom
            //Spawn top
            GameObject newCoins = (GameObject)Instantiate(m_CoinPrefab);
            newCoins.transform.position = m_BottomObsticleSpawn.position;
            m_Coins.Add(newCoins);
        }

        //Reset timer
        float CoinSpawnTime = Random.Range(m_CoinMinTimer, m_CoinMaxTimer);

        float TimeDifference = Mathf.Abs(m_ObjectSpawnTimer - CoinSpawnTime);

        if(TimeDifference < m_CoinSpawnBuffer)
        {
            if(CoinSpawnTime < m_ObjectSpawnTimer)
            {
                CoinSpawnTime = m_ObjectSpawnTimer - m_CoinSpawnBuffer;
            }
            else
            {
                CoinSpawnTime = m_ObjectSpawnTimer + m_CoinSpawnBuffer;
            }
        }

        m_CoinSpawnTimer = CoinSpawnTime;
    }

    public float GetPlayerSpeed()
    {
        return m_CurrentPlayerSpeed;
    }

    void MovePlayer()
    { 
        //TODO: add check if game is over.
        if(m_ObsticleHit)
        {
            return;
        }

        //intropolate player position based on moster position.
                
        float MonsterPosX = m_Monster.transform.position.x;
        float MonsterStart = m_MonsterClose.position.x;
        float MonsterFinal = m_MonsterFinal.position.x;

        //Moster to far away to affect player pos.
        if(MonsterPosX < MonsterStart)
        {
            m_Player.transform.position = new Vector3(m_PlayerStart.position.x, m_Player.transform.position.y, m_Player.transform.position.z);
            return;
        }
        else if (MonsterPosX < MonsterFinal) // the player is inbetween the two objects and we need to intropolate his position.
        {
            float PlayerPosX = m_Player.transform.position.x;
            float PlayerStart = m_PlayerStart.position.x;

            float MonsterStartDifference = Mathf.Abs(Mathf.Abs(MonsterStart) - Mathf.Abs(MonsterFinal));
            float MonsterDistance = Mathf.Abs(Mathf.Abs(MonsterStart) - Mathf.Abs(MonsterPosX));

            if (MonsterStartDifference != 0 && MonsterDistance != 0)
            {
                //Get Interpolation amount
                float DistancePercentage = MonsterDistance / MonsterStartDifference;

                //get distance to Move player;
                float PlayerMoveDistance = Mathf.Abs(PlayerStart * DistancePercentage);

                //Move Player               
                m_Player.transform.position = new Vector3(PlayerMoveDistance + PlayerStart, m_Player.transform.position.y, m_Player.transform.position.z);
                return;
            }
        }
        else
        {
            m_Player.transform.position = new Vector3(0, m_Player.transform.position.y, m_Player.transform.position.z);
            return;
        }
    }

    void MoveCoins()
    {
        //Check if player is moving. 
        if (m_CurrentPlayerSpeed != 0)
        {
            //Move all objects towards player.
            for (int i = 0; i < m_Coins.Count; i++)
            {
                if(m_Coins[i] == null)
                {
                    m_Coins.RemoveAt(i);
                    continue; 
                }

                Vector3 ObPos = m_Coins[i].transform.position;
                Vector3 NewPos = new Vector3(ObPos.x - m_CurrentPlayerSpeed * Time.deltaTime, ObPos.y, ObPos.z);
                m_Coins[i].transform.position = NewPos;

                if (ObPos.x < m_DestroyObsticleDistance)
                {
                    Destroy(m_Coins[i].gameObject, 0.05f);
                    m_Coins.RemoveAt(i);
                    continue;
                }                
            }
        }
    }
    
    void MoveObsticles()
    {
        //Check if player is moving. 
        if(m_CurrentPlayerSpeed != 0)
        {
            //Move all objects towards player.
                for (int i = 0; i < m_Obsticles.Count; i++)
                {   
                    Vector3 ObPos = m_Obsticles[i].transform.position;
                    Vector3 NewPos = new Vector3(ObPos.x - m_CurrentPlayerSpeed  * Time.deltaTime, ObPos.y, ObPos.z); 
                    m_Obsticles[i].transform.position = NewPos;
                                        
                     if (ObPos.x < m_DestroyObsticleDistance)
                    {
                        Destroy(m_Obsticles[i].gameObject, 0.05f);
                        m_Obsticles.RemoveAt(i);
                        continue;
                    }
                                                                           
                    if (ObPos.x < m_Player.transform.position.x)
                    {
                        if (m_Obsticles[i].GetComponent<Obsticle>().PassedPlayer)
                        {
                            //Do nothing
                            continue;
                        }
                        else
                        {
                            m_Obsticles[i].GetComponent<Obsticle>().PassedPlayer = true;
                            PassedBox();
                        }
                    }
                }           
         }
    }

    void MoveMonster()
    { 
        //How much we are going to move this frame.
        float CurrentMovement = m_MonsterSpeed - m_CurrentPlayerSpeed;
        
        //get current monster pos
        Vector3 MonPos = m_Monster.transform.position;
        m_Monster.transform.position = new Vector3(MonPos.x + (CurrentMovement * Time.deltaTime), MonPos.y, MonPos.z);

        if (m_Monster.transform.position.x < -m_MaxMonsterDistance)
        {
            m_Monster.transform.position = new Vector3(-m_MaxMonsterDistance, MonPos.y, MonPos.z);
        }

        //Move monster Slider
        float DistanceFromPlayer = Mathf.Abs(m_Monster.transform.position.x);

        if (DistanceFromPlayer != 0)
        {
            float PercentageOfMaxDistance = DistanceFromPlayer / m_MaxMonsterDistance;
            m_Slider.value = 1 - PercentageOfMaxDistance;
        }
        else
        {
            m_Slider.value = 1;
        }
    }

    public void SetObsticleCollided()
    {
        m_MonsterSpeed += m_CurrentPlayerSpeed;
        m_CurrentPlayerSpeed = 0;
        m_ObsticleHit = true;
    }

    void PassedBox()
    {
        //Increment box Counter.
        m_Hud.IncrementBoxCounter();
        
        //Increment monster speed
        m_MonsterSpeed += m_MonsterSpeedIncrement;
    }
}