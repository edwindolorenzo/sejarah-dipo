using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyObject;
    public Transform[] spawnPoints;
    bool stop = false;
    public float spawnWait = 10f;
    public float maxWait = 2f;
    public int numberUnit = 10;
    public int maxUnit = 20;
    public string tagEnemies;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].gameObject.SetActive(true);
        }
        StartCoroutine(Spawning());               
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            StopAllCoroutines();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tagEnemies);
            foreach(GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyScript>())
                    enemy.GetComponent<EnemyScript>().enabled = false;
                if (enemy.GetComponent<EnemyGunSoldier>())
                    enemy.GetComponent<EnemyGunSoldier>().enabled = false;
            }
        }
    }

    bool checkNumber()
    {
        if(GameObject.FindGameObjectsWithTag(tagEnemies).Length < numberUnit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Spawning()
    {
        yield return new WaitForSeconds(spawnWait);
        while(!stop)
        {
            if (checkNumber())
            {
                int randspawn = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyObject,spawnPoints[randspawn].position, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
        }
    }

    public void AddSpawn()
    {
        if(spawnWait > maxWait)
            spawnWait -= 0.5f;
        if(numberUnit < maxUnit)
            numberUnit += 1;
    }

    public void StopGame()
    {
        stop = true;
    }
}
