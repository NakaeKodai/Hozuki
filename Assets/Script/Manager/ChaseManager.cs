using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    public static ChaseManager instance;

    public GameObject ghost;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // シーンが変わっても消えないようにする
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isChaseTime)
        {
            SoundManager.instance.PlayBGM("追われる");
            SoundManager.instance.PlayBGM("耳鳴り");
        }
        else
        {
            SoundManager.instance.StopBGM();
        }
        
    }

    public void MoveLocation(float x, float y)
    {
        ghost.transform.position = new Vector3(x,y,0);
    }
}
