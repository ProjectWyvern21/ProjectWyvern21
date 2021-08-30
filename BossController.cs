using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState
{
    Idle,

    Summon,

    Healing,

    RangeAttack,
}

public enum BossStage
{
    stageOne,

    stageTwo,

    dead,
}

public class BossController : MonoBehaviour
{
    public int bossMaxHealth = 1000;
    public int bossCurrentHealth;
    [Tooltip("set this equal to how much damage the boss can do to the player if the player runs into the boss")]
    public int bossDamage = 20;
    [Tooltip("set this equal to how much damage the player can do to the boss")]
    public int playerDamageToBoss = 50;
    [Tooltip("how much the boss will heal")]
    public int bossHeal = 20;
    public GameObject bossHusks;
    public GameObject waveAttack;
    public Transform WaveAttackRangeEndPoint;
    public float waveAttackSpeed = 8f;
    public HealthBar bossHealthBar;
    private float waitTime = 0f;
    private float animDelay = 2f;
    public float animDelayStart = 0f; 
    [SerializeField]
    [Tooltip("Changes delay time between states")]
    private float delayTime = 3f;
    public Transform spawnPoint;
    public GameObject boss;
    public GameObject bossMusic;
    public GameObject levelMusic; 
    public BossState currentState = BossState.Idle;
    public BossStage currentStage = BossStage.stageOne;
    
    public ArrayList stageOneStateProb = new ArrayList();
    private int stageOneSummonProb = 25;
    private int stageOneRangeProb = 65;
    private int stageOneIdleProb = 10;

    public ArrayList stageTwoStateProb = new ArrayList();
    private int stageTwoSummonProb = 25;
    private int stageTwoHealProb = 20;
    private int stageTwoIdleProb = 10;
    private int stageTwoRangeProb = 45;

    private int randomState;
    private bool spawnedAHusk = false;
    private bool spawnAWave = false;
    private bool healed = false;
    public Animator bossAnim; 
    // Start is called before the first frame update
    void Start()
    {
        bossCurrentHealth = bossMaxHealth;
        bossHealthBar.SetMaxHealth(bossMaxHealth);
        levelMusic.SetActive(false);
        bossMusic.SetActive(true); 
        // stage one setting the weighted states up
        for (int i =0; i < stageOneIdleProb; i++)
        {
            stageOneStateProb.Add(BossState.Idle);
        }
        for (int i = 0; i < stageOneRangeProb; i++)
        {
            stageOneStateProb.Add(BossState.RangeAttack);
        }
        for (int i = 0; i < stageOneSummonProb; i++)
        {
            stageOneStateProb.Add(BossState.Summon);
        }
        // stage two setting the weighted states up
        for (int i = 0; i < stageTwoIdleProb; i++)
        {
            stageTwoStateProb.Add(BossState.Idle);
        }
        for (int i = 0; i < stageTwoSummonProb; i++)
        {
            stageTwoStateProb.Add(BossState.Summon);
        }
        for (int i = 0; i < stageTwoHealProb; i++)
        {
            stageTwoStateProb.Add(BossState.Healing);
        }
        for (int i = 0; i < stageTwoRangeProb; i++)
        {
            stageTwoStateProb.Add(BossState.RangeAttack);
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (BossState.Idle):
                Idle();
                break;

            case (BossState.Summon):
                SummonAttack();
                break;

            case (BossState.Healing):
                Healing();
                break;

            case (BossState.RangeAttack):
                RanageAttack();
                break;
        }
        switch (currentStage)
        {
            case (BossStage.stageOne):
                StageOne();
                break;

            case (BossStage.stageTwo):
                StageTwo();
                break;
            case (BossStage.dead):
                Dead();
                break;
        }
        if (bossCurrentHealth <= 0)
        {
            currentStage = BossStage.dead;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // damages the player
            Debug.Log("hit a player");
            FindObjectOfType<GameManager>().PlayerHealthLoss(bossDamage);
        }
        else if (other.tag == "Bullet" && currentState != BossState.Idle)
        {
            Debug.Log("Hit");
            //damage the boss 
            BossHealthLoss();
        }
    }
    public void BossHealthLoss()
    {
        bossCurrentHealth -= playerDamageToBoss;
        bossHealthBar.SetHealth(bossCurrentHealth);
    }

    void StageOne()
    {
        if(bossCurrentHealth > (bossMaxHealth * 0.5))
        {
            currentStage = BossStage.stageOne;
        }
        else
        {
            currentStage = BossStage.stageTwo;
        }
    }

    void StageTwo()
    {
        if(bossCurrentHealth <= (bossMaxHealth * 0.5))
        {
            currentStage = BossStage.stageTwo;
        }
        else if (bossCurrentHealth <= 0)
        {
            currentStage = BossStage.dead;
        }
    }
    void Dead()
    {
        Debug.Log("You beat the boss congrats");
        //Destroy(gameObject);
        boss.SetActive(false);
        bossMusic.SetActive(false);
        levelMusic.SetActive(true);
    }
    void Idle()
    {
        Debug.Log("idle");
        bossAnim.SetBool("IsIdle", true);
        bossAnim.SetBool("IsGroundAttack", false);
        bossAnim.SetBool("IsHeal", false);
        bossAnim.SetBool("IsSummon", false);
        if (currentStage == BossStage.stageOne && waitTime > delayTime)
        {
            randomState = Random.Range(0, stageOneStateProb.Count);
            if (stageOneStateProb[randomState].ToString() == "Idle")
            {
                currentState = BossState.Idle;
            }
            else if (stageOneStateProb[randomState].ToString() == "RangeAttack")
            {
                currentState = BossState.RangeAttack;
            }
            else if (stageOneStateProb[randomState].ToString() == "Summon")
            {
                currentState = BossState.Summon;
            }
            waitTime = 0f;
        }
         else if (currentStage == BossStage.stageTwo && waitTime > delayTime)
        {
            randomState = Random.Range(0, stageTwoStateProb.Count);
            if (stageTwoStateProb[randomState].ToString() == "Idle")
            {
                currentState = BossState.Idle;
            }
            else if (stageTwoStateProb[randomState].ToString() == "Healing")
            {
                currentState = BossState.Healing;
            }
            else if (stageTwoStateProb[randomState].ToString() == "Summon")
            {
                currentState = BossState.Summon;
            }
            else if (stageTwoStateProb[randomState].ToString() == "RangeAttack")
            {
                currentState = BossState.RangeAttack;
            }
            waitTime = 0f;
        } 
        else
        {
            waitTime += Time.deltaTime;
        }
    }
    void Healing()
    {
        Debug.Log("Healing");
        bossAnim.SetBool("IsHeal", true);
        bossAnim.SetBool("IsIdle", false);
        if ( bossCurrentHealth < (bossMaxHealth * 0.5) && healed == false)
        {
            bossCurrentHealth += bossHeal;
            bossHealthBar.SetHealth(bossCurrentHealth);
            healed = true;
        }
        if(waitTime < delayTime)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            currentState = BossState.Idle;
            waitTime = 0f;
            healed = false;
            bossAnim.SetBool("IsHeal", false);
        }
        
    }

    void SummonAttack()
    {
        Debug.Log("Summoning husk");
        bossAnim.SetBool("IsSummon", true);
        bossAnim.SetBool("IsIdle", false);
        if (spawnedAHusk == false)
        {
            /*if (animDelayStart < animDelay)
            {
                animDelayStart += Time.deltaTime;
            }*/

                Instantiate(bossHusks, spawnPoint.position, spawnPoint.rotation);
                spawnedAHusk = true;

        }
        
        if (waitTime < delayTime)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            currentState = BossState.Idle;
            waitTime = 0f;
            spawnedAHusk = false;
            bossAnim.SetBool("IsSummon", false);
            //animDelayStart = 0;
        }
    }

    void RanageAttack()
    {
        Debug.Log("Doing a range attack");
        bossAnim.SetBool("IsGroundAttack", true);
        bossAnim.SetBool("IsIdle", false);
        if (spawnAWave == false)
        {
            if (animDelayStart < animDelay)
            {
                animDelayStart += Time.deltaTime;
            }
            else
            {
                Vector2 direction = WaveAttackRangeEndPoint.transform.position - transform.position;
                direction.Normalize();
                GameObject bulletClone;
                bulletClone = Instantiate(waveAttack, spawnPoint.position, spawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * waveAttackSpeed;
                spawnAWave = true;
            }
        }
        if (waitTime < delayTime)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            currentState = BossState.Idle;
            waitTime = 0f;
            spawnAWave = false;
            bossAnim.SetBool("IsGroundAttack", false);
            animDelayStart = 0;
        }
    }
        
}
