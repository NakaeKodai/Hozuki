using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadObject : MonoBehaviour
{
    private bool canTalk;

    [SerializeField] private TalkTopic talkTopic; // 会話内容

    [SerializeField] private TalkManager talkManager;
    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;
    

    void Update()
    {
        if(canTalk && !GameManager.instance.isOtherMenu)
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

            if(!operation.activeSelf) operation.SetActive(true);


            if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
            {
                talkManager.Talk(talkTopic.topicList[0].topic, true);
            }
        }
        
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
}
