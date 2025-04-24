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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("タイムライン開始");
            GameManager.instance.director = director;
            controlTimeline.director = director;
            playTimeline.SetActive(true);

            if(onlyOnce) Destroy(gameObject);
        }
    }
}
