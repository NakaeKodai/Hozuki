using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseSwitch_InputSystem : MonoBehaviour
{
    [SerializeField] private ChaseManager chaseManager;

    private bool inPlayer;

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
    

    [Header("一度だけタイムラインを流したいならTrue、何回でも流したいならFalse")]
    public bool onlyOnce;


    // Update is called once per frame
    void Update()
    {
        
        if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
        {
            if(cahseSwitch)
            {
                Debug.Log("チェイス開始");
                GameManager.instance.isChaseTime = true;
                chaseManager.ResetTarget();
                SoundManager.instance.PlayChaseBGM();
                ghostNav.speed = speed;
                ghost.SetActive(true);
            }
            else
            {
                Debug.Log("チェイス終了");
                GameManager.instance.isChaseTime = false;
                chaseManager.MoveLocation(ghostX,ghostY);
                SoundManager.instance.StopBGM();
                ghost.SetActive(false);
            }

            if(onlyOnce) Destroy(gameObject);
        }
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
