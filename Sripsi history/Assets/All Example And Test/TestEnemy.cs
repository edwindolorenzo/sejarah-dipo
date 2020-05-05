using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    Enemy enemy = new Enemy();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        enemy.Health -= damage;
        if(enemy.Health <= 0)
        {
            Object.Destroy(this.gameObject);
            Die();
        }
    }

    void Die()
    {
    }
}
