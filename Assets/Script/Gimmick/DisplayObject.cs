using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayObject : MonoBehaviour
{
    private bool canSee,isSee;

    [SerializeField] private Sprite image;
    [SerializeField] private GameObject display;
    [SerializeField] private Image displayImage;

    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    // Update is called once per frame
    void Update()
    {
        if(canSee && !GameManager.instance.isOtherMenu)
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


            if(!isSee)
            {
                if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
                {
                    GameManager.instance.isOtherMenu = true;
                    isSee = true;
                    displayImage.sprite = image;
                    display.SetActive(true);
                    //操作説明
                    if(GameManager.controllerType == GameManager.ControllerType.Unknown)
                    {
                        operationText.text = "ctrl : 閉じる";
                    }
                    else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
                    {
                        operationText.text = "× : 閉じる";
                    }
                    else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
                    {
                        operationText.text = "B : 閉じる";
                    }
                    else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
                    {
                        operationText.text = "A : 閉じる";
                    }
                }
            }
        }
        else if(canSee && isSee)
        {
            if(GameManager.instance.playerInputAction.UI.CloseMenu.triggered)
            {
                GameManager.instance.isOtherMenu = false;
                isSee = false;
                display.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canSee = true;
            operation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            canSee = false;
            operation.SetActive(false);
        }
    }
}
