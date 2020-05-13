using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGameManager : MonoBehaviour
{
    private PlatformGenerator platformGenerator;
    private Vector3 platformStartPoint;
    private ObjectDestroyer[] objectWithDestroyerList;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PlatformGenerator>() != null)
        {
            platformGenerator = FindObjectOfType<PlatformGenerator>();
            if (FindObjectOfType<PlatformGenerator>() != null)
            {
                platformGenerator = FindObjectOfType<PlatformGenerator>();
                platformStartPoint = platformGenerator.transform.position;
            }
        }
    }

    public void RestartEndlessRun()
    {
        //Method that can run by itself and run independently at the rest of script
        StartCoroutine("RestartEndlessCo");
    }

    public IEnumerator RestartEndlessCo()
    {
        if (platformGenerator == null)
        {
            platformGenerator = FindObjectOfType<PlatformGenerator>();
        }
        yield return new WaitForSeconds(1f);
        objectWithDestroyerList = FindObjectsOfType<ObjectDestroyer>();
        for (int i = 0; i < objectWithDestroyerList.Length; i++)
        {
            objectWithDestroyerList[i].gameObject.SetActive(false);
        }
        platformGenerator.transform.position = platformStartPoint;
        yield return new WaitForSeconds(1f);
    }
}
