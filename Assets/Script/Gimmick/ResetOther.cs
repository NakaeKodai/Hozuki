using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOther : MonoBehaviour
{
    [Header("後半表示したいオブジェクト")]
    public List<GameObject> otherObjects = new List<GameObject>();//setActiveで起動するものすべて
    [Header("後半表示したくないオブジェクト")]
    public List<GameObject> hideObjects = new List<GameObject>();//非表示するものすべて


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //オブジェクトの表示、非表示をする（true→起動、false→非表示）
    public void SetActiveObjects(bool set)
    {
        for(int i = 0; i < otherObjects.Count; i++)
        {
            if(otherObjects[i] != null)
            {
                if(set)
            {
                otherObjects[i].SetActive(true);
            }
            else
            {
                otherObjects[i].SetActive(false);
            }
            }
            
        }
        for(int i = 0; i < hideObjects.Count; i++)
        {
            if(hideObjects[i] != null)
            {
                if(set)
            {
                hideObjects[i].SetActive(false);
            }
            else
            {
                hideObjects[i].SetActive(true);
            }
            }
            
        }
    }
}
