using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [Header("0より上の数字")]
    public int itemID; //アイテムのID

    [SerializeField, ReadOnly] private string itemName; // インスペクター上に表示するが編集不可

    [SerializeField] private ItemDataBase itemDatabase;

    [SerializeField] private TakeManager takeManager;

    private bool canGet;
    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    void Update()
    {
        if(canGet)
        {
            //操作説明
            if(GameManager.controllerType == GameManager.ControllerType.Unknown)
            {
                operationText.text = "Space : 調べる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
            {
                operationText.text = "● : 調べる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
            {
                operationText.text = "A : 調べる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
            {
                operationText.text = "B : 調べる";
            }

            if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
            {
                takeManager.TakeItem(ItemManager.instance.PickUp(itemID));
                StartCoroutine(DestroyObject());
            }
        }
    }


    //自分をデストロイする
    IEnumerator DestroyObject()
    {
        yield return null;
        
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canGet = true;
            operation.SetActive(true);
            //ShowOperation();
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canGet = false;
            operation.SetActive(false);
        }
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
