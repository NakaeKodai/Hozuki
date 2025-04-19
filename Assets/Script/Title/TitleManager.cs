using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public PlayerInputAction playerInputAction; //inputSystemの変数

    public enum TitleStatus
    {
        START,
        CHARASELECT
    }

    public TitleStatus status;

    public GameObject startPanel;
    public Animator startPanelAnimator;

    public GameObject explain;
    public TextMeshProUGUI explainText;

    public GameObject charaSelectPanel;
    public Animator charaPanelAnimator;

    private int nowCursorNum = 0;
    private int beforeCursorNum = 1; //nowCursorNumとは違う数値

    private GameObject nowCursor;
    private GameObject nowCursorImage;

    public string cursorSE = "カーソル移動";
    public string decisionSE = "決定";

    // Start is called before the first frame update
    void Start()
    {
        status = TitleStatus.START;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(status == TitleStatus.START)
        {
            StartControl();
        }
        
    }

    void StartControl()
    {
        if(beforeCursorNum != nowCursorNum)
        {
            nowCursor = startPanel.transform.GetChild(nowCursorNum).gameObject;
            nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
            beforeCursorNum = nowCursorNum;
            nowCursorImage.SetActive(true);
            SetExplainText();
        }

        //カール上移動
        if (playerInputAction.UI.CursorMoveUp.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum --;
            if (nowCursorNum < 0) nowCursorNum = startPanel.transform.childCount - 1;
        }

        //カーソル下移動
        if (playerInputAction.UI.CursorMoveDown.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum ++;
            if (nowCursorNum >= startPanel.transform.childCount) nowCursorNum = 0;
        }

        //メニューの選択
        if(playerInputAction.UI.DecisionMenu.triggered)
        {
            SoundManager.instance.PlaySE(decisionSE);
            switch(nowCursorNum)
            {
                case 0:
                    Debug.Log("はじめる");
                    status = TitleStatus.CHARASELECT;
                    startPanel.SetActive(false);
                    explain.SetActive(false);
                    charaSelectPanel.SetActive(true);
                    charaPanelAnimator.SetTrigger("FadeIn");
                    beforeCursorNum = -1;
                    break;
                case 1:
                    Debug.Log("つづき");
                    break;
                case 2:
                    Debug.Log("設定");
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
    public void SetExplainText()
    {
        switch(nowCursorNum)
            {
                case 0:
                    explainText.text = "ゲームを開始します";
                    break;
                case 1:
                    explainText.text = "デモ版のため実行できません";
                    break;
                case 2:
                    explainText.text = "デモ版のため実行できません";
                    break;
            }
    }
}
