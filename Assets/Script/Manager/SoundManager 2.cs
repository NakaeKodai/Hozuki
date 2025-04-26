using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource bgmSource; //BGMのオーディオソース
    [SerializeField] private AudioSource seSource; //SEのオーディオソース

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> bgmList; //BGMのリスト
    public List<Sound> seList; //SEのリスト

    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> seDict;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // シーンが変わっても消えないようにする

        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var sound in bgmList) bgmDict[sound.name] = sound.clip;

        seDict = new Dictionary<string, AudioClip>();
        foreach (var sound in seList) seDict[sound.name] = sound.clip;
    }
    
    public void PlayBGM(string name)
    {
        if (bgmDict.TryGetValue(name, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.Log($"BGM '{name}' not found!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySE(string name)
    {
        if (seDict.TryGetValue(name, out AudioClip clip))
        {
            seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError($"SE '{name}' not found!");
        }
    }
}
