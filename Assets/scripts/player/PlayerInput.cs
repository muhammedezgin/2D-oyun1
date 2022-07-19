using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
     Player player;

    void Start()
    {

    player = GetComponent<Player>();

    }


    void Update()
    {

       // && den sonra e�er grounded do�ruysa yani karakter yerde ise z�plama ger�ekle�sin diyoruz ��nk� fazladan z�plama yapar
     if (Input.GetButtonDown("Jump")  && player.isGrounded)
        {
              player.Jump();
              player.canDoubleJump = true;
        }
        else if(Input.GetButtonDown("Jump") && !player.isGrounded && player.canDoubleJump)
        {
            player.DoubleJump();
            player.canDoubleJump = false;
        }

    }
}
