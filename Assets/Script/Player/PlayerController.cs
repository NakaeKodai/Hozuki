using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //プレイヤーの操作全般を制御するスクリプト

    //Manager
    public GameManager gameManager;
    public TalkManager talkManager;
    public ItemManager itemManager;
    public TakeManager takeManager;

    //プレイヤーの変数
    private Rigidbody2D rb;
    public float player_x, player_y; //プレイヤーの位置
    public float speed = 5f; //歩く速さ
    bool isMoving; //動いているかどうか アニメーションで使う
    public bool canMove; //移動可能かどうか
    public bool isTeleport; //瞬間移動したかどうか
    public bool canCarry; //運搬可能なオブジェクトに触れているか
    public bool canPush;//押し出し可能なオブジェクトにふれているか
    //bool moveFloor; //確か移動可能なオブジェクトがあった時に使ってた気がする
    Vector2 playerDirection; //プレイヤーの向き
    Vector2 iventDirection; //イベントを調べる向き
    Animator animator; //アニメーション変数

    //敵関連の変数
    public GameObject trackPoint;

    public bool freeControlType; //falseでマス移動、trueで自由移動

    //タイマー変数
    public float timer = 0.0f;
    public float freezeTime = 0.5f;
    public bool wasNonZeroLastFrame; //0じゃなくなったか;

    //UI関連
    // [SerializeField] private GameObject textWindow;
    // [SerializeField] private GameObject takeItemwindow;
    private TalkTopic talkTopic;
    // private ItemInfo itemInfo;
    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    [SerializeField] LayerMask limitLayer; //壁判定
    [SerializeField] LayerMask talkLayer; //会話可能なオブジェクトの判定
    [SerializeField] LayerMask itemLayer; //入手可能アイテムの判定


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameManager.instance.isOpenMenu && !GameManager.instance.isOtherMenu  
        && !GameManager.instance.isIvent && canMove)
        {
            // プレイヤーの向きを取得
            playerDirection = GameManager.instance.playerInputAction.Player.Move.ReadValue<Vector2>();

            // 小さな入力を無視
            if (playerDirection.magnitude < 0.5f) 
            {
                playerDirection = Vector2.zero;
                isMoving = false;
            }
            else 
            {
                iventDirection = playerDirection;
                isMoving = true;
            }

            // 壁判定を行い、移動方向を調整
            //CheckCollisions();

            // isMoving の更新
            // isMoving = playerDirection != Vector2.zero;

            // アニメーションの更新
            MoveAnimator(playerDirection.x, playerDirection.y);

            // TalkSearch();
            // ItemSearch();
            CarryOperation();
        }
        else if (!canMove)
        {
            if (timer <= freezeTime)
            {
                isMoving = false;
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0.0f;
                canMove = true;
            }
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("IsMove", isMoving);
    }

    void FixedUpdate()
    {
        // Rigidbody2D に速度を適用
        if (!GameManager.instance.isOpenMenu && !GameManager.instance.isOtherMenu 
        && !GameManager.instance.isIvent&& canMove)
        {
            rb.velocity = playerDirection * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // 停止
        }
    }

    void CheckCollisions()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.3f, limitLayer) && playerDirection.y > 0)
            playerDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1f, limitLayer) && playerDirection.y < 0)
            playerDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.5f, limitLayer) && playerDirection.x < 0)
            playerDirection.x = 0;
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.5f, limitLayer) && playerDirection.x > 0)
            playerDirection.x = 0;
    }

    //アニメーションの制御
    public void MoveAnimator(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            animator.SetFloat("InputX", x);
            animator.SetFloat("InputY", y);
        }
    }

    //しゃべることができるかどうか探す
    // void TalkSearch()
    // {
    //     //iventDirectionの向きでRayを飛ばし、talkLayerを検知
    //     RaycastHit2D hitTalk = Physics2D.Raycast(transform.position, iventDirection, 1.0f, talkLayer);

    //     //Rayがhitしたら
    //     if(hitTalk.collider != null)
    //     {
    //         //Debug.Log("お話可能");

    //         //talkTopicがnullなら、Rayで入手したTalkTopicを入手
    //         //無駄にtalkTopicに代入しないように制御している
    //         if(talkTopic == null)
    //         {
    //             talkTopic = hitTalk.collider.gameObject.GetComponent<TalkTopic>();
    //         }

    //         //操作説明
    //         if(GameManager.controllerType == GameManager.ControllerType.Unknown)
    //         {
    //             operationText.text = "Space : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
    //         {
    //             operationText.text = "● : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
    //         {
    //             operationText.text = "A : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
    //         {
    //             operationText.text = "B : 調べる";
    //         }

    //         operation.SetActive(true);

    //         //talkTopicがnullじゃないかつ、ボタンが押されたら調べる
    //         if(gameManager.playerInputAction.Player.ActionANDDecision.triggered && talkTopic != null)
    //         {
    //             //動けなくして、UIを出現させ、話す

    //             //Debug.Log("おなはしー");
    //             isMoving = false;
    //             animator.SetBool("IsMove", isMoving);
    //             //textWindow.SetActive(true);
    //             talkManager.Talk(talkTopic.topicList[0].topic, true);
    //             operation.SetActive(false);
    //         }
    //     }
    //     else
    //     {
    //         //Rayが外れているかつ、TalkTopicに何かが入っていたらTalkTopicをnullにする
    //         if(talkTopic != null) talkTopic = null;
    //         operation.SetActive(false);
    //     }
    // }

    //アイテムを探す
    // void ItemSearch()
    // {
    //     //iventDirectionの向きでRayを飛ばし、itemLayerを検知
    //     // RaycastHit2D hitItem = Physics2D.Raycast(transform.position, Vector2.zero, 1.0f, itemLayer);
    //     Vector2 center = GetComponent<Collider2D>().bounds.center;
    //     Collider2D hitItem = Physics2D.OverlapCircle(center, 0.8f, itemLayer);
    //     if(hitItem!= null)
    //     {
    //         //操作説明
    //         if(GameManager.controllerType == GameManager.ControllerType.Unknown)
    //         {
    //             operationText.text = "Space : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
    //         {
    //             operationText.text = "● : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
    //         {
    //             operationText.text = "A : 調べる";
    //         }
    //         else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
    //         {
    //             operationText.text = "B : 調べる";
    //         }
    //         operation.SetActive(true);

    //         //itemInfoがnullなら、Rayで入手したitemInfoを入手
    //         ItemInfo itemInfo = hitItem.gameObject.GetComponent<ItemInfo>();
    //         //talkTopicがnullじゃないかつ、ボタンが押されたら調べる
    //         if(gameManager.playerInputAction.Player.ActionANDDecision.triggered && itemInfo != null)
    //         {
    //             //itemIDからアイテムを探し、itemdataにpicUPitemに代入
    //             //UIを出して入手したことを表示させる
    //             //その後に入手したアイテムをデストロイする

    //             ItemData pickUPitem = itemManager.PickUp(itemInfo.itemID);
    //             Debug.Log(pickUPitem.itemID);
    //             //takeItemwindow.SetActive(true);
    //             takeManager.TakeItem(pickUPitem);
    //             itemInfo.DestroyObject();
    //         }
    //     }
    //     else
    //     {
    //         // operation.SetActive(false);
    //     }
    // }

    // void PasswordLockSerch()
    // {
        
    // }

    void CarryOperation()
    {
        if (canCarry)
        {
            //操作説明
            if(GameManager.controllerType == GameManager.ControllerType.Unknown)
            {
                operationText.text = "Space : 運ぶ";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
            {
                operationText.text = "● : 運ぶ";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
            {
                operationText.text = "A : 運ぶ";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
            {
                operationText.text = "B : 運ぶ";
            }

            operation.SetActive(true);
        }
        else 
        {
            //operation.SetActive(false);
        }
    }

    //当たり判定に当たったか(Collision)
    private void OnCollisionEnter2D(Collision2D other)
    {
        //瞬間移動の処理
        if (other.gameObject.CompareTag("Teleport"))
        {
            canMove = false;
            trackPoint.transform.position = new Vector2(other.transform.position.x,other.transform.position.y);
            isTeleport = true;
        }
    }

    //当たり判定に当たったか(trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CarryObject"))
        {
            Debug.Log("運搬可能");
            canCarry = true;
        }else if(other.gameObject.CompareTag("PushObject"))
        {
            Debug.Log("押し出し可能");
            canPush = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CarryObject"))
        {
            Debug.Log("運搬不可");
             canCarry = false;
        }else if(other.gameObject.CompareTag("PushObject"))
        {
            Debug.Log("押し出し不可能");
            canPush = false;
        }
    }
}

