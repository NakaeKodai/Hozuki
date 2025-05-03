using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseSwitch_ActiveObject : MonoBehaviour
{
    [SerializeField] private ChaseManager chaseManager;

    private bool inPlayer;

    [Header("追う時標的をリセットするならTrue")]
    public bool reset;

    [Header("追うまでの猶予時間(秒)　無いなら0でOK")]
    public int waitTime;

    [Header("追われるようにするならTrue、追われるのをやめるならFalse")]
    public bool cahseSwitch;
    public GameObject ghost;
    [SerializeField] private NavMeshAgent2D ghostNav;

    [Header("chaseSwitchがTrueのときに使う変数 Falseなら変更しなくても良い")]
    public float speed = 5.0f;

    //cahseSwitchがtrueの時に使う変数
    [Header("chaseSwitchがFalseのときに使う変数 Trueなら変更しなくても良い")]
    public float ghostX; //敵を飛ばす先の座標
    public float ghostY;

    public GameObject targetObject;

    private bool isActive = true;
    

    [Header("一度だけタイムラインを流したいならTrue、何回でも流したいならFalse")]
    public bool onlyOnce;
    
    void Update()
    {
        if(inPlayer && !isActive) //今回は非表示になった時のみ
        {
            if(cahseSwitch)
            {
                if(waitTime <= 0)
                {
                    GameManager.instance.isChaseTime = true;
                    Debug.Log("チェイス開始");
                    if(reset) chaseManager.ResetTarget();
                    chaseManager.MoveLocation(ghostX,ghostY);
                    SoundManager.instance.PlayChaseBGM();
                    ghostNav.speed = speed;
                    ghost.SetActive(true);
                    if(onlyOnce) Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(WaitChase(waitTime));
                }
            }
            else
            {
                Debug.Log("チェイス終了");
                GameManager.instance.isChaseTime = false;
                chaseManager.MoveLocation(ghostX,ghostY);
                SoundManager.instance.StopBGM();
                ghost.SetActive(false);
                if(onlyOnce) Destroy(gameObject);
            }
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

    private IEnumerator WaitChase(int seconds)
    {
        if(reset) chaseManager.ResetTarget();
        SoundManager.instance.PlayChaseBGM();
        ghostNav.speed = speed;
        ghost.SetActive(true);
        yield return new WaitForSeconds(seconds);

        GameManager.instance.isChaseTime = true;
        if(onlyOnce) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
