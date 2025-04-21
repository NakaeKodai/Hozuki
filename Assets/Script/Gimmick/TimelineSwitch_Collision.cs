using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSwitch_Collision : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playTimeline;
    public ControlTimeline controlTimeline;

    [Header("一度だけタイムラインを流したいならTrue、何回でも流したいならFalse")]
    public bool onlyOnce;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.director = director;
            controlTimeline.director = director;
            playTimeline.SetActive(true);

            if(onlyOnce) Destroy(gameObject);
        }
    }
}
