using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    public ObjectPooler trapPool;

    public void SpawnTraps(Vector3 startPosition)
    {
        GameObject trap = trapPool.GetPooledObject();
        trap.transform.position = startPosition;
        trap.SetActive(true);
    }
}
