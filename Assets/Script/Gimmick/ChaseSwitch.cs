using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseSwitch : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(cahseSwitch)
            {
                Debug.Log("チェイス開始");
                GameManager.instance.isChaseTime = true;
                ChaseManager.instance.ResetTarget();
                ghostNav.speed = speed;
                ghost.SetActive(true);
            }
            else
            {
                Debug.Log("チェイス終了");
                GameManager.instance.isChaseTime = false;
                ChaseManager.instance.MoveLocation(ghostX,ghostY);
                ghost.SetActive(false);
            }

            if(onlyOnce) Destroy(gameObject);
        }
    }
}
