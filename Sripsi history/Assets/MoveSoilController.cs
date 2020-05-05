using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSoilController : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    bool endedPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(startPoint, endPoint, 5 * Time.deltaTime);
    }
}
