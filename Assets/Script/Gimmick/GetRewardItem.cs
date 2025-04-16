using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRewardItem : MonoBehaviour, IRewards
{
    [Header("0より上の数字")]
    public int itemID; //アイテムのID
    [SerializeField, ReadOnly] private string itemName; // インスペクター上に表示するが編集不可

    [SerializeField] private ItemDataBase itemDatabase;

    [SerializeField] private TakeManager takeManager;

    public void Reward()
    {
        takeManager.TakeItem(ItemManager.instance.PickUp(itemID));
    }


    //インスペクター上でアイテム名が分かりやすくなるように
    private void OnValidate()
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDataBase is not assigned in the Inspector!");
            return;
        }

        ItemData item = itemDatabase.itemList.Find(i => i.itemID == itemID);
        itemName = item != null ? item.itemName : "Unknown";
    }
}
