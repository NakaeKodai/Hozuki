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
    private Vector2 lastPlayerPosition;//前フレームのキャラの位置（穴用）
    private bool touchHole = false;

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
        lastPlayerPosition = rb.position;

        ItemManager.instance.ReSetItemStatus(); //デモ版のみ
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

        //穴に触れていない時に現在の位置を保存（穴に落ちない用）
        if(!touchHole)
        {
            lastPlayerPosition = rb.position;
        }

        animator.SetBool("IsMove", isMoving);
    }

    void FixedUpdate()
    {
        // Rigidbody2D に速度を適用
        if (!GameManager.instance.isOpenMenu && !GameManager.instance.isOtherMenu 
        && !GameManager.instance.isIvent&& canMove && !touchHole)
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
        else if(other.gameObject.CompareTag("HoleObject"))
        {
            Debug.Log("穴に当たった");
            Hole holeObject = other.GetComponent<Hole>();
            if(!holeObject.setRock){
                touchHole = true;
                rb.position = lastPlayerPosition;
                rb.velocity = Vector2.zero;
            }
            
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
        else if(other.gameObject.CompareTag("HoleObject"))
        {
            Debug.Log("穴にから離れた");
            touchHole = false;
        }
    }
}

