using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfomationInfo : MonoBehaviour
{
    [Header("0より上の数字")]
    public int infoID; //アイテムのID

    [SerializeField, ReadOnly] private string infoName; // インスペクター上に表示するが編集不可

    [SerializeField] private InfomationDataBase infomationDataBase;

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
                takeManager.TakeInfomation(ItemManager.instance.PickUpInfomation(infoID));
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
        if (infomationDataBase == null)
        {
            Debug.LogError("InfomationDaInfomationDataBase is not assigned in the Inspector!");
            return;
        }

        InfomationData info = infomationDataBase.infomationList.Find(i => i.infoID == infoID);
        infoName = info != null ? info.infoName : "Unknown";
    }
}
