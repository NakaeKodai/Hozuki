using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTopic : MonoBehaviour
{
    [Header("会話内容　※一行で収まるように※")]
    [SerializeField]
    public List<Topic> topicList = new List<Topic>(); //stringのListのList

    [System.Serializable]
    public class Topic
    {
        public List<string> topic = new List<string>(); //stringのList
    }
}
