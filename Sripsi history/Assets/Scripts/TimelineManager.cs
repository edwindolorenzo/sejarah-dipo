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

    AudioManager audioManager = AudioManager.instance;
    [SerializeField] bool sceneMusic = true;

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
            cameraSetting.GetComponent<CameraFollow>().enabled = false;
        }
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
        if (sceneMusic)
        {
            audioManager.Play("CutSceneMusic", true);
            audioManager.Stop("GameMusic", true);
        }
        else
            audioManager.Play("GameMusic", true);
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
            if (sceneMusic)
            {
                audioManager.Stop("CutSceneMusic", true);
                audioManager.Play("GameMusic", true);
            }
            if (cameraSetting)
            {
                cameraSetting.GetComponent<CameraFollow>().enabled = true;
            }
        }
    }

    public void ChangeTimeLine(PlayableDirector newDirector, GameObject newDialogueScript, bool stopGameMusic = false)
    {
        director = newDirector;
        if (stopGameMusic)
        {
            audioManager.Stop("GameMusic", true);
            audioManager.Play("CutSceneMusic", true);
        }
        dialougueScript = newDialogueScript;
        director.Play();
        fix = false;
    }
}
