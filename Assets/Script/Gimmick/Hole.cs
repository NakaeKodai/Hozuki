using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool setRock = false;//岩が入ったか
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

    public void holeReset()
    {
        setRock = false;
        //以下、グラフィックリセットしろ
        
    }
}
