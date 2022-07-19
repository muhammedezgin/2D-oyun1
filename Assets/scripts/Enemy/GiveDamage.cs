using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{

   
    public int damage;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();    

    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        
       // if (other.tag == "Player")
        {
            // ToDo: oyuncaya zarar ver

            player.isHurt = true;
        }
    }


   /* void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            // ToDo: oyuncaya zarar ver

            player.isHurt = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            // ToDo: oyuncaya zarar ver

            player.isHurt = false;
        }
    }*/

}
