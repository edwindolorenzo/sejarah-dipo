using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    public GameObject thePlatformThatWillGenerate;
    public Transform generationPoint;

    private float distanceBetween;

    private float platformWidth;
    public float distanceBetweenMin;
    public float distanceBetweenMax;

    // Start is called before the first frame update
    void Start()
    {
        platformWidth = thePlatformThatWillGenerate.GetComponent<BoxCollider2D>().size.x;    
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x)
        {
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

            transform.position = new Vector3(transform.position.x + platformWidth + distanceBetween, transform.position.y, transform.position.z);

            Instantiate(thePlatformThatWillGenerate, transform.position, transform.rotation);
        }
    }
}
