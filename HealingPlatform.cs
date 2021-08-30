using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlatform : MonoBehaviour
{
    [Tooltip("Set this to be how much health is heal per tick")]
    public int heal = 100; 
    //[Tooltip("Set this to be how many ticks happen ")]
    //public float maxHealTime= 3;
    //[Tooltip("change this value to how long you want the cool down before the player can regain health again")]
    //public float cooldownTime = 30f;
    [Tooltip("Set this to be how much mana is added per tick")]
    public float manaAdded = 100;

    //private float cooldownRemaining; 
    //public bool isOnCoolDown = false;
    public GameObject gamemanagaer;
    private int playermaxhealth;
    private int currentHealth;
    private float playercurrentMana;
    private float playerMaxMana; 
    public GameObject healingArea;
    public bool isActivated = false;
    public GameObject fountainLight; 

    public void Start()
    {
        playermaxhealth = gamemanagaer.GetComponent<GameManager>().playerMaxHealth; // gets the maximum health that the player has and assigns it to a variable in this script
        currentHealth = gamemanagaer.GetComponent<GameManager>().playerCurretHealth; // gets the current health that the player has 
        playercurrentMana = gamemanagaer.GetComponent<GameManager>().playerCurrentMana;
        playerMaxMana = gamemanagaer.GetComponent<GameManager>().playerMaxMana;
        fountainLight.SetActive(false); 
    }

    public void Update()
    {
        currentHealth = gamemanagaer.GetComponent<GameManager>().playerCurretHealth; // constantly checks and changes the players current health

        /*if(isOnCoolDown == true)
        {
            //Debug.Log("waiting for cool down");
            // start the cool down period 
            if(cooldownRemaining < cooldownTime)
            {
                cooldownRemaining += Time.deltaTime;
            }
            else if (cooldownRemaining >= cooldownTime)
            {
                //one cool down is done set cool down bool to false
                isOnCoolDown = false;
                cooldownRemaining = 0;
            }
        }*/

    }

    // if the player enters the healing area and the player is not max health it will start the healing coroutine
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            isActivated = true;
            fountainLight.SetActive(true);
        }

        if(col.tag == "Player"  && isActivated == true)  
        {
            //StartCoroutine("Heal");
            FindObjectOfType<GameManager>().PlayerHealthGain(heal);
            FindObjectOfType<GameManager>().PlayerManaGain(manaAdded);
        }
    }

    //if the player exits the healing area it stops the healing corroutine
    /*private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            StopCoroutine("Heal");
        }
    }*/ 

    // healing coroutine its restores the players health by what the heal is and it does it the for what ever number is  maxHealTime
    /*IEnumerator Heal()
    {
        for(float i = 0; i < maxHealTime; i++)
        {
            FindObjectOfType<GameManager>().PlayerHealthGain(heal);
            FindObjectOfType<GameManager>().PlayerManaGain(manaAdded);
            yield return new WaitForSeconds(1f);
            if(i == maxHealTime-1)
            {
                isOnCoolDown = true;
            } 
        } 
    }*/
}
