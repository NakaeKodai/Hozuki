using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    //持ち物の管理全般

    public GameManager gameManager;
    public UIManager  uiManager;
    public ItemManager itemManager;
    public GameObject itemPrefab;
    public GameObject nothingText;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //メニューを再び開く、または戻るボタンを押すと、BackPackメニューが閉じて、メニューが開く
        if (gameManager.playerInputAction.UI.OpenMenu.triggered || gameManager.playerInputAction.UI.CloseMenu.triggered)
        {
            gameObject.SetActive(false);
            uiManager.OpenMenuWindow();
        }

        //アイテムを開いていたら情報を、情報を開いていたらアイテムを開く
        if (gameManager.playerInputAction.UI.PageMoveRight.triggered || gameManager.playerInputAction.UI.PageMoveLeft.triggered)
        {
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
                SelectControlItem();
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

    public void AddBackpack_Infomation(ItemData pickUPitem)
    {
        //Prefabを作成し、情報欄に追加
        GameObject instance = Instantiate(itemPrefab, content_Info);

        //追加した情報のimageをセットする
        GameObject childObject = instance.transform.GetChild(0).gameObject;//imageを取る
        Image itemImage = childObject.GetComponent<Image>();
        itemImage.sprite = pickUPitem.itemImage;

        //追加した情報のTextをセットする
        childObject = instance.transform.GetChild(1).gameObject;//TMPを取る
        TextMeshProUGUI itemText = childObject.GetComponent<TextMeshProUGUI>();
        itemText.text = pickUPitem.itemName;
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
        if (gameManager.playerInputAction.UI.CursorMoveUp.triggered && content_Item.childCount > 1)
        {
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
        if (gameManager.playerInputAction.UI.CursorMoveDown.triggered && content_Item.childCount > 1)
        {
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
            TextMeshProUGUI itemText = nowCursorImage_Info.GetComponent<TextMeshProUGUI>();
            itemTextInfo_Info.text = itemManager.SearchInfoText(itemText.text);

            //カーソル表示
            nowCursorImage_Info = nowCursor_Info.transform.GetChild(2).gameObject;
            beforeCursorNum_Info= nowCursorNum_Info;
            nowCursorImage_Info.SetActive(true);
        }

        //カーソル上移動
        if (gameManager.playerInputAction.UI.CursorMoveUp.triggered && content_Info.childCount > 1)
        {
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
        if (gameManager.playerInputAction.UI.CursorMoveDown.triggered && content_Info.childCount > 1)
        {
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
