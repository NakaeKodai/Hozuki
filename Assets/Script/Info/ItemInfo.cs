using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("0より上の数字")]
    public int itemID; //アイテムのID

    //自分をデストロイする
    public void DestroyObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
