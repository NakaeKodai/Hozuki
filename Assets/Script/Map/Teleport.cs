using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleport : MonoBehaviour
{
    //瞬間移動関連　
    [Header("変数をセットするのが面倒なため、コピペして使ってください")]

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject mapInfo; 
    [SerializeField] private TextMeshProUGUI mapText1,mapText2;
    [SerializeField] private Animator mapInfoAnimator;
    private bool isAnimation;

    public string teleportSE;

    [Header("行先の座標と名前を書いて")]
    public float x;
    public float y;
    public string mapName;

    [Header("移動後に向いてほしい方向を1か-1で 特にないなら両方0")]
    public float animX;
    public float animY;

    private RectTransform rectTransformMapInfo;

    [Header("岩リセット用（いじる必要無し）")]
    public bool resetRocks = false;//岩のリセットを行うか否か
    private bool setFirstRocksPosition = true;//岩の位置初期保存するか否か

    [Header("オブジェクト表示用（いじる必要無し）")]
    public bool resetOthers = false;//オブジェクト表示を行うか否か
    public bool sets = true;//trueで後半用、falseで前半用

    void Update()
    {
        if(isAnimation)
        {
            //瞬間移動する時のアニメーション
            AnimatorStateInfo stateInfo = mapInfoAnimator.GetCurrentAnimatorStateInfo(0);

            // アニメーションが終了したかをチェック
            if (stateInfo.normalizedTime >= 1.0f && !mapInfoAnimator.IsInTransition(0))
            {
                // rectTransformMapInfo.anchoredPosition3D = initialPosition;
                mapInfoAnimator.SetTrigger("end");
                mapInfo.SetActive(false);
                isAnimation = false;
            }
        }
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        //プレイヤーに当たったら、プレイヤーを瞬間移動させる
        if (other.gameObject.CompareTag("Player"))
        {
            if(teleportSE != "") SoundManager.instance.PlaySE(teleportSE);
            Debug.Log("オラッ！　移動！");
            other.gameObject.transform.position = new Vector3(x,y,0);
            playerController.MoveAnimator(animX,animY);
            mapText1.text = mapName;
            mapText2.text = mapName;
            mapInfo.SetActive(true);
            mapInfoAnimator.SetTrigger("fade");
            isAnimation = true;

            //岩運びリセット
            if(resetRocks)
            {
                RockReset rockResetObject = GetComponent<RockReset>();
                if(setFirstRocksPosition)
                {
                    rockResetObject.setRocksPosition();
                    setFirstRocksPosition = false;
                    Debug.Log("岩初期位置設定完了");
                }

                rockResetObject.resetRocks();
                Debug.Log("岩リセット完了");
            }

            //オブジェクト表示
            if(resetOthers)
            {
                ResetOther resetOthersObject = GetComponent<ResetOther>();
                resetOthersObject.SetActiveObjects(sets);
                Debug.Log("オブジェクトセット完了");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //敵に当たったら、敵を瞬間移動させる
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(teleportSE != "") SoundManager.instance.PlaySE(teleportSE);
            StartCoroutine(WaitAndTeleport(other));
        }
    }

    private IEnumerator WaitAndTeleport(Collider2D other)
    {    
        //少し待った後に敵を瞬間移動させる
        yield return new WaitForSeconds(0.3f);
        NavMeshAgent2D agent = other.gameObject.GetComponent<NavMeshAgent2D>();
        if (agent != null)
        {
            agent.enabled = false;
        }
        other.gameObject.transform.position = new Vector3(x,y,0);
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
}
