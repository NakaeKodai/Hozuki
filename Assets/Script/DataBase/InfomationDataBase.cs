using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InfomationDataBase : ScriptableObject
{
    public List<InfomationData> infomationList = new List<InfomationData>();
}

[System.Serializable]
public class InfomationData
{
    public int infoID;
    public string infoName;
    public Sprite infoImage;

    public enum Status
    {
        NOTHAVE, //未所持
        HAVE, //所持
        WASHAVE //過去所持
    }

    public Status haveStatus;

    [Header("アイテムの説明を書いてね")]
    public string infoInfo;
}
