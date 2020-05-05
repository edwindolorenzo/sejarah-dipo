using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public GameObject [] finishedObjects;
    public GameObject [] closedObjects;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < finishedObjects.Length; i++)
        {
            finishedObjects[i].SetActive(true);
        }
        for (int i = 0; i < closedObjects.Length; i++)
        {
            closedObjects[i].SetActive(false);
        }
    }

    public void Finish()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}
