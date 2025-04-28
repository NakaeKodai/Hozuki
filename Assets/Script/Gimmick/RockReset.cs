using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockReset : MonoBehaviour
{
    public bool rockFinished = false;//岩運びが終了したか
    [System.Serializable]
    public class rocks//岩の保存
    {
        public GameObject rock;//岩本体
        public Vector2 firstPosition;//岩の初期位置
    }
    public List<rocks> rockList = new List<rocks>();//岩を保存するリスト
    public List<GameObject> holeList = new List<GameObject>();//穴を保存するリスト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //岩をリセットする
    public void resetRocks()
    {
        //岩の初期位置リセット
        for(int i = 0; i < rockList.Count; i++)
        {
            rockList[i].rock.transform.position = rockList[i].firstPosition;
            rockList[i].rock.SetActive(true);
        }
        //穴の状態リセット
        for(int i = 0; i < holeList.Count; i++)
        {
            Hole holeScript = holeList[i].GetComponent<Hole>();
            holeScript.holeReset();
        }
    }

    //初めて入った時に岩の座標を保存する
    public void setRocksPosition()
    {
        for(int i = 0; i < rockList.Count; i++)
        {
            rockList[i].firstPosition = rockList[i].rock.transform.position;
            // Debug.Log(rockList[i].rock.name+"→x:"+rockList[i].firstPosition.x+"y:"+rockList[i].firstPosition.y);
        }
    }
}
