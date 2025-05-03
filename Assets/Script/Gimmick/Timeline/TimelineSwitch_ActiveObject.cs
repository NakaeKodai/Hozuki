using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSwitch_ActiveObject : MonoBehaviour
{
    private bool canPlay;

    public PlayableDirector director;
    public GameObject playTimeline;
    public ControlTimeline controlTimeline;

    [Header("一瞬タイムラインを遅らせたいならTrueにしてください")]
    public bool isWait;

    [Header("一度だけタイムラインを流したいならTrue、何回でも流したいならFalse")]
    public bool onlyOnce;

    public GameObject targetObject;

    private bool isActive = true;

    void Update()
    {
         //デモ版のため、非表示になった時と、キー操作で判定
        if(canPlay && isActive && GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
        {
            if(isWait) StartCoroutine(PlayTimeline_Coroutine());
            else PlayTimeline();
        }

        if (targetObject != null)
        {
            if (isActive && !targetObject.activeSelf)
            {
                isActive = false;
            }
            else if (!isActive && targetObject.activeSelf)
            {
                isActive = true;
            }
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
