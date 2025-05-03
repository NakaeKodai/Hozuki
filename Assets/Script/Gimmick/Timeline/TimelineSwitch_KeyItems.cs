using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSwitch_KeyItems : MonoBehaviour
{
    // 特定アイテムの所持によってタイムラインを再生する

    [Header("所持で再生ならTrue、未所持で再生ならFalse")]
    public bool haveSwitch;

    [Header("0より上の数字")]
    public List<int> itemIDList = new List<int>(); //アイテムのID

    [SerializeField, ReadOnly] private string itemName; // インスペクター上に表示するが編集不可

    public PlayableDirector director;
    public GameObject playTimeline;
    public ControlTimeline controlTimeline;

    [SerializeField] private ItemDataBase itemDataBase; // 手動でアタッチする

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("キャラ触れた");
            bool allHave = true;//全部所持でtrue維持
            for(int k = 0; k < itemIDList.Count; k++)
        {
            for(int i = 0; i < itemDataBase.itemList.Count; i++)
            {
                
                    if(itemIDList[k] == itemDataBase.itemList[i].itemID)
                {   
                    if(haveSwitch)
                    {
                        if(itemDataBase.itemList[i].haveStatus != ItemData.Status.HAVE)
                        {
                            // GameManager.instance.director = director;
                            // controlTimeline.director = director;
                            // playTimeline.SetActive(true);
                            // break;
                            allHave = false;
                            break;
                        }
                    }
                    else
                    {
                        if(itemDataBase.itemList[i].haveStatus != ItemData.Status.NOTHAVE)
                        {
                            // GameManager.instance.director = director;
                            // controlTimeline.director = director;
                            // playTimeline.SetActive(true);
                            // break;
                            allHave = false;
                            break;
                        }
                    }
                }
                
                
            }
        }

            if(allHave)
            {
                GameManager.instance.director = director;
                controlTimeline.director = director;
                playTimeline.SetActive(true);
            }
            else
            {
                Debug.Log("なんか未所持");
            }
        }
    }

    // private void OnValidate()
    // {
    //     if (itemDataBase == null)
    //     {
    //         Debug.LogError("ItemDataBase is not assigned in the Inspector!");
    //         return;
    //     }

    //     ItemData item = itemDataBase.itemList.Find(i => i.itemID == itemID);
    //     itemName = item != null ? item.itemName : "Unknown";
    // }
}
