using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    //UIの処理　主にメニュー画面

    public GameManager gameManager;

    public bool isMenuWindow; //メニュー画面を操作できるか
    public bool isSettingWindow; //設定画面を操作できるか

    private int nowCursorNum = 0;
    private int beforeCursorNum = 1; //nowCursorNumとは違う数値

    private GameObject nowCursor;
    private GameObject nowCursorImage;

    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject explain;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private GameObject backpack;
    [SerializeField] private GameObject mapInfo1,mapInfo2; 
    [SerializeField] private GameObject settingWindow;
    [SerializeField] private GameObject textWindow;
    [SerializeField] private TalkTopic  playerTalkTopic;
    [SerializeField] private TalkManager talkManager;
    
    void Start()
    {
    }


    void Update()
    {
        if (!gameManager.isOpenMenu)
        {
            if (gameManager.playerInputAction.UI.OpenMenu.triggered)
            {
                gameManager.isOpenMenu = true;
                OpenMenuWindow();
                mapInfo1.SetActive(false);
            }
        }
        else if(!gameManager.isOtherMenu)
        {
            if(isMenuWindow)
            {
                MenuWindowOperation();
                // MenuExplainText();
            }
        }
    }

    //メニューを開く処理
    public void OpenMenuWindow()
    {
        menuWindow.SetActive(true);
        explain.SetActive(true);
        mapInfo2.SetActive(true);
        isMenuWindow = true;
    }

    //メニューを閉じる処理
    public void CloseMenuWindow()
    {
        menuWindow.SetActive(false);
        explain.SetActive(false);
        mapInfo2.SetActive(false);
        isMenuWindow = false;
    }


    //メニューの操作の処理
    public void MenuWindowOperation()
    {
        if(beforeCursorNum != nowCursorNum)
        {
            nowCursor = menuWindow.transform.GetChild(nowCursorNum).gameObject;
            nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
            beforeCursorNum = nowCursorNum;
            nowCursorImage.SetActive(true);
            MenuExplainText();
        }
        
        //メニューウィンドウを閉じる(強制)
        if (gameManager.playerInputAction.UI.OpenMenu.triggered || gameManager.playerInputAction.UI.CloseMenu.triggered)
        {
            gameManager.isOpenMenu = false;
            CloseMenuWindow();
        }

        //カール上移動
        if (gameManager.playerInputAction.UI.CursorMoveUp.triggered)
        {
            nowCursorImage.SetActive(false);
            nowCursorNum --;
            if (nowCursorNum < 0) nowCursorNum = menuWindow.transform.childCount - 1;
        }

        //カーソル下移動
        if (gameManager.playerInputAction.UI.CursorMoveDown.triggered)
        {
            nowCursorImage.SetActive(false);
            nowCursorNum ++;
            if (nowCursorNum >= menuWindow.transform.childCount) nowCursorNum = 0;
        }

        // //カール左移動
        // if (gameManager.playerInputAction.UI.CursorMoveLeft.triggered)
        // {
        //     nowCursorImage.SetActive(false);
        //     nowCursorNum--;
        //     if (nowCursorNum % 2 == 1 || nowCursorNum % 2 == -1) nowCursorNum += 2;
        // }

        // //カーソル右移動
        // if (gameManager.playerInputAction.UI.CursorMoveRight.triggered)
        // {
        //     nowCursorImage.SetActive(false);
        //     nowCursorNum++;
        //     if (nowCursorNum % 2 == 0) nowCursorNum -= 2;
        // }

        // //カール上移動
        // if (gameManager.playerInputAction.UI.CursorMoveUp.triggered)
        // {
        //     nowCursorImage.SetActive(false);
        //     nowCursorNum -= 2;
        //     if (nowCursorNum < 0) nowCursorNum += menuWindow.transform.childCount;
        // }

        // //カーソル下移動
        // if (gameManager.playerInputAction.UI.CursorMoveDown.triggered)
        // {
        //     nowCursorImage.SetActive(false);
        //     nowCursorNum += 2;
        //     if (nowCursorNum >= menuWindow.transform.childCount) nowCursorNum -= menuWindow.transform.childCount;
        // }

        //メニューの選択
        if(gameManager.playerInputAction.UI.DecisionMenu.triggered)
        {
            switch(nowCursorNum)
            {
                case 0:
                    Debug.Log("持ち物");
                    backpack.SetActive(true);
                    CloseMenuWindow();
                    break;
                case 1:
                    Debug.Log("話す");
                    // menuWindow.SetActive(false);
                    textWindow.SetActive(true);
                    talkManager.Talk(playerTalkTopic.topicList[gameManager.playerTalkState].topic, false);
                    explain.SetActive(false);
                    break;
                case 2:
                    Debug.Log("セーブ");
                    gameManager.Save();
                    break;
                case 3:
                    Debug.Log("設定");
                    settingWindow.SetActive(true);
                    CloseMenuWindow();
                    isSettingWindow = true;
                    break;
                // case 4:
                //     Debug.Log("セーブ");
                //     gameManager.Save();
                //     break;
                // case 5:
                //     Debug.Log("タイトル");
                //     break;
            }
        }
    }

    //メニューの説明
    public void MenuExplainText()
    {
        switch(nowCursorNum)
            {
                case 0:
                    explainText.text = "集めたアイテムや情報を見ることができます";
                    break;
                case 1:
                    explainText.text = "私と話すよ";
                    break;
                case 2:
                    explainText.text = "ゲームの進行を保存します";
                    break;
                case 3:
                    explainText.text = "ゲーム内の設定を変えることができます";
                    break;
                // case 4:
                //     Debug.Log("セーブ");
                //     gameManager.Save();
                //     break;
                // case 5:
                //     Debug.Log("タイトル");
                //     break;
            }
    }
}