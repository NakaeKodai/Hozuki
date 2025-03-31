using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("0より上の数字")]
    public int itemID; //アイテムのID

    [SerializeField, ReadOnly] private string itemName; // インスペクター上に表示するが編集不可

    [SerializeField] private ItemDataBase itemDatabase;

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

    //自分をデストロイする
    public void DestroyObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
