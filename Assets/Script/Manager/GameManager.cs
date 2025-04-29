using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData //セーブ関係の変数
{
    public Vector3 position; //プレイヤーの位置
    public float[] scrolbarValue; //スクロールバーの変数  現在　0はLight 1はAudio
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerInputAction playerInputAction; //inputSystemの変数

    public enum ControllerType
    {
        Unknown, //接続無し、またはそれ以外
        PlayStation,
        Nintendo,
        Xbox,
        Logitech,
        Generic
    }

    public static ControllerType controllerType = ControllerType.Unknown;

    string[] controllerName;

    //セーブ関連
    [SerializeField] Transform player; //プレイヤーの位置
    [SerializeField] SettingManager settingManager; //設定関連
    [SerializeField] Image lightImage; //明るさ調整のimage

    public bool isOpenMenu; //メニューを開いているかどうか
    public bool isOtherMenu; //メニュー以外のUIをほらいているかどうか
    public bool isChaseTime; //敵に追われているか

    public PlayableDirector director;
    public bool isIvent;

    public int playerTalkState; //なんだっけ？ 多分今何行目の会話かの制御？
    
    
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // シーンが変わっても消えないようにする
    }

    void Start()
    {
        playerTalkState = 0; //会話内容の初期化(デモ版仕様)
        //Load();
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckController();
        CheckIvent();


        //セーブ、ロード等のテスト　後で消す
        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     Save();
        // }
        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     Load();
        // }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            if(isChaseTime) isChaseTime = false;
            else isChaseTime = true;
        }
    }

    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    //セーブ機能
    public void Save()
    {
        //セーブに必要な情報を代入
        SaveData data = new SaveData()
        {
            position = player.position, //プレイヤーの位置
            scrolbarValue = new float[settingManager.scrollbar.Count] //設定スクロールバー
        };

        for(int i = 0;i < data.scrolbarValue.Length; i++)
        {
            //スクロールバーの価
            data.scrolbarValue[i] = settingManager.scrollbar[i].value;
        }

        //情報をjson化
        string json = JsonUtility.ToJson(data,true);
        Debug.Log(json);

        //Fileとして保存
        File.WriteAllText(GetFilePath(),json);

        // PlayerPrefs.SetString("SaveData", json);
        // PlayerPrefs.Save();
    }

    //ロード機能
    public void Load()
    {
        //Fileを入手
        string path = GetFilePath();
        if(File.Exists(path))
        {
            //jsonでFileの情報を入手し、SaveDataとして復元
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            //プレイヤーの位置を戻す
            player.position = data.position;

            //スクロールバー関連を戻す
            for(int i = 0;i < data.scrolbarValue.Length; i++)
            {
                settingManager.scrollbar[i].value = data.scrolbarValue[i];
                switch(i)
                {
                    case 0:
                        Color color = lightImage.color;
                        color.a = 1.0f - settingManager.scrollbar[i].value;
                        if(color.a >= 0.96f) color.a = 0.95f;
                        lightImage.color = color;
                    break;
                }
            }
        }
    }

    private void CheckController()
    {
        controllerName = Input.GetJoystickNames();

        // Debug.Log(controllerName[0]);

        if (controllerName.Length == 0 || string.IsNullOrEmpty(controllerName[0]))
        {
            //Debug.Log("No controller detected.");
            controllerType = ControllerType.Unknown;
            return;
        }

        string controller = controllerName[0].ToLower(); //判定をしやすくする

        if(controller.Contains("wireless controller") || controller.Contains("dualshock") || controller.Contains("dual sense"))
        {
            controllerType = ControllerType.PlayStation;
        }
        else if (controller.Contains("nintendo") || controller.Contains("switch"))
        {
            controllerType = ControllerType.Nintendo;
        }
        else if (controller.Contains("xbox"))
        {
            controllerType = ControllerType.Xbox;
        }
        else if (controller.Contains("logitech"))
        {
            controllerType = ControllerType.Logitech;
        }
        else
        {
            controllerType = ControllerType.Generic;
        }
    }

    private void CheckIvent()
    {
        if(director != null)
        {
            if(director.state != PlayState.Playing && director.time == 0)
            {
                isIvent = false;
            }
            else isIvent = true;
        }
    }

    public void ResetCurrentScene()
    {
        // 現在のシーンを取得
        Scene currentScene = SceneManager.GetActiveScene();

        // 同じシーンを再読み込み
        SceneManager.LoadScene(currentScene.name);
    }

    // private void OnApplicationQuit()
    // {
    //     Save();
    // }
}
