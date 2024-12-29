using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    //設定関連の処理

    //今現在、明るさ調整

    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] GameObject menuWindow;

    [SerializeField]private Image lightImage; //明るさ調整用の画像
    [SerializeField] public List<Scrollbar> scrollbar;

    public Color normalColor,selectColor;

    Image scrollbarImage;
    GameObject scrollbarSelectImage;
    ColorBlock colorBlock;

    private int selectBarNum,beforeSelectBarNum = -1;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.playerInputAction.UI.CursorMoveRight.triggered)
        {
            if(scrollbar[selectBarNum].value <= 1.0f) scrollbar[selectBarNum].value += 0.1f;
        }
        if(gameManager.playerInputAction.UI.CursorMoveLeft.triggered)
        {
            if(scrollbar[selectBarNum].value >= 0.01f) scrollbar[selectBarNum].value -= 0.1f;
        }

        switch(selectBarNum)
        {
            case 0: //明るさ調整
                Color color = lightImage.color;
                color.a = 1.0f - scrollbar[selectBarNum].value;
                if(color.a >= 0.96f) color.a = 0.95f;
                lightImage.color = color;
            break;
        }

        //メニューを再び開く、または閉じたらメニューが開かれる
        if(gameManager.playerInputAction.UI.OpenMenu.triggered || gameManager.playerInputAction.UI.CloseMenu.triggered)
        {
            uiManager.OpenMenuWindow();
            uiManager.isSettingWindow = false;
            gameObject.SetActive(false);

        }
        SelectControl();
    }


    //カーソル移動
    void SelectControl()
    {
        if(beforeSelectBarNum != selectBarNum)
        {
            colorBlock = scrollbar[selectBarNum].colors;
            colorBlock.disabledColor = Color.gray;
            scrollbar[selectBarNum].colors = colorBlock;
            //scrollbarImage = scrollbar[selectBarNum].GetComponent<Image>();
            scrollbarSelectImage = scrollbar[selectBarNum].transform.GetChild(1).gameObject;
            scrollbarSelectImage.SetActive(true);
            //scrollbarImage.color = selectColor;
            beforeSelectBarNum = selectBarNum;
        }
        if(gameManager.playerInputAction.UI.CursorMoveUp.triggered)
        {
            colorBlock.disabledColor = normalColor;
            scrollbar[selectBarNum].colors = colorBlock;

            scrollbarSelectImage.SetActive(false);
            //scrollbarImage.color = normalColor;
            selectBarNum --;
            if(selectBarNum < 0) selectBarNum = scrollbar.Count - 1;
        }

        if(gameManager.playerInputAction.UI.CursorMoveDown.triggered)
        {
            colorBlock.disabledColor = normalColor;
            scrollbar[selectBarNum].colors = colorBlock;
            scrollbarSelectImage.SetActive(false);
            //scrollbarImage.color = normalColor;
            selectBarNum ++;
            if(selectBarNum >= scrollbar.Count) selectBarNum = 0;
        }
    }
}
