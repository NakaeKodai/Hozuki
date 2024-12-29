using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TakeManager : MonoBehaviour
{
    //アイテム入手の処理

    public GameManager gameManager;
    public UIManager  uiManager;
    public BackpackManager backpackManager;
    [SerializeField] private TextMeshProUGUI talkText;
    [SerializeField] private Image itemImage;

    private ItemData pickUPitem;
    // private List<string> talkTopic = new List<string>();
    // private byte talkPage = 0;
    // private byte beforeTalkPage = 1;
    private bool isInfoText;
    private bool talkEnd;
    private bool calledPlayer;
    private float timer = 0.0f;
    private Animator animator;


    void Awake()
    {
        //テキストが段々濃く表示されるためのアニメーター
        animator = GetComponent<Animator>();
    }


    //アイテム入手時の処理
    public void TakeItem(ItemData pickUPitem)
    {
        //テキスト内容のセット
        this.pickUPitem = pickUPitem;
        gameManager.isOtherMenu = true;
        isInfoText = false;
        talkText.text = pickUPitem.itemName + "を入手しました";
        itemImage.sprite = pickUPitem.itemImage;
        gameManager.isOpenMenu = true;

        //テキストの表示と制御の開始
        gameObject.SetActive(true);
        animator.SetTrigger("NewText");
        talkEnd = false;

        //アイテムの追加
        backpackManager.AddBackpack_Item(pickUPitem);
        // this.talkTopic = talkTopic;
        // this.calledPlayer = calledPlayer;
        // if(!calledPlayer) uiManager.isMenuWindow = false;
    }


    void Update()
    {
        if(!talkEnd) //会話中の処理
        {

            if(timer <= 0.2f) //連打対策用に少しだけ待つ
            {
                timer += Time.deltaTime;
                //Debug.Log("だめだよ");
            }
            else
            {
                //Debug.Log("ぺーちゃくーちゃ");

                //入手した時の画面を閉じる
                if(gameManager.playerInputAction.UI.DecisionMenu.triggered 
                || gameManager.playerInputAction.UI.CancelCloseMenu.triggered)
                {
                    //Debug.Log("次へ");
                    timer = 0.0f;
                    talkEnd = true;
                }

                //アイテムの詳細を表示
                if(gameManager.playerInputAction.UI.InteractDetail.triggered)
                {
                    if(isInfoText) //詳細が表示されている場合、「〇〇を入手しました」と表示
                    {
                        talkText.text = pickUPitem.itemName + "を入手しました";
                        isInfoText = false;
                    }
                    else //アイテムの詳細を表示
                    {
                        talkText.text = pickUPitem.itemInfo;
                        isInfoText = true;
                    }
                    animator.SetTrigger("NewText");
                }
            }
        }
        else //会話終了の処理
        {
            //timer = 0.0f;
            talkEnd = false;
            talkText.text = null;
            gameManager.isOtherMenu = false;
            gameManager.isOpenMenu = false;
            gameObject.SetActive(false);
            // if(!calledPlayer) uiManager.isMenuWindow = true;
        }
    }
}
