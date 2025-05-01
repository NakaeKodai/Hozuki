using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTERepeatedhits : MonoBehaviour
{
    public float rockHP = 20f;//最大HP
    private float rockNowHP;//現在のHP
    public int rockAttack = 2;//岩へのダメージ

    public GameObject QTEObject;
    public Image QTEGauge;
    private Color maxColor = new Color(1f,1f,1f,1f);
    private Color harfColor = new Color(0.7f,0.7f,0f,1f);
    private Color quarterColor = new Color(0.6f,0f,0f,1f);

    public PlayerController playerScript;//プレイヤーのスクリプト
    public GameManager gameManager;
    
    private bool canQTE = false;//QTEにプレイヤーが触れているか
    private bool nowQTE = false;//QTEが進行中
    private bool nowCoolDown = false;
    private float coolDownTime = 0.1f;
    private float nowCoolDownTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rockNowHP = rockHP;
    }

    // Update is called once per frame
    void Update()
    {   
        if(nowQTE)
        {
            //クールタイムならボタンを押してダメージを与える
            if(!nowCoolDown)
            {
                if(gameManager.playerInputAction.Player.ActionANDDecision.triggered)
                {
                    rockNowHP -= rockAttack;
                    nowCoolDown = true;
                    Debug.Log("岩の残りHP："+rockNowHP);
                    float HPRatio = rockNowHP/rockHP;
                    QTEGauge.fillAmount = HPRatio;
                    // Debug.Log(rockNowHP/rockHP);
                }
            }
            //クールタイム中はカウントを行う
            else
            {
                nowCoolDownTime += Time.deltaTime;
                if(nowCoolDownTime >= coolDownTime)
                {
                    nowCoolDown = false;
                    nowCoolDownTime = 0f;
                }
            }

            //岩のHPで色変更、HPがなくなったら終了
            if(rockNowHP <= 0)
            {
                nowQTE = false;
                playerScript.canMove = false;
                gameManager.isOtherMenu = false;
                Debug.Log("岩消える");
                this.gameObject.SetActive(false);
                QTEObject.SetActive(false);
            }
            else if(rockNowHP <= rockHP/3)
            {
                QTEGauge.color = quarterColor;
                Debug.Log("赤色");
            }
            // else if(rockNowHP <= rockHP/2)
            // {
            //     QTEGauge.color = harfColor;
            //     Debug.Log("黄色");
            // }
            
        }
        //QTEが開始していなければ開始する
        else if(canQTE)
        {
            if(gameManager.playerInputAction.Player.ActionANDDecision.triggered)
            {
                nowQTE = true;
                //他の動作を不可能にする
                playerScript.canMove = true;
                gameManager.isOtherMenu = true;
                Debug.Log("QTE開始");
                QTEObject.SetActive(true);
                QTEGauge.color = maxColor;
                QTEGauge.fillAmount = 1f;
                Debug.Log(rockHP/4);
            }
        }
    }

    //当たり判定に当たったか(Collision)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("壊せるででで");
            canQTE = true;
        }
    }

        //当たり判定から出たか(Collision)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("壊せぬぬ");
            canQTE = false;
        }
    }
}
