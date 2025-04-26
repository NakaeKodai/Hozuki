using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSwitch : MonoBehaviour
{
    //　オブジェクトに触れることでタイムラインを再生する
    
    public PlayableDirector director;
    public GameObject playTimeline;
    public ControlTimeline controlTimeline;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.director = director;
            controlTimeline.director = director;
            playTimeline.SetActive(true);
        }
    }
}
