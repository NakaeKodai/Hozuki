using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [Header("0より上の数字")]
    public int itemID; //アイテムのID

    [SerializeField, ReadOnly] private string itemName; // インスペクター上に表示するが編集不可

    [SerializeField] private ItemDataBase itemDataBase; // 手動でアタッチする
    [SerializeField] private TalkManager talkManager;
    private TalkTopic talkTopic;

    public string openSE,closeSE;

    void Start()
    {
        talkTopic = GetComponent<TalkTopic>();
    }

    private void OnValidate()
    {
        if (itemDataBase == null)
        {
            Debug.LogError("ItemDataBase is not assigned in the Inspector!");
            return;
        }

        ItemData item = itemDataBase.itemList.Find(i => i.itemID == itemID);
        itemName = item != null ? item.itemName : "Unknown";
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for(int i = 0; i < itemDataBase.itemList.Count; i++)
            {
                if(itemID == itemDataBase.itemList[i].itemID)
                {   
                    if(itemDataBase.itemList[i].haveStatus == ItemData.Status.HAVE)
                    {
                        talkManager.Talk(talkTopic.topicList[0].topic, true);
                        SoundManager.instance.PlaySE(openSE);
                        Destroy(gameObject);
                        break;
                    }
                    else
                    {
                        talkManager.Talk(talkTopic.topicList[1].topic, true);
                        SoundManager.instance.PlaySE(closeSE);
                        break;
                    }
                }
            }
        }
    }
}
