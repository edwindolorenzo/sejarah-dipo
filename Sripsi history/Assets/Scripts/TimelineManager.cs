using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{

    // disable and enable animation
    public Animator playerAnimator;
    public RuntimeAnimatorController playerAnim;

    //check directior
    public PlayableDirector director;
    private bool fix = false;

    // check before and after cutscene
    public GameObject gamePlayUI;
    public GameObject dialougueScript;
    public GameObject cameraSetting;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(playerAnimator != null)
        {
            playerAnim = playerAnimator.runtimeAnimatorController;
            playerAnimator.runtimeAnimatorController = null;
        }
        if (gamePlayUI)
        {
            gamePlayUI.SetActive(false);
        }
        if (cameraSetting)
        {
            if (cameraSetting.GetComponent<CameraFollowHorse>() != null)
                cameraSetting.GetComponent<CameraFollowHorse>().enabled = false;
            if(cameraSetting.GetComponent<CameraFollow>() != null)
                cameraSetting.GetComponent<CameraFollow>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            if (gamePlayUI)
            {
                gamePlayUI.SetActive(true);
            }
            fix = true;
            if(playerAnimator != null)
            playerAnimator.runtimeAnimatorController = playerAnim;
            dialougueScript.SetActive(true);
            if (cameraSetting)
            {
                if (cameraSetting.GetComponent<CameraFollowHorse>() != null)
                    cameraSetting.GetComponent<CameraFollowHorse>().enabled = true;
                if (cameraSetting.GetComponent<CameraFollow>() != null)
                    cameraSetting.GetComponent<CameraFollow>().enabled = true;
            }
        }
    }
}
