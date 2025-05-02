using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public float startTime = 3.0f; // タイマー開始時間（秒）
    private float currentTime;
    private bool timerFinished = false;

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

    public GameObject charaImageBackGround;
    public Image charaImage;
    public List<Sprite> charaImageList;

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
        currentTime = startTime;
        timerFinished = false;
        status = TitleStatus.START;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerFinished)
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
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                timerFinished = true;
                Debug.Log("タイマー終了");
            }
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
        if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum --;
            if (nowCursorNum < 0) nowCursorNum = startPanel.transform.childCount - 1;
        }

        //カーソル下移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum ++;
            if (nowCursorNum >= startPanel.transform.childCount) nowCursorNum = 0;
        }

        //メニューの選択
        if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered)
        {
            currentTime = startTime;
            timerFinished = false;
            SoundManager.instance.PlaySE(decisionSE);
            switch(nowCursorNum)
            {
                case 0:
                    Debug.Log("はじめる");
                    status = TitleStatus.CHARASELECT;
                    startPanel.SetActive(false);
                    startExplain.SetActive(false);
                    charaImageBackGround.SetActive(true);
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
            if(nowCursorNum != charaSelectPanel.transform.childCount - 1) charaImage.sprite = charaImageList[nowCursorNum];
            nowCursor = charaSelectPanel.transform.GetChild(nowCursorNum).gameObject;
            nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
            beforeCursorNum = nowCursorNum;
            nowCursorImage.SetActive(true);
            SetCharaExplainText();
        }

        //カール上移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum --;
            if (nowCursorNum < 0) nowCursorNum = charaSelectPanel.transform.childCount - 1;
        }

        //カーソル下移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage.SetActive(false);
            nowCursorNum ++;
            if (nowCursorNum >= charaSelectPanel.transform.childCount) nowCursorNum = 0;
        }

        //メニューの選択
        if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered)
        {
            currentTime = startTime;
            timerFinished = false;
            SoundManager.instance.PlaySE(decisionSE);
            switch(nowCursorNum)
            {
                case 0:
                    Debug.Log("走力ちゃん");
                    GameManager.charactor = GameManager.Charactor.RUNNER;
                    GameManager.instance.HP = 5;
                    GameManager.instance.MaxHP = 5;
                    StartCoroutine(ChengeScene("Runner"));
                    break;
                case 1:
                    Debug.Log("知力くん");
                    GameManager.charactor = GameManager.Charactor.INTELLI;
                    GameManager.instance.HP = 3;
                    GameManager.instance.MaxHP = 3;
                    StartCoroutine(ChengeScene("Intelli"));
                    break;
                case 2:
                    Debug.Log("筋力くん");
                    GameManager.charactor = GameManager.Charactor.POWER;
                    GameManager.instance.HP = 5;
                    GameManager.instance.MaxHP = 5;
                    StartCoroutine(ChengeScene("Power"));
                    break;
                case 3:
                    Debug.Log("戻る");
                    status = TitleStatus.START;
                    charaImageBackGround.SetActive(false);
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
                    charaExplainText.text = "パズルとQTEが主なゲーム内容となっています";
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
