using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    public Animator playerAnimator;
    public RuntimeAnimatorController playerAnim;
    public PlayableDirector director;
    public GameObject gamePlayUI;
    public GameObject dialougueScript;
    public GameObject cameraSetting;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
        playerAnimator.runtimeAnimatorController = null;
        if (gamePlayUI)
        {
            gamePlayUI.SetActive(false);
        }
        if (cameraSetting)
        {
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
            playerAnimator.runtimeAnimatorController = playerAnim;
            dialougueScript.SetActive(true);
            if (cameraSetting)
            {
                cameraSetting.GetComponent<CameraFollow>().enabled = true;
            }
        }
    }
}
