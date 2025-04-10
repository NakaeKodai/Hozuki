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
}
