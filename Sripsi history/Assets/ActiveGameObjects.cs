using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGameObjects : MonoBehaviour
{
    public GameObject[] gameobjects;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < gameobjects.Length; i++)
            {
                gameobjects[i].SetActive(true);
            }
        gameObject.active = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
