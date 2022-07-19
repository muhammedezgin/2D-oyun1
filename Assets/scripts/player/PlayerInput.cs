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

       // && den sonra eðer grounded doðruysa yani karakter yerde ise zýplama gerçekleþsin diyoruz çünkü fazladan zýplama yapar
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
