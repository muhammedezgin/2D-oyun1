using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player player;

    // Uý
     public Slider healthBar;



    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        healthBar.maxValue = player.maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            // bu fonk çaðýrmadan önce 1 sn bekle 10 yazarsak 10 sn bekler
            Invoke("RestartGame", 2);
           
        } 
        UpdateUI();
    }
    public void RestartGame()
    {
        // içinde bulunduðum leveli al daha sonra bu leveli yðkle
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
      
    }
    void UpdateUI()
    {
         healthBar.value = player.currentPlayerHealth;
         if (player.currentPlayerHealth <= 0)
         healthBar.minValue = 0;

    }
}
