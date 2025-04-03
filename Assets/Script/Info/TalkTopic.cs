using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTopic : MonoBehaviour
{
    [Header("調べて話すことができるオブジェクトにアタッチしていたらTrue")]
    public bool isTalkObject = false;

    [Header("会話内容　※一行で収まるように※")]
    [SerializeField]
    public List<Topic> topicList = new List<Topic>(); //stringのListのList

    [System.Serializable]
    public class Topic
    {
        public List<string> topic = new List<string>(); //stringのList
    }

    private bool canTalk;

    [SerializeField] private TalkManager talkManager;
    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    void Update()
    {
        if(isTalkObject)
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
                    talkManager.Talk(topicList[0].topic, true);
                }
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
