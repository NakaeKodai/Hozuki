using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public GameManager gameManager;
    public bool canMove = true;

    NavMeshAgent2D agent; //NavMeshAgent2Dを使用するための変数
    [SerializeField] Transform target; //追跡するターゲット
    [SerializeField] Transform spareTarget; //追跡するターゲット

    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
    }

    // Update is called once per frame
    void Update()
    {
        //追跡モードなら
        if(gameManager.isChaseTime)
        {
            //プレイヤーが動けるかつ、自分も動けるなら
            if(!gameManager.isOpenMenu && !gameManager.isOtherMenu && canMove)
            {
                //もし主人公が瞬間移動したら、瞬間移動した手前の位置を追いかける
                //それ以外はプレイヤーを追いかける
                if(!playerController.isTeleport)
                {
                    agent.destination = target.position;
                }
                else
                {
                    agent.destination = spareTarget.position;
                }
            }
        }
    }

    //当たり判定に当たったか(Collision)
    private void OnTriggerEnter2D(Collider2D other)
    {
        //瞬間移動したら、プレイヤーが瞬間移動した判定をfalseにする
        if (other.gameObject.CompareTag("Teleport"))
        {
            Debug.Log("見つけた");
            playerController.isTeleport = false;
        }
    }
}
