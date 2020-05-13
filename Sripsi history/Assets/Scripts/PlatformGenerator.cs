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

    //public GameObject[] thePlatforms;
    private int platformSelector;
    private float[] platformWidths;

    public ObjectPooler[] theObjectPools;

    private float minHeight;
    public Transform maxHeightPoint;
    private float maxHeight;
    public float maxHeightChange;
    private float heightChange;

    //private TrapGenerator theTrapGenerator;

    public float randomObjectThreshold;

    private ObjectGenerator theHealthGenerator;
    public float randomHealthThreshold;
    private bool healthExist;

    public float randomTrapThreshold;
    public ObjectPooler trapPool;

    // Start is called before the first frame update
    void Start()
    {
        //platformWidth = thePlatformThatWillGenerate.GetComponent<BoxCollider2D>().size.x;

        platformWidths = new float[theObjectPools.Length];

        for(int i=0; i < theObjectPools.Length;i++)
        {
            platformWidths[i] = theObjectPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;

        theHealthGenerator = GameObject.Find("HeartGenerator").GetComponent<ObjectGenerator>();

        healthExist = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x)
        {
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);
            //distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax) + 1f;

            platformSelector = Random.Range(0, theObjectPools.Length);

            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if(heightChange > maxHeight)
            {
                heightChange = maxHeight;
            } else if (heightChange < minHeight)
            {
                heightChange = minHeight;
            }

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector]/2) + distanceBetween, heightChange, transform.position.z);

            //Instantiate(theObjectPools[platformSelector], transform.position, transform.rotation);
             
            GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();

            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            if (Random.Range(0, 100) < randomObjectThreshold)
            {
                if ((Random.Range(0, 100) < randomHealthThreshold) && healthExist)
                {
                    theHealthGenerator.SpawnObjects(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
                }else
                {
                    healthExist = false;
                }

                if ((Random.Range(0, 100) < randomTrapThreshold) && !healthExist)
                {
                    GameObject newTrap = trapPool.GetPooledObject();

                    float trapXPosition = Random.Range(-platformWidths[platformSelector] / 2 + 1f, platformWidths[platformSelector] / 2 - 1f);

                    Vector3 trapPosition = new Vector3(trapXPosition, 0.75f, 0f);

                    newTrap.transform.position = transform.position + trapPosition;
                    newTrap.transform.rotation = transform.rotation;
                    newTrap.SetActive(true);
                }else
                {
                    healthExist = true;
                }
            }
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
        }
    }
}
