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
        //float screenRatio = (float)Screen.width / (float)Screen.height;
        //float targetRatio = rink.bounds.size.x* Screen.height / Screen.width * 0.5f;


        //if(screenRatio >= targetRatio)
        //Camera.main.orthographicSize = rink.bounds.size.y/2;
        //else
        //{
        //    float differenceInSize = targetRatio / screenRatio;
        //    Camera.main.orthographicSize = rink.bounds.size.y / 2 * differenceInSize;
        //}

        // screen in x size
        float orthoSize = cameraSize.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = orthoSize;
    }

}
