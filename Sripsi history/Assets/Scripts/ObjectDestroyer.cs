using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject objectDestructionPoint;

    // Start is called before the first frame update
    void Start()
    {
        objectDestructionPoint = GameObject.Find("ObjectDestructionPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < objectDestructionPoint.transform.position.x)
        {
            gameObject.SetActive(false);
        }

    }
}
