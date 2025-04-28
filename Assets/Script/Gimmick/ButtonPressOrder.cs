using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPressOrder : MonoBehaviour
{
    private bool isOpen,inPlayer, isLook,isClear;
    public string openSE = "鍵を開ける";

    [SerializeField] private GameObject lockedimage;
    [SerializeField] private Sprite image;
    [SerializeField] private Image LockedDisplay;
    [SerializeField] private GameObject password_Prefab;
    private TextMeshProUGUI numberText;
    private GameObject nowCursor;

    private byte[] paswordOrder; //入力の内容
    public byte[] answerOrder; //答えのない内容

    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    [Header("報酬のスクリプトに位置をこのスクリプトより上にしてね")]
    public MonoBehaviour reward;


    // Start is called before the first frame update
    void Start()
    {
        paswordOrder = new byte[answerOrder.Length];
        Debug.Log(paswordOrder.Length);

        ResetPassword();
    }

    // Update is called once per frame
    void Update()
    {
        if(inPlayer && !isClear)
        {
            if(!isLook)
            {
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
                if(!isOpen)
                {
                    if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered
                    && !GameManager.instance.isOpenMenu)
                    {
                        LookPassword();
                        // LockedDisplay.sprite = image;
                        // lockedimage.SetActive(true);
                        // GameManager.instance.isOtherMenu = true;
                        // isLook = true;
                    }
                }
                else
                {
                    if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered
                    && !GameManager.instance.isOpenMenu)
                    {
                        if(reward is IRewards action)
                        {
                            Debug.Log("報酬だよ");
                            action.Reward();
                            isClear =  true;
                        }
                    }
                }
            }
            else
            {
                // 操作説明
                if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                {
                    operationText.text = "ctrl : 閉じる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                {
                    operationText.text = "× : 閉じる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                {
                    operationText.text = "B : 閉じる";
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                {
                    operationText.text = "A : 閉じる";
                }
                if(GameManager.instance.playerInputAction.UI.CloseMenu.triggered)
                {
                    foreach (Transform child in lockedimage.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    lockedimage.SetActive(false);
                    GameManager.instance.isOtherMenu = false;
                    isLook = false;
                }
            }
        }
        
    }

    public void SetPasswaord(byte pass)
    {
        for(int i = 0;i < paswordOrder.Length;i++)
        {
            if(paswordOrder[i] != 0 && paswordOrder[i] != 1)
            {
                paswordOrder[i] = pass;
                break;
            }
        }

        if(CheckPassword()) CheckAnswer(); 
    }

    // パスワードが全て埋まっているか確認
    public bool CheckPassword()
    {
        for(int i = 0;i < paswordOrder.Length;i++)
        {
            if(paswordOrder[i] != 0 && paswordOrder[i] != 1)
            {
                return false;
            }
        }

        return true;
    }

    // 入力したものと答え合わせ
    public void CheckAnswer()
    {
        bool clear = true;
        for(int i = 0;i < paswordOrder.Length;i++)
        {
            if(paswordOrder[i] != answerOrder[i])
            {
                clear = false;
                ResetPassword();
                break;
            }
        }

        if(clear)
        {
            SoundManager.instance.PlaySE(openSE);
            isOpen = true;
        }
    }

    private void LookPassword()
    {
        for(int i = 0;i < answerOrder.Length;i++)
        {
            Instantiate(password_Prefab, lockedimage.transform);
        }

        for(int i = 0;i < answerOrder.Length;i++)
        {
            nowCursor = lockedimage.transform.GetChild(i).gameObject;
            numberText = nowCursor.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            if(paswordOrder[i] == 0) numberText.text = "0";
            else if(paswordOrder[i] == 1) numberText.text = "1";
            else numberText.text = " ";
        }

        LockedDisplay.sprite = image;
        lockedimage.SetActive(true);
        GameManager.instance.isOtherMenu = true;
        isLook = true;
    }

    private void ResetPassword()
    {
        for(int i = 0;i < answerOrder.Length;i++)
        {
            paswordOrder[i] = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
            operation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = false;
            operation.SetActive(false);
        }
    }
}
