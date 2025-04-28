using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PressButton : MonoBehaviour
{
    [SerializeField] private ButtonPressOrder buttonPressOrder;

    private bool inPlayer;

    public byte sendNum;

    public string cursorSE = "カーソル移動";

    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    void Update()
    {
        if(inPlayer)
        {
            // 操作説明
            if(GameManager.controllerType == GameManager.ControllerType.Unknown)
            {
                operationText.text = "Space : 押す";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
            {
                operationText.text = "● : 押す";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
            {
                operationText.text = "A : 押す";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
            {
                operationText.text = "B : 押す";
            }

            if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered
            && !GameManager.instance.isOpenMenu)
            {
                Debug.Log(sendNum+"を送ったよ");
                SoundManager.instance.PlaySE(cursorSE);
                buttonPressOrder.SetPasswaord(sendNum);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
            operation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = false;
            operation.SetActive(false);
        }
    }
}
