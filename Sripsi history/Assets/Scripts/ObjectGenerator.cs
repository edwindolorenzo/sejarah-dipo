using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public ObjectPooler objectPool;

    public void SpawnObjects(Vector3 startPosition)
    {
        GameObject obj = objectPool.GetPooledObject();
        obj.transform.position = startPosition;
        obj.SetActive(true);
    }
}
