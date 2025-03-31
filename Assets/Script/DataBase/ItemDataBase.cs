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
    [Header("アイテムの説明を書いてね")]
    public string itemInfo;
}
