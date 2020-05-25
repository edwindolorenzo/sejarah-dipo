using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveTimeline : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject dialougueScript;
    public bool sceneMusic = true;

    public GameObject timelineDirector;
    TimelineManager timelineManager;

    public GameObject gamePlayUI;

    private void Start()
    {
        timelineManager = timelineDirector.GetComponent<TimelineManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(PlayingTimeline(sceneMusic));
        }
    }

    IEnumerator PlayingTimeline(bool music)
    {
        gamePlayUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        timelineManager.ChangeTimeLine(director, dialougueScript, music);
        gameObject.SetActive(false);
        yield return null;
    }
}
