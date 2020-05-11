using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    private Vector2 screenBounds;
    [SerializeField] Transform leftBounds, rightBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 camera = Camera.main.transform.position;
        leftBounds.position = new Vector3(screenBounds.x * -1 + camera.x, leftBounds.position.y, leftBounds.position.z);
        rightBounds.position = new Vector3(screenBounds.x - camera.x, rightBounds.position.y, rightBounds.position.z);
    }
}
