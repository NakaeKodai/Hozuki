using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSwitch_InputSystem : MonoBehaviour
{
    private bool canPlay;

    public PlayableDirector director;
    public GameObject playTimeline;
    public ControlTimeline controlTimeline;

    [Header("一瞬タイムラインを遅らせたいならTrueにしてください")]
    public bool isWait;

    [Header("一度だけタイムラインを流したいならTrue、何回でも流したいならFalse")]
    public bool onlyOnce;

    void Update()
    {
        if(canPlay && GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
        {
            if(isWait) StartCoroutine(PlayTimeline_Coroutine());
            else PlayTimeline();
        }
        
    }

    IEnumerator PlayTimeline_Coroutine()
    {
        yield return null;
        Debug.Log("タイムライン開始");
        GameManager.instance.director = director;
        controlTimeline.director = director;
        playTimeline.SetActive(true);

        if(onlyOnce) Destroy(gameObject);
    }

    private void PlayTimeline()
    {
        Debug.Log("タイムライン開始");
        GameManager.instance.director = director;
        controlTimeline.director = director;
        playTimeline.SetActive(true);

        if(onlyOnce) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPlay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPlay = false;
        }
    }
}
