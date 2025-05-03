using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    //持ち物の管理全般

    public UIManager  uiManager;
    public ItemManager itemManager;
    public GameObject itemPrefab;
    public GameObject infoPrefab;
    public GameObject nothingText;

    public ItemDataBase itemDataBase;
    public InfomationDataBase infomationDataBase;

    //持ち物欄を見ているか、情報欄を見ているか
    private bool isItemView = true;

    public GameObject itemView; //持ち物欄のオブジェクト
    public GameObject infomationView; //情報欄のオブジェクト

    public Image pageImage_Item;
    public Image pageImage_Info;
    public TextMeshProUGUI pageText_Item;
    public TextMeshProUGUI pageText_Info;

    private Color onColor = new Color(1.0f,1.0f,1.0f,1.0f); //選択時の色(非透明)
    private Color offColor = new Color(1.0f,1.0f,1.0f,0.3f);//非選択時の色(微透明)

    public string cursorSE = "カーソル移動";

    [Header("アイテム")]

    //アイテム関連
    public Transform content_Item;
    public RectTransform contentTransform_Item;
    public Image itemImageInfo_Item;
    public TextMeshProUGUI itemTextInfo_Item;

    public GameObject infoText_Item;
    public GameObject infoImage_Item;

    private GameObject nowCursor_Item;
    private GameObject nowCursorImage_Item;
    private int nowCursorNum_Item,beforeCursorNum_Item = -1;
    private int contentMoveControlUp_Item = 0;
    private int contentMoveControlDown_Item = 6;


    [Header("情報")]

    //情報関連
    public Transform content_Info;
    public RectTransform contentTransform_Info;
    public TextMeshProUGUI itemTextInfo_Info;

    public GameObject infoText_Info;

    private GameObject nowCursor_Info;
    private GameObject nowCursorImage_Info;
    private int nowCursorNum_Info,beforeCursorNum_Info = -1;
    private int contentMoveControlUp_Info = 0;
    private int contentMoveControlDown_Info = 6;

    [Header("アイテム使用")]

    //アイテム使用関連
    public GameObject selectUse;//使用確認ウインドウ
    public GameObject useCursor;//使かう
    public GameObject cancelCursor;//やめる
    public TextMeshProUGUI useText;//「使う」と書かれた文字
    private string useType;//カーソルのアイテムの種類
    private bool selectUseFlug = false;//使用確認ウインドウを開いてるか
    private bool selectUseConfirmation = true;//使用しますかのtrue or false

    [Header("画像表示")]
    public GameObject showImageObject;//表示画像のオブジェクト
    public GameObject showImage;//表示する画像
    private bool showImageFlug;//画像を表示中かを判定する

    [Header("アイテム入手")]
    public TakeManager takeManager;//アイテム入手のやつ
    
    //サウンド
    public string decisionSE = "決定";
    public string cancelSE = "キャンセル";
    public string errorSE = "無効";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //メニューを再び開く、または戻るボタンを押すと、BackPackメニューが閉じて、メニューが開く
        if (GameManager.instance.playerInputAction.UI.OpenMenu.triggered)
        {
            selectUse.SetActive(false);
            selectUseFlug = false;
            showImageObject.SetActive(false);
            showImageFlug = false;
            gameObject.SetActive(false);
            SoundManager.instance.PlaySE(cancelSE);
            uiManager.OpenMenuWindow();
        }

        //もどるボタンの処理
        if(GameManager.instance.playerInputAction.UI.CloseMenu.triggered)
        {
            SoundManager.instance.PlaySE(cancelSE);
            if(selectUseFlug)//アイテム使用確認ウインドウ
            {
                selectUse.SetActive(false);
                selectUseFlug = false;
                selectUseConfirmation = true;
            }
            else if(showImageFlug)//アイテムの画像表示
            {
                showImageObject.SetActive(false);
                showImageFlug = false;
            }
            else//メニュー閉じる（上のコピーして）
            {
                gameObject.SetActive(false);
                uiManager.OpenMenuWindow();
            }
        }

        //アイテムを開いていたら情報を、情報を開いていたらアイテムを開く
        if ((GameManager.instance.playerInputAction.UI.PageMoveRight.triggered || GameManager.instance.playerInputAction.UI.PageMoveLeft.triggered) && !selectUseFlug && !showImageFlug)
        {
            SoundManager.instance.PlaySE(cursorSE);
            if(isItemView) isItemView = false;
            else isItemView = true;
        }

        if(isItemView) //アイテム欄の処理
        {
            itemView.SetActive(true);
            infomationView.SetActive(false);

            ColorChenge(isItemView);

            //もしアイテムを何も持っていない場合の処理
            if(content_Item.childCount == 0)
            {
                nothingText.SetActive(true);
                infoImage_Item.SetActive(false);
                infoText_Item.SetActive(false);

            }
            else //一つでもアイテムを持っている場合の処理
            {
                nothingText.SetActive(false);
                infoImage_Item.SetActive(true);
                infoText_Item.SetActive(true);

                if(selectUseFlug)//アイテム使用確認ウインドウを開いている
                {
                    selectUseCursor();//カーソル移動

                    //決定ボタンで選択
                    if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered)
                    {
                        if(selectUseConfirmation)//使う
                        {
                            // Debug.Log(itemTextInfo_Item.text);
                            useType = itemManager.SearchTypeText(itemTextInfo_Item.text);
                            // Debug.Log(useType);
                            if(useType == "NOUSE")
                            {
                                Debug.Log("フラグ用アイテム");
                                SoundManager.instance.PlaySE(errorSE);
                            }
                            else if(useType == "SHOWIMAGE")
                            {
                                Debug.Log("画像表示");
                                SoundManager.instance.PlaySE(decisionSE);
                                showItemImage();
                            }
                            else if(useType == "GETITEM")
                            {
                                Debug.Log("アイテムゲット");
                                SoundManager.instance.PlaySE(decisionSE);
                                getItem();
                            }
                            else
                            {
                                Debug.Log("その他or正しく取得できてない");
                            } 
                        }
                        else//やめる
                        {
                            selectUse.SetActive(false);
                            SoundManager.instance.PlaySE(cancelSE);
                            selectUseFlug = false;
                            selectUseConfirmation = true;
                        }
                    }
                    
                }
                else if(!showImageFlug)//開いていない(普通のアイテム選択画面)
                {
                    SelectControlItem();
                    //アイテム使用確認ウインドウを開く
                    if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered){
                        // 「使う」と書かれたテキストの色変え
                        useType = itemManager.SearchTypeText(itemTextInfo_Item.text);
                        if(useType == "NOUSE")
                        {
                            useText.color = offColor;
                        }
                        else
                        {
                            useText.color = onColor;
                        }
                        // ウインドウ表示
                        selectUse.SetActive(true);
                        selectUseFlug = true;
                        useCursor.SetActive(true);
                        cancelCursor.SetActive(false);
                        selectUseConfirmation = true;
                        SoundManager.instance.PlaySE(decisionSE);
                        
                    }
                }
                
            }
        }
        else //情報欄の処理
        {
            itemView.SetActive(false);
            infomationView.SetActive(true);

            ColorChenge(isItemView);

            //もし情報を何も持っていない場合の処理
            if(content_Info.childCount == 0)
            {
                nothingText.SetActive(true);
                infoText_Info.SetActive(false);

            }
            else
            {
                //一つでも情報を持っている場合の処理
                nothingText.SetActive(false);
                infoText_Info.SetActive(true);
                SelectControlInfomation();
            }   
        }
    }

    //アイテムの追加処理
    public void AddBackpack_Item(ItemData pickUPitem)
    {
        //Prefabを作成し、アイテム欄に追加
        GameObject instance = Instantiate(itemPrefab, content_Item);

        //追加したアイテムのimageをセットする
        GameObject childObject = instance.transform.GetChild(0).gameObject;
        Image itemImage = childObject.GetComponent<Image>();
        itemImage.sprite = pickUPitem.itemImage;

        //追加したアイテムのTextをセットする
        childObject = instance.transform.GetChild(1).gameObject;//TMPを取る
        TextMeshProUGUI itemText = childObject.GetComponent<TextMeshProUGUI>();
        itemText.text = pickUPitem.itemName;
        // if(content_Item.childCount > 5) contentMoveControl ++;
    }

    public void AddBackpack_Infomation(InfomationData pickUPinfo)
    {
        //Prefabを作成し、情報欄に追加
        GameObject instance = Instantiate(infoPrefab, content_Info);

        //追加した情報のimageをセットする
        GameObject childObject = instance.transform.GetChild(0).gameObject;//imageを取る
        Image infoImage = childObject.GetComponent<Image>();
        infoImage.sprite = pickUPinfo.infoImage;

        //追加した情報のTextをセットする
        childObject = instance.transform.GetChild(1).gameObject;//TMPを取る
        TextMeshProUGUI infoText = childObject.GetComponent<TextMeshProUGUI>();
        infoText.text = pickUPinfo.infoName;
    }

    //アイテムのカーソル移動
    private void SelectControlItem()
    {
        if(beforeCursorNum_Item != nowCursorNum_Item)
        {
            nowCursor_Item = content_Item.GetChild(nowCursorNum_Item).gameObject;
            
            //Infoにimage
            nowCursorImage_Item = nowCursor_Item.transform.GetChild(0).gameObject;
            Image itemImage = nowCursorImage_Item.GetComponent<Image>();
            itemImageInfo_Item.sprite = itemImage.sprite;

            //InfoにText
            nowCursorImage_Item = nowCursor_Item.transform.GetChild(1).gameObject;
            TextMeshProUGUI itemText = nowCursorImage_Item.GetComponent<TextMeshProUGUI>();
            itemTextInfo_Item.text = itemManager.SearchInfoText(itemText.text);

            //カーソル表示
            nowCursorImage_Item = nowCursor_Item.transform.GetChild(2).gameObject;
            beforeCursorNum_Item = nowCursorNum_Item;
            nowCursorImage_Item.SetActive(true);

        }

        //カーソル上移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered && content_Item.childCount > 1)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage_Item.SetActive(false);
            nowCursorNum_Item --;
            if(nowCursorNum_Item <= contentMoveControlUp_Item && contentTransform_Item.anchoredPosition.y != 0)
            {
                Vector2 currentPos = contentTransform_Item.anchoredPosition;
                currentPos.y -= 100;
                contentTransform_Item.anchoredPosition = currentPos;
                contentMoveControlUp_Item--;
                contentMoveControlDown_Item--;

            }
            if (nowCursorNum_Item < 0)
            {
                nowCursorNum_Item = content_Item.childCount - 1;
                Vector2 currentPos = contentTransform_Item.anchoredPosition;
                if(content_Item.childCount > 5)
                {
                    currentPos.y = (content_Item.childCount - 5) * 100;
                }
                contentTransform_Item.anchoredPosition = currentPos;
                contentMoveControlUp_Item = content_Item.childCount - 7;
                if(contentMoveControlUp_Item < 0) contentMoveControlUp_Item = 0;
                contentMoveControlDown_Item = content_Item.childCount - 1;
            }
        }

        //カーソル下移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered && content_Item.childCount > 1)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage_Item.SetActive(false);
            nowCursorNum_Item ++;
            if(nowCursorNum_Item >= contentMoveControlDown_Item && contentMoveControlDown_Item < content_Item.childCount - 1)
            {
                Vector2 currentPos = contentTransform_Item.anchoredPosition;
                currentPos.y += 100;
                contentTransform_Item.anchoredPosition = currentPos;
                contentMoveControlUp_Item++;
                contentMoveControlDown_Item++;
            }
            if (nowCursorNum_Item >= content_Item.childCount)
            {
                nowCursorNum_Item = 0;
                Vector2 currentPos = contentTransform_Item.anchoredPosition;
                currentPos.y = 0;
                contentTransform_Item.anchoredPosition = currentPos;
                contentMoveControlUp_Item = 0;
                contentMoveControlDown_Item = 6;
            }
        }
    }

    //情報のカーソル移動
    private void SelectControlInfomation()
    {
        if(beforeCursorNum_Info != nowCursorNum_Info)
        {
            nowCursor_Info = content_Info.GetChild(nowCursorNum_Info).gameObject;

            //InfoにText
            nowCursorImage_Info = nowCursor_Info.transform.GetChild(1).gameObject;
            TextMeshProUGUI infoText = nowCursorImage_Info.GetComponent<TextMeshProUGUI>();
            itemTextInfo_Info.text = itemManager.SearchInfoText_Infomation(infoText.text);

            //カーソル表示
            nowCursorImage_Info = nowCursor_Info.transform.GetChild(2).gameObject;
            beforeCursorNum_Info= nowCursorNum_Info;
            nowCursorImage_Info.SetActive(true);
        }

        //カーソル上移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered && content_Info.childCount > 1)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage_Info.SetActive(false);
            nowCursorNum_Info --;
            if(nowCursorNum_Info <= contentMoveControlUp_Info && contentTransform_Info.anchoredPosition.y != 0)
            {
                Vector2 currentPos = contentTransform_Info.anchoredPosition;
                currentPos.y -= 100;
                contentTransform_Info.anchoredPosition = currentPos;
                contentMoveControlUp_Info--;
                contentMoveControlDown_Info--;

            }
            if (nowCursorNum_Info < 0)
            {
                nowCursorNum_Info = content_Info.childCount - 1;
                Vector2 currentPos = contentTransform_Info.anchoredPosition;
                if(content_Info.childCount > 5)
                {
                    currentPos.y = (content_Info.childCount - 5) * 100;
                }
                contentTransform_Info.anchoredPosition = currentPos;
                contentMoveControlUp_Info = content_Info.childCount - 7;
                if(contentMoveControlUp_Info < 0) contentMoveControlUp_Info = 0;
                contentMoveControlDown_Info = content_Info.childCount - 1;
            }
        }

        //カーソル下移動
        if (GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered && content_Info.childCount > 1)
        {
            SoundManager.instance.PlaySE(cursorSE);
            nowCursorImage_Info.SetActive(false);
            nowCursorNum_Info ++;
            if(nowCursorNum_Info >= contentMoveControlDown_Info && contentMoveControlDown_Info < content_Info.childCount - 1)
            {
                Vector2 currentPos = contentTransform_Info.anchoredPosition;
                currentPos.y += 100;
                contentTransform_Info.anchoredPosition = currentPos;
                contentMoveControlUp_Info++;
                contentMoveControlDown_Info++;
            }
            if (nowCursorNum_Info >= content_Info.childCount)
            {
                nowCursorNum_Info = 0;
                Vector2 currentPos = contentTransform_Info.anchoredPosition;
                currentPos.y = 0;
                contentTransform_Info.anchoredPosition = currentPos;
                contentMoveControlUp_Info = 0;
                contentMoveControlDown_Info = 6;
            }
        }
    }

    //アイテム使用確認ウインドウのカーソル移動
    private void selectUseCursor()
    {
        //カーソル移動（上下どっちも同じ）
        if (GameManager.instance.playerInputAction.UI.CursorMoveUp.triggered || GameManager.instance.playerInputAction.UI.CursorMoveDown.triggered)
        {
            SoundManager.instance.PlaySE(cursorSE);
            if(selectUseConfirmation)
            {
                selectUseConfirmation = false;
                useCursor.SetActive(false);
                cancelCursor.SetActive(true);
            }
            else
            {
                selectUseConfirmation = true;
                useCursor.SetActive(true);
                cancelCursor.SetActive(false);
            }
        }

    }

    //アイテムの画像表示
    private void showItemImage()
    {
        //アイテム使用確認ウインドウ閉じる
        selectUse.SetActive(false);
        selectUseFlug = false;
        selectUseConfirmation = true;

        // 画像表示
        showImageFlug = true;
        Image img = showImage.GetComponent<Image>();
        img.sprite = itemManager.SearchShowImage(itemTextInfo_Item.text);
        showImageObject.SetActive(true);
    }

    //アイテム入手
    private void getItem()
    {
        //ウインドウを閉じる
        selectUse.SetActive(false);
        selectUseFlug = false;
        showImageObject.SetActive(false);
        showImageFlug = false;
        gameObject.SetActive(false);
        // uiManager.OpenMenuWindow();

        //アイテムの入手
        int useItemID = itemManager.SearchID(itemTextInfo_Item.text);
        string getItemName = itemDataBase.itemList[useItemID].getItemName;
        int getItemID = itemManager.SearchIDForName(getItemName);
        deleteItem(itemDataBase.itemList[useItemID].itemName);
        // itemDataBase.itemList[useItemID].haveStatus = ItemData.Status.WASHAVE;
        takeManager.TakeItem(ItemManager.instance.PickUp(getItemID));
    }

    //アイテムの削除
    public void deleteItem(string targetName)
    {
        GameObject itemObject;//アイテムのオブジェクト
        GameObject itemChild;//アイテムのテキストのオブジェクト
        TextMeshProUGUI itemName;//アイテムのテキスト

        Debug.Log(targetName+"を削除するよ");
        Debug.Log(content_Item.gameObject.transform.childCount);

        for(int i = 0; i < content_Item.gameObject.transform.childCount; i++)
        {
            //各オブジェクト取得
            itemObject = content_Item.transform.GetChild(i).gameObject;
            itemChild = itemObject.transform.GetChild(1).gameObject;
            itemName = itemChild.GetComponent<TextMeshProUGUI>();

            Debug.Log(itemName.text+"を削除したい");

            // 名前が一致していた時のみ削除
            if(itemName.text == targetName)
            {
                Destroy(itemObject);
                itemDataBase.itemList[itemManager.SearchID(targetName)].haveStatus = ItemData.Status.WASHAVE;
                Debug.Log(itemName.text+"を削除した");
                break;
            }
        }
    }

    //今見ている欄を分かりやすくするために色を変更する処理
    private void ColorChenge(bool itemView)
    {
        if(itemView)
        {
            pageImage_Item.color = onColor;
            pageText_Item.color = onColor;
            pageImage_Info.color = offColor;
            pageText_Info.color = offColor;
        }
        else
        {
            pageImage_Item.color = offColor;
            pageText_Item.color = offColor;
            pageImage_Info.color = onColor;
            pageText_Info.color = onColor;
        }
    }
}
