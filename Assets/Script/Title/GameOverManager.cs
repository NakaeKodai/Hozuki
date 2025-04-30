using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public float startTime = 3.0f; // タイマー開始時間（秒）
    private float currentTime;
    private bool timerFinished = false;

    public GameObject cursorPanel;

    public Animator gameOverAnimation;

    public TextMeshProUGUI operationText;

    private int nowCursorNum = 0;
    private int beforeCursorNum = 1; //nowCursorNumとは違う数値

    private GameObject nowCursor;
    private GameObject nowCursorImage;

    public string cursorSE = "カーソル移動";
    public string decisionSE = "決定";

    void Start()
    {
        currentTime = startTime;
        timerFinished = false;
        SoundManager.instance.PlayBGM("追われる");
    }

    void Update()
    {
        //操作説明
        if(GameManager.controllerType == GameManager.ControllerType.Unknown)
        {
            operationText.text = "Space : 決定";
        }
        else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
        {
            operationText.text = "● : 決定";
        }
        else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
        {
            operationText.text = "A : 決定";
        }
        else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
        {
            operationText.text = "B : 決定";
        }
        if(timerFinished)
        {        
            if(beforeCursorNum != nowCursorNum)
            {
                nowCursor = cursorPanel.transform.GetChild(nowCursorNum).gameObject;
                nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
                beforeCursorNum = nowCursorNum;
                nowCursorImage.SetActive(true);
            }

            //カール上移動
            if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered)
            {
                SoundManager.instance.PlaySE(cursorSE);
                nowCursorImage.SetActive(false);
                nowCursorNum --;
                if (nowCursorNum < 0) nowCursorNum = cursorPanel.transform.childCount - 1;
            }

            //カーソル下移動
            if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered)
            {
                SoundManager.instance.PlaySE(cursorSE);
                nowCursorImage.SetActive(false);
                nowCursorNum ++;
                if (nowCursorNum >= cursorPanel.transform.childCount) nowCursorNum = 0;
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
                        Debug.Log("リトライ");
                        if(GameManager.charactor == GameManager.Charactor.RUNNER)
                        {
                            GameManager.instance.HP = 5;
                            GameManager.instance.MaxHP = 5;
                            StartCoroutine(ChengeScene("Runner"));
                        }
                        else if(GameManager.charactor == GameManager.Charactor.INTELLI)
                        {
                            GameManager.instance.HP = 3;
                            GameManager.instance.MaxHP = 3;
                            StartCoroutine(ChengeScene("Intelli"));
                        }
                        else if(GameManager.charactor == GameManager.Charactor.POWER)
                        {
                            GameManager.instance.HP = 5;
                            GameManager.instance.MaxHP = 5;
                            StartCoroutine(ChengeScene("Power"));
                        }
                        break;
                    case 1:
                        Debug.Log("タイトル");
                        StartCoroutine(ChengeScene("Title"));
                        break;

                }
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

    IEnumerator ChengeScene(string sceneName)
    {
        gameOverAnimation.SetTrigger("FadeOut");
        yield return new WaitForSeconds(3.0f);
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene(sceneName);
    }
}
