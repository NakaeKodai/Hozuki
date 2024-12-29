using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkManager : MonoBehaviour
{
    //会話の処理

    public GameManager gameManager;
    public UIManager  uiManager;
    [SerializeField] private GameObject explain;
    [SerializeField] private TextMeshProUGUI talkText;
    private List<string> talkTopic = new List<string>();
    private byte talkPage = 0;
    private bool talkEnd;
    private bool calledPlayer;
    private float timer = 0.0f;
    private Animator animator;
    

    void Awake()
    {
        //テキストが段々濃く表示されるためのアニメーター
        animator = GetComponent<Animator>();
    }

    //会話の処理
    public void Talk(List<string> talkTopic,bool calledPlayer)
    {
        //会話テキストのセット
        gameManager.isOtherMenu = true;
        talkPage = 0;
        this.talkTopic = talkTopic;

        //テキストの表示と制御の開始
        gameObject.SetActive(true);
        talkText.text = talkTopic[talkPage];
        animator.SetTrigger("NewText");
        talkEnd = false;

        //メニューからの呼び出しかどうか
        //メニューからなら終了時にメニューを表示させる
        this.calledPlayer = calledPlayer;
        if(!calledPlayer) uiManager.isMenuWindow = false;
        else gameManager.isOpenMenu = true;
    }

    void Update()
    {
        if(!talkEnd)
        {
            if(timer <= 0.2f) //連打対策
            {
                timer += Time.deltaTime;
                //Debug.Log("だめだよ");
            }
            else
            {
                //Debug.Log("ぺーちゃくーちゃ");
                if(gameManager.playerInputAction.UI.DecisionMenu.triggered)
                {
                    //Debug.Log("次へ");
                    talkPage++;
                    timer = 0.0f;
                    if(talkPage >= talkTopic.Count) talkEnd = true;
                    else 
                    {
                        talkText.text = talkTopic[talkPage];
                        animator.SetTrigger("NewText");
                    }
                }
            }
        }
        else
        {
            timer = 0.0f;
            talkEnd = false;
            talkText.text = null;
            gameManager.isOtherMenu = false;
            gameObject.SetActive(false);
            if(!calledPlayer)
            {
                uiManager.isMenuWindow = true;
                // explain.SetActive(true);
                uiManager.OpenMenuWindow();
            }
            else gameManager.isOpenMenu = false;
        }
    }
}
