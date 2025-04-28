using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour
{
    public GameObject blackOut;
    public Image blackOutImage;
    public void BackTitleScene()
    {
        blackOut.SetActive(true);
        Color color = blackOutImage.color; // 現在の色を取得
        color.a = Mathf.Clamp01(1);  // アルファ値を設定 (0〜1に制限)
        blackOutImage.color = color;       // 設定し直す
        SceneManager.LoadScene("Title");
    }
}
