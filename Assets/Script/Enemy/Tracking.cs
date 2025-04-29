using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public bool canMove = true;

    NavMeshAgent2D agent; // NavMeshAgent2Dを使用するための変数
    [SerializeField] Transform target; // 追跡するターゲット
    [SerializeField] Transform spareTarget; // 瞬間移動後の追跡対象

    Animator animator; // アニメーション制御用のAnimator

    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>(); // agentにNavMeshAgent2Dを取得
        animator = GetComponent<Animator>(); // Animatorを取得
    }

    // Update is called once per frame
    void Update()
    {
        // 追跡モードなら
        if (GameManager.instance.isChaseTime)
        {
            // プレイヤーが動けるかつ、自分も動けるなら
            if (!GameManager.instance.isOpenMenu && !GameManager.instance.isOtherMenu && canMove)
            {
                // 瞬間移動していなければ通常の目的地を設定
                if (!playerController.isTeleport)
                {
                    agent.destination = target.position;
                }
                else // 瞬間移動後は予備の目的地を設定
                {
                    agent.destination = spareTarget.position;
                }

                // 移動方向に応じたアニメーション制御
                Vector2 moveDirection = agent.CurrentDirection;
                float xDir = moveDirection.x;
                float yDir = moveDirection.y;

                // アニメーターに移動方向を渡す（例として、"SpeedX"と"SpeedY"を設定）
                animator.SetFloat("InputX", -xDir);
                animator.SetFloat("InputY", yDir);
                animator.SetBool("IsMove", canMove);
            }
        }
    }

    // 当たり判定に当たったか(Collision)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.isChaseTime = false;
            SoundManager.instance.StopBGM();
            GameManager.instance.ResetCurrentScene();
        }

        // 瞬間移動したら、プレイヤーが瞬間移動した判定をfalseにする
        if (other.gameObject.CompareTag("Teleport"))
        {
            Debug.Log("見つけた");
            playerController.isTeleport = false;
        }
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Tracking : MonoBehaviour
// {
//     [SerializeField] private PlayerController playerController;
//     public GameManager.instance GameManager.instance;
//     public bool canMove = true;

//     NavMeshAgent2D agent; //NavMeshAgent2Dを使用するための変数
//     [SerializeField] Transform target; //追跡するターゲット
//     [SerializeField] Transform spareTarget; //追跡するターゲット

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent2D>(); //agentにNavMeshAgent2Dを取得
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //追跡モードなら
//         if(GameManager.instance.isChaseTime)
//         {
//             //プレイヤーが動けるかつ、自分も動けるなら
//             if(!GameManager.instance.isOpenMenu && !GameManager.instance.isOtherMenu && canMove)
//             {
//                 //もし主人公が瞬間移動したら、瞬間移動した手前の位置を追いかける
//                 //それ以外はプレイヤーを追いかける
//                 if(!playerController.isTeleport)
//                 {
//                     agent.destination = target.position;
//                 }
//                 else
//                 {
//                     agent.destination = spareTarget.position;
//                 }
//             }
//         }
//     }

//     //当たり判定に当たったか(Collision)
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         //瞬間移動したら、プレイヤーが瞬間移動した判定をfalseにする
//         if (other.gameObject.CompareTag("Teleport"))
//         {
//             Debug.Log("見つけた");
//             playerController.isTeleport = false;
//         }
//     }
// }
