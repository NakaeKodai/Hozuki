using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionalReadObject : MonoBehaviour
{
    private bool canTalk;

    [SerializeField] private TalkTopic talkTopic; // 会話内容

    [SerializeField] private TalkManager talkManager;
    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    public GameObject deleteObject;

    public string breakSE;

    private bool isCleared;

    public string clearText,notClearText;

    [Header("0より上の数字")]
    public int infoID; //アイテムのID

    [SerializeField, ReadOnly] private string infoName; // インスペクター上に表示するが編集不可

    [SerializeField] private InfomationDataBase infomationDataBase;
    

    void Update()
    {
        if(canTalk && !GameManager.instance.isOtherMenu)
        {
            if(!isCleared)
            {
                //操作説明
                if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                {
                    operationText.text = "Space : " + notClearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                {
                    operationText.text = "● : " + notClearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                {
                    operationText.text = "A : " + notClearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                {
                    operationText.text = "B : " + notClearText;
                }

                if(!operation.activeSelf) operation.SetActive(true);


                if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
                {
                    talkManager.Talk(talkTopic.topicList[0].topic, true);
                }
            }
            else
            {
                //操作説明
                if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                {
                    operationText.text = "Space : " + clearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                {
                    operationText.text = "● : " + clearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                {
                    operationText.text = "A : " + clearText;
                }
                else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                {
                    operationText.text = "B : " + clearText;
                }

                if(!operation.activeSelf) operation.SetActive(true);


                if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
                {
                    SoundManager.instance.PlaySE(breakSE);
                    gameObject.SetActive(false); //デモ版のため、1つだけにする。ほんとはクリア用のスクリプトをアタッチして、それをやるのがよさそう
                    deleteObject.SetActive(false);
                }
            }
        }

        if(infomationDataBase.infomationList[infoID].haveStatus == InfomationData.Status.HAVE)
        {
            isCleared = true;
        }
        else isCleared = false;
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canTalk = true;
            operation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canTalk = false;
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
