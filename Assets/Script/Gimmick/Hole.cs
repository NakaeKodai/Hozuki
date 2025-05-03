using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool setRock = false;//岩が入ったか
    public Sprite image;
    public Sprite rockUnder;
    private float desiredWorldWidth = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void a()
    {
        Debug.Log("岩のオブジェクト判定できた");
    }

    public void holeInRock(){
        setRock = true;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Vector2 oldSize = spriteRenderer.bounds.size;
        spriteRenderer.sprite = rockUnder;
        // Vector2 newSize = spriteRenderer.bounds.size;
        //  Vector3 currentScale = transform.localScale;
        //     float scaleX = currentScale.x * (oldSize.x / newSize.x);
        //     float scaleY = currentScale.y * (oldSize.y / newSize.y);

        //     transform.localScale = new Vector3(scaleX, scaleY, currentScale.z);
        // スプライトのピクセルサイズ（幅・高さ）
        // float spritePixelWidth = rockUnder.rect.width;
        // float pixelsPerUnit = rockUnder.pixelsPerUnit;

        // // スプライトのワールドサイズ（ユニット換算）
        // float spriteWorldWidth = spritePixelWidth / pixelsPerUnit;

        // // スケール調整比率
        // float scaleRatio = desiredWorldWidth / spriteWorldWidth;

        // // スケールを設定（縦横等倍に保つ）
        // transform.localScale = new Vector3(scaleRatio, scaleRatio, 1);
    }

    public void holeReset()
    {
        setRock = false;
        //以下、グラフィックリセットしろ
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Vector2 oldSize = spriteRenderer.bounds.size;
        spriteRenderer.sprite = image;
        // Vector2 newSize = spriteRenderer.bounds.size;
        //  Vector3 currentScale = transform.localScale;
        //     float scaleX = currentScale.x * (oldSize.x / newSize.x);
        //     float scaleY = currentScale.y * (oldSize.y / newSize.y);
        // float spritePixelWidth = image.rect.width;
        // float pixelsPerUnit = image.pixelsPerUnit;

        // // スプライトのワールドサイズ（ユニット換算）
        // float spriteWorldWidth = spritePixelWidth / pixelsPerUnit;

        // // スケール調整比率
        // float scaleRatio = desiredWorldWidth / spriteWorldWidth;

        // // スケールを設定（縦横等倍に保つ）
        // transform.localScale = new Vector3(scaleRatio, scaleRatio, 1);
    }
}
