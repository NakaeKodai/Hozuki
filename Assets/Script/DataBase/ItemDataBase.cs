using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> itemList = new List<ItemData>();
}

[System.Serializable]
public class ItemData
{
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    
    public enum Status
    {
        NOTHAVE, //未所持
        HAVE, //所持
        WASHAVE //過去所持
    }

    public Status haveStatus;

    public enum UseType
    {
        NOUSE,//使用不可（フラグ用）
        SHOWIMAGE,//画像表示
        GETITEM//アイテム入手
    }
    [Header("アイテムの種類")]
    public UseType useType;

    [Header("アイテムの説明を書いてね")]
    public string itemInfo;

    [Header("アイテム使用に関するやつ（いらないの空白でおけ）")]
    public Sprite showImage;//画像表示のための画像
    public string getItemName;//入手できるアイテムのID

}
