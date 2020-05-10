using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public SpriteRenderer cameraSize;
    // Start is called before the first frame update
    void Start()
    {
        // screen in y size
        float orthoSize = cameraSize.bounds.size.y / 2;
        Camera.main.orthographicSize = orthoSize;
        // screen in x size
        //float orthoSize = cameraSize.bounds.size.x * Screen.height / Screen.width * 0.5f;

        //Camera.main.orthographicSize = orthoSize;
    }

}
