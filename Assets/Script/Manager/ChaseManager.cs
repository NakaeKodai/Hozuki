using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    // public static ChaseManager instance;

    [SerializeField] private PlayerController playerController;

    public GameObject ghost;

    // private void Awake()
    // {
    //     if (instance == null) instance = this;
    //     else Destroy(gameObject);

    //     DontDestroyOnLoad(gameObject); // シーンが変わっても消えないようにする
    // }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTarget()
    {
        playerController.isTeleport = false;
    }

    public void MoveLocation(float x, float y)
    {
        ghost.transform.position = new Vector3(x,y,0);
    }
}
