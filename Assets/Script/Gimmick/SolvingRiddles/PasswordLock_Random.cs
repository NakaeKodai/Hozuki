using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PasswordLock_Random : MonoBehaviour
{
    private bool isClear, inPlayer, isPlay;
    
    public string cursorSE = "カーソル移動";
    public string openSE = "鍵を開ける";


    //本体のオブジェクトの変数
    [SerializeField] private GameObject lockedimage;
    [SerializeField] private Sprite image;
    [SerializeField] private Image LockedDisplay;
    [SerializeField] private GameObject password_Prefab;

    //ランダムな数オブジェクトを置く変数
    public List<GameObject> randomPrefab = new List<GameObject>();

    private int minCount = 1;  // 最小生成数
    private int maxCount = 9;  // 最大生成数
    

    //カーソル移動の変数
    private int nowCursorNum = 0;
    private int beforeCursorNum = 1; //nowCursorNumとは違う数値
    private TextMeshProUGUI numberText;
    private GameObject nowCursor;
    private GameObject nowCursorImage;

    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    public List<int> answerNumber = new List<int>();

    [SerializeField]
    private List<int> passwordNumber = new List<int>();

    
    [Header("報酬のスクリプトに位置をこのスクリプトより上にしてね")]
    public MonoBehaviour reward;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < randomPrefab.Count; i++)
        {
            int spawnCount = Random.Range(minCount, maxCount + 1);
            answerNumber.Add(spawnCount);
            for (int j = 0; j < spawnCount; j++)
            {
                // ランダムな位置を決定（x: 63～76, y: 54～73）
                float x = Random.Range(63f, 77f); // 上限は排他的なので+1
                float y = Random.Range(54f, 74f);

                Vector3 spawnPosition = new Vector3(x, y, 0f); // Z=0と仮定

                // プレハブを生成
                GameObject obj = Instantiate(randomPrefab[i], spawnPosition, Quaternion.identity);
                obj.transform.parent = this.transform;
            }
        }
    }

    void Update()
    {
        if(inPlayer && !isClear)
        {
            if(!isPlay)
            {
                // 入力前の処理

                // 操作説明
                if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                {
                    operationText.text = "Space : 調べる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                {
                    operationText.text = "● : 調べる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                {
                    operationText.text = "A : 調べる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                {
                    operationText.text = "B : 調べる";
                }

                if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered
                && !GameManager.instance.isOpenMenu)
                {
                    ResetPassword();
                    LockedDisplay.sprite = image;
                    lockedimage.SetActive(true);
                    // operation.SetActive(false);
                    GameManager.instance.isOtherMenu = true;
                    isPlay = true;
                }
            }
            else
            {
                // パスワード入力の処理

                // 操作説明
                if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                {
                    operationText.text = "ctrl : 閉じる　　W S : 数字変更　　A D : 桁変更";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                {
                    operationText.text = "× : 閉じる　　↑ ↓ : 数字変更　　← → : 桁変更";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                {
                    operationText.text = "B : 閉じる　　↑ ↓ : 数字変更　　← → : 桁変更";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                {
                    operationText.text = "A : 閉じる　　↑ ↓ : 数字変更　　← → : 桁変更";
                }

                if(GameManager.instance.playerInputAction.UI.CloseMenu.triggered)
                {
                    lockedimage.SetActive(false);
                    // operation.SetActive(true);
                    GameManager.instance.isOtherMenu = false;
                    isPlay = false;
                }
                

                //カーソル変更と数字変更
                if(beforeCursorNum != nowCursorNum)
                {
                    nowCursor = lockedimage.transform.GetChild(nowCursorNum).gameObject;
                    nowCursorImage = nowCursor.transform.GetChild(0).gameObject;
                    numberText = nowCursor.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                    beforeCursorNum = nowCursorNum;
                    nowCursorImage.SetActive(true);
                }

                //数字プラス
                if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered)
                {
                    SoundManager.instance.PlaySE(cursorSE);
                    passwordNumber[nowCursorNum]++;
                    if(passwordNumber[nowCursorNum] >= 10) passwordNumber[nowCursorNum] = 0;
                    numberText.text = passwordNumber[nowCursorNum] + "";
                    CheckPassword();
                }

                //数字マイナス
                if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered)
                {
                    SoundManager.instance.PlaySE(cursorSE);
                    passwordNumber[nowCursorNum]--;
                    if(passwordNumber[nowCursorNum] < 0) passwordNumber[nowCursorNum] = 9;
                    numberText.text = passwordNumber[nowCursorNum] + "";
                    CheckPassword();
                }

                //カール左移動
                if (GameManager.instance.playerInputAction.UI.CursorMoveLeft.triggered)
                {
                    SoundManager.instance.PlaySE(cursorSE);
                    nowCursorImage.SetActive(false);
                    nowCursorNum--;
                    if (nowCursorNum < 0) nowCursorNum = answerNumber.Count - 1;
                }

                //カーソル右移動
                if (GameManager.instance.playerInputAction.UI.CursorMoveRight.triggered)
                {
                    SoundManager.instance.PlaySE(cursorSE);
                    nowCursorImage.SetActive(false);
                    nowCursorNum++;
                    if (nowCursorNum >= answerNumber.Count) nowCursorNum = 0;
                }
            }
        }
        
    }

    private void CheckPassword()
    {
        int count = 0;
        for(int i  = 0;i < answerNumber.Count;i++)
        {
            if(passwordNumber[i] == answerNumber[i]) count++;
            else break;
        }
        if(count == answerNumber.Count)
        {
            SoundManager.instance.PlaySE(openSE);
            Debug.Log("当たり");
            if(reward is IRewards action)
            {
                GameManager.instance.isOtherMenu = false;
                operation.SetActive(false);
                lockedimage.SetActive(false);
                Debug.Log("報酬だよ");
                action.Reward();
                isClear =  true;
            }
        }
    }

    private void ResetPassword()
    {
        nowCursorNum = 0;
        beforeCursorNum = 1;
        foreach (Transform child in lockedimage.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0;i < answerNumber.Count;i++)
        {
            Instantiate(password_Prefab, lockedimage.transform);
            passwordNumber[i] = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        inPlayer = true;
        operation.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        inPlayer = false;
        operation.SetActive(false);
    }
}
