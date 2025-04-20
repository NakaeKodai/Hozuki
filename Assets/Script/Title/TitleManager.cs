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

    public Animator logoAnimation;

    public GameObject startPanel;

    public GameObject startExplain;
    public TextMeshProUGUI startExplainText;

    public GameObject charaExplain;
    public TextMeshProUGUI charaExplainText;

    public GameObject charaSelectPanel;
    public Animator charaPanelAnimator;

    public GameObject blackoutPanel;
    public Animator blackoutAnimator;

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
        else  if(status == TitleStatus.CHARASELECT)
        {
            CharaSelectControl();
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
            SetStartExplainText();
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
                    startExplain.SetActive(false);
                    charaSelectPanel.SetActive(true);
                    charaExplain.SetActive(true);
                    charaPanelAnimator.SetTrigger("FadeIn");
                    beforeCursorNum = -1;
                    break;
                case 1:
                    Debug.Log("つづき");
                    break;
                case 2:
                    Debug.Log("設定");
                    break;

            }
        }
    }

    //メニューの説明
    void SetStartExplainText()
    {
        switch(nowCursorNum)
            {
                case 0:
                    startExplainText.text = "ゲームを開始します";
                    break;
                case 1:
                    startExplainText.text = "デモ版のため実行できません";
                    break;
                case 2:
                    startExplainText.text = "デモ版のため実行できません";
                    break;
            }
    }

    void CharaSelectControl()
    {
        if(beforeCursorNum != nowCursorNum)
        {
            nowCursor = charaSelectPanel.transform.GetChild(nowCursorNum).gameObject;
            nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
            beforeCursorNum = nowCursorNum;
            nowCursorImage.SetActive(true);
            SetCharaExplainText();
        }

        //カール上移動
        if (playerInputAction.UI.CursorMoveUp.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum --;
            if (nowCursorNum < 0) nowCursorNum = charaSelectPanel.transform.childCount - 1;
        }

        //カーソル下移動
        if (playerInputAction.UI.CursorMoveDown.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum ++;
            if (nowCursorNum >= charaSelectPanel.transform.childCount) nowCursorNum = 0;
        }

        //メニューの選択
        if(playerInputAction.UI.DecisionMenu.triggered)
        {
            SoundManager.instance.PlaySE(decisionSE);
            switch(nowCursorNum)
            {
                case 0:
                    Debug.Log("走力ちゃん");
                    StartCoroutine(ChengeScene("Runner"));
                    break;
                case 1:
                    Debug.Log("知力くん");
                    StartCoroutine(ChengeScene("Intelli"));
                    break;
                case 2:
                    Debug.Log("筋力くん");
                    StartCoroutine(ChengeScene("Power"));
                    break;
                case 3:
                    Debug.Log("戻る");
                    status = TitleStatus.START;
                    charaSelectPanel.SetActive(false);
                    charaExplain.SetActive(false);
                    startPanel.SetActive(true);
                    startExplain.SetActive(true);
                    logoAnimation.SetTrigger("FadeIn");
                    nowCursorImage.SetActive(false);
                    nowCursorNum = 0;
                    beforeCursorNum = -1;
                    break;

            }
        }
    }

    void SetCharaExplainText()
    {
        switch(nowCursorNum)
            {
                case 0:
                    charaExplainText.text = "敵とのチェイスが主なゲーム内容となっています";
                    break;
                case 1:
                    charaExplainText.text = "ナゾ解きが主なゲーム内容となっています";
                    break;
                case 2:
                    charaExplainText.text = "筋力くんが主なゲーム内容となっています";
                    break;
                case 3:
                    charaExplainText.text = "タイトル画面にもどります";
                    break;
            }
    }

    IEnumerator ChengeScene(string sceneName)
    {
        blackoutPanel.SetActive(true);
        blackoutAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneName);
    }
}
