using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    //アイテムのデータベース
    [SerializeField] private ItemDataBase itemDataBase;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // シーンが変わっても消えないようにする
    }

    void Start()
    {
        //ゲームスタート時に全アイテムを未所持状態にする
        ReSetItemStatus();
    }

    //itemIDから、そのIDにあったアイテムをデータベースから探し、見つけたら情報を返す
    //もし見つからなかったら[0]番目のデータ(test)を返す
    public ItemData PickUp(int itemID)
    {
        ItemData pickUPitem = itemDataBase.itemList[0];
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemID == itemDataBase.itemList[i].itemID)
            {
                pickUPitem = itemDataBase.itemList[i];
                
                //アイテムを入手済みにする
                itemDataBase.itemList[i].haveStatus = ItemData.Status.HAVE;
                break;
            }
        }
        // //アイテムを入手済みにする
        // itemDataBase.itemList[itemID].haveStatus = ItemData.Status.HAVE;
        return pickUPitem;
    }

    //アイテムの名前からアイテムの情報のテキストを返す (もっといいのがあるはず)
    public string SearchInfoText(string itemName)
    {
        string itemInfoText = itemDataBase.itemList[0].itemInfo;
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemName == itemDataBase.itemList[i].itemName)
            {
                itemInfoText = itemDataBase.itemList[i].itemInfo;
                break;
            }
        }
        return itemInfoText;
    }

    //アイテムの説明からアイテムの種類を返す（カーソルでアイテムの名前を取得してないっぽいので新しく作るのごちゃごちゃしそうなので）
    public string SearchTypeText(string itemInfo)
    {
        string itemTypeText = itemDataBase.itemList[0].itemInfo;
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemInfo == itemDataBase.itemList[i].itemInfo)
            {
                itemTypeText = itemDataBase.itemList[i].useType.ToString();
                break;
            }
        }
        return itemTypeText;
    }

    //アイテムの説明から表示する画像を返す（カーソルでアイテムの名前を取得してないっぽいので新しく作るのごちゃごちゃしそうなので）
    public Sprite SearchShowImage(string itemInfo)
    {
        Sprite itemShowImage = itemDataBase.itemList[0].showImage;
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemInfo == itemDataBase.itemList[i].itemInfo)
            {
                itemShowImage = itemDataBase.itemList[i].showImage;
                break;
            }
        }
        return itemShowImage;
    }

    //アイテムの説明からアイテムのIDを返す（カーソルでアイテムの名前を取得してないっぽいので新しく作るのごちゃごちゃしそうなので）
    public int SearchID(string itemInfo)
    {
        int itemID = itemDataBase.itemList[0].itemID;
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemInfo == itemDataBase.itemList[i].itemInfo)
            {
                itemID = itemDataBase.itemList[i].itemID;
                break;
            }
        }
        return itemID;
    }

    //アイテムの名前からアイテムのIDを返す（カーソルでアイテムの名前を取得してないっぽいので新しく作るのごちゃごちゃしそうなので）
    public int SearchIDForName(string itemName)
    {
        int itemID = itemDataBase.itemList[0].itemID;
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemName == itemDataBase.itemList[i].itemName)
            {
                itemID = itemDataBase.itemList[i].itemID;
                break;
            }
        }
        return itemID;
    }


    //所持状態のリセット(デモ版用)
    private void ReSetItemStatus()
    {
        for(int i = 0; i < itemDataBase.itemList.Count; i++)
        {
            if(itemDataBase.itemList[i].haveStatus != ItemData.Status.NOTHAVE)
            {
                itemDataBase.itemList[i].haveStatus = ItemData.Status.NOTHAVE;
            }
        }
    }
}
