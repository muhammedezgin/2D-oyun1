using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamageToEnemy : MonoBehaviour
{

    public float damage;
    EnemyHealth enemyHealth;


   

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
