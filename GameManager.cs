using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerMaxHealth = 100;
    public int playerCurretHealth;
    public int playerLifeCount = 3;
    public float playerMaxMana = 100;
    public float playerCurrentMana;
    public float manaRegenPerSecond = 1;
    [SerializeField]
    public float crystalsCollected = 0;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public Text crystalText;
    public Text playerHealthText;
    public Text playerManaText;
    public Text playerLifeCountText;
    public Transform levelStartSpawn;
    public GameObject player;

    public GameObject crystalChunkZero; 
    public GameObject crystalChunkOne;
    public GameObject crystalChunkTwo;
    public GameObject crystalChunkThree;
    public GameObject crystalChunkFour;
    public GameObject crystalChunkFive;
    public GameObject crystalChunkSix;
    public GameObject crystalChunkSeven;

    public GameObject screenFlash;



    void Start()
    {
        playerCurretHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
        playerHealthText.text = "HP: " + playerCurretHealth;
        playerCurrentMana = playerMaxMana;
        manaBar.SetMaxMana(playerMaxMana);
        playerManaText.text = "MP: " + playerCurrentMana;
        crystalText.text = "x " + crystalsCollected;
        playerLifeCountText.text = "" + playerLifeCount;
        crystalChunkOne.SetActive(false);
        crystalChunkTwo.SetActive(false);
        crystalChunkThree.SetActive(false);
        crystalChunkFour.SetActive(false);
        crystalChunkFive.SetActive(false);
        crystalChunkSix.SetActive(false);
        crystalChunkSeven.SetActive(false);
        screenFlash.SetActive(false);  

    }
    void Update()
    {
        
        if(playerCurretHealth <= 0)
        {
            playerLifeCount = playerLifeCount - 1;
            playerCurretHealth = playerMaxHealth;
            healthBar.SetHealth(playerCurretHealth);
            playerCurrentMana = playerMaxMana;
            manaBar.SetCurrentMana(playerCurrentMana);
            playerManaText.text = "MP: " + playerCurrentMana.ToString("f0");
            player.transform.position = levelStartSpawn.position; 
            playerHealthText.text = "HP: " + playerCurretHealth;
            playerLifeCountText.text = "" + playerLifeCount;
        }
        if(playerCurretHealth > playerMaxHealth)
        {
            playerCurretHealth = playerMaxHealth;
            healthBar.SetHealth(playerCurretHealth);
            playerHealthText.text = "HP: " + playerCurretHealth;
        }

        if(playerLifeCount <= 0)
        {
            SceneManager.LoadScene(2);
            Cursor.visible = true;
        }

        /*if(playerCurrentMana < playerMaxMana)
        {
            playerCurrentMana += manaRegenPerSecond * Time.deltaTime;
            manaBar.SetCurrentMana(playerCurrentMana);
            playerManaText.text = "MP: " + playerCurrentMana.ToString("f0");
        }*/

        if(playerCurrentMana > playerMaxMana)
        {
            playerCurrentMana = playerMaxMana;
            manaBar.SetCurrentMana(playerCurrentMana);
            playerManaText.text = "MP: " + playerCurrentMana.ToString("f0");
        }
    }
    //function to add health to player
    public void PlayerHealthGain(int HealthAdd)
    {
        if(playerCurretHealth < playerMaxHealth)
        {
            playerCurretHealth += HealthAdd;
            healthBar.SetHealth(playerCurretHealth);
            playerHealthText.text = "HP: " + playerCurretHealth;
        }
    }

    public void PlayerManaGain(float ManaAdd)
    {
        if(playerCurrentMana < playerMaxMana)
        {
            playerCurrentMana += ManaAdd;
            manaBar.SetCurrentMana(playerCurrentMana);
            playerManaText.text = "MP: " + playerCurrentMana.ToString("f0");
        }
    }

    /*public void PagesCollected(float pageAdd)
    {
        pagesCollected += pageAdd;
        pageText.text = "" + pagesCollected + "/" + totalNumberofPages;
    } */

    public void CrystalsCollected(float crystalAdd)
    {
        crystalsCollected += crystalAdd;
        crystalText.text = "x " + crystalsCollected;
        if (crystalsCollected == 1)
        {
            //crystalChunkZero.SetActive(false);
            crystalChunkOne.SetActive(true);
        }
        else if (crystalsCollected == 2)
        {
            crystalChunkOne.SetActive(false);
            crystalChunkTwo.SetActive(true);
        }
        else if (crystalsCollected == 3)
        {
            crystalChunkTwo.SetActive(false);
            crystalChunkThree.SetActive(true);
        }
        else if (crystalsCollected == 4)
        {
            crystalChunkThree.SetActive(false);
            crystalChunkFour.SetActive(true);
        }
        else if (crystalsCollected == 5)
        {
            crystalChunkFour.SetActive(false);
            crystalChunkFive.SetActive(true);
        }
        else if (crystalsCollected == 6)
        {
            crystalChunkFive.SetActive(false);
            crystalChunkSix.SetActive(true);
        }
        else if (crystalsCollected >= 7)
        {
            crystalChunkSix.SetActive(false);
            crystalChunkSeven.SetActive(true);
        }
    }
    public void PlayerHealthLoss(int HealthLoss)
    {
        playerCurretHealth -= HealthLoss;
        healthBar.SetHealth(playerCurretHealth);
        playerHealthText.text = "HP: " + playerCurretHealth;
        StartCoroutine(damageFlash(screenFlash, 0.1f)); 

    }

    public void PlayerManaLoss(float ManaLoss)
    {
        playerCurrentMana -= ManaLoss;
        manaBar.SetCurrentMana(playerCurrentMana);
        playerManaText.text = "MP: " + playerCurrentMana.ToString("f0");
    }

    IEnumerator damageFlash (GameObject screenFlash, float delayFlash)
    {
        screenFlash.SetActive(true);
        yield return new WaitForSeconds(delayFlash);
        screenFlash.SetActive(false);
         
    }
}
