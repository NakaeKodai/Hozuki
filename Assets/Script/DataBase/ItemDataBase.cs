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
    [Header("アイテムの説明を書いてね")]
    public string itemInfo;
    [Header("0:未入手, 1:入手済, 2:使用済")]
    public byte type;
}
