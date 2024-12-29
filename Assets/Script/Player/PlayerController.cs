using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //プレイヤーの操作全般を制御するスクリプト

    //Manager
    public GameManager gameManager;
    public TalkManager talkManager;
    public ItemManager itemManager;
    public TakeManager takeManager;

    //プレイヤーの変数
    public float player_x, player_y; //プレイヤーの位置
    public float speed = 5f; //歩く速さ
    bool isMoving; //動いているかどうか アニメーションで使う
    public bool canMove; //移動可能かどうか
    public bool isTeleport; //瞬間移動したかどうか
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
    private ItemInfo itemInfo;

    [SerializeField] LayerMask limitLayer; //壁判定
    [SerializeField] LayerMask talkLayer; //会話可能なオブジェクトの判定
    [SerializeField] LayerMask itemLayer; //入手可能アイテムの判定


    void Start()
    {
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //諸々のメニューが開いていなく、動ける状態の場合
        if (!gameManager.isOpenMenu && !gameManager.isOtherMenu && canMove)
        {
            //プレイヤーの向きをInputSystemから入手
            playerDirection = gameManager.playerInputAction.Player.Move.ReadValue<Vector2>();

            //もしInputSystemから得られた値が小さい場合、移動させない
            if(playerDirection.magnitude < 0.5f) playerDirection = Vector2.zero;

            //プレイヤーのxに向きのxを、プレイヤーのyに向きのyを代入
            player_x = playerDirection.x;
            player_y = playerDirection.y;

            //もし向きが0ではないなら、イベントの向きにプレイヤーの向きを代入
            if(playerDirection != Vector2.zero) iventDirection = playerDirection;

            //上に壁があるかつ、上に移動使用とするなら、上方向の移動を0にする
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.3f,limitLayer);
            if(hitUp.collider != null && player_y > 0)
            {
                //Debug.Log("上かべぇ");   
                player_y = 0;
            }

            //下に壁があるかつ、下に移動使用とするなら、下方向の移動を0にする
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1f,limitLayer);
            if(hitDown.collider != null && player_y < 0)
            {
                //Debug.Log("下かべぇ");   
                player_y = 0;
            }

            //左に壁があるかつ、左に移動使用とするなら、左方向の移動を0にする
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f,limitLayer);
            if(hitLeft.collider != null && player_x < 0)
            {
                //Debug.Log("左かべぇ");   
                player_x = 0;
            }

            //右に壁があるかつ、右に移動使用とするなら、右方向の移動を0にする
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f,limitLayer);
            if(hitRight.collider != null && player_x > 0)
            {
                // Debug.Log("右かべぇ");   
                player_x = 0;
            }

            //移動していないならisMovingをfalseにする それ以外はtrue
            if(player_x == 0 && player_y == 0) isMoving = false;
            else isMoving = true;

            //player_x、player_yの向きに移動する
            transform.Translate(
            player_x * speed * Time.deltaTime,
            player_y * speed * Time.deltaTime,
            0.0f);

            //アニメーターにプレイヤーの向きを教える
            MoveAnimator(playerDirection.x, playerDirection.y);
            
            TalkSearch(); //しゃべることができるか確認
            ItemSearch(); //アイテムがあるか確認
        }
        else if(!canMove) //もし動けないなら
        {
            //主に瞬間移動したときの一瞬動けなくする処理
            if(timer <= freezeTime)
            {
                // Debug.Log("止まるぜ");
                isMoving = false;
                timer += Time.deltaTime;
            }
            else
            {
                // Debug.Log("動くぜ");
                timer = 0.0f;
                canMove = true;
            }
        }
        else
        {
            isMoving = false;
        }

        //動いているかどうかをアニメーターに教える
        animator.SetBool("IsMove", isMoving);
    }

    //アニメーションの制御
    public void MoveAnimator(float x, float y)
    {
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            animator.SetFloat("InputX", playerDirection.x);
            animator.SetFloat("InputY", playerDirection.y);
        }
    }

    //しゃべることができるかどうか探す
    void TalkSearch()
    {
        //iventDirectionの向きでRayを飛ばし、talkLayerを検知
        RaycastHit2D hitTalk = Physics2D.Raycast(transform.position, iventDirection, 1.0f, talkLayer);

        //Rayがhitしたら
        if(hitTalk.collider != null)
        {
            //Debug.Log("お話可能");

            //talkTopicがnullなら、Rayで入手したTalkTopicを入手
            //無駄にtalkTopicに代入しないように制御している
            if(talkTopic == null)
            {
                talkTopic = hitTalk.collider.gameObject.GetComponent<TalkTopic>();
            }

            //talkTopicがnullじゃないかつ、ボタンが押されたら調べる
            if(gameManager.playerInputAction.Player.ActionANDDecision.triggered && talkTopic != null)
            {
                //動けなくして、UIを出現させ、話す

                //Debug.Log("おなはしー");
                isMoving = false;
                animator.SetBool("IsMove", isMoving);
                //textWindow.SetActive(true);
                talkManager.Talk(talkTopic.topicList[0].topic, true);
            }
        }
        else
        {
            //Rayが外れているかつ、TalkTopicに何かが入っていたらTalkTopicをnullにする
            if(talkTopic != null) talkTopic = null;
        }
    }

    //アイテムを探す
    void ItemSearch()
    {
        //iventDirectionの向きでRayを飛ばし、itemLayerを検知
        RaycastHit2D hitItem = Physics2D.Raycast(transform.position, iventDirection, 1.0f, itemLayer);
        if(hitItem.collider != null)
        {

            //itemInfoがnullなら、Rayで入手したitemInfoを入手
            //無駄にitemInfoに代入しないように制御している
            if(itemInfo == null)
            {
                itemInfo = hitItem.collider.gameObject.GetComponent<ItemInfo>();
            }

            //talkTopicがnullじゃないかつ、ボタンが押されたら調べる
            if(gameManager.playerInputAction.Player.ActionANDDecision.triggered && itemInfo != null)
            {
                //itemIDからアイテムを探し、itemdataにpicUPitemに代入
                //UIを出して入手したことを表示させる
                //その後に入手したアイテムをデストロイする

                ItemData pickUPitem = itemManager.PickUp(itemInfo.itemID);
                Debug.Log(pickUPitem.itemID);
                //takeItemwindow.SetActive(true);
                takeManager.TakeItem(pickUPitem);
                itemInfo.DestroyObject();
            }
        }
    }

    //当たり判定に当たったか(Collision)
    private void OnCollisionEnter2D(Collision2D other)
    {
        //瞬間移動の処理
        if (other.gameObject.CompareTag("Teleport"))
        {
            trackPoint.transform.position = new Vector2(other.transform.position.x,other.transform.position.y);
            isTeleport = true;
            canMove = false;
        }
    }

    //当たり判定に当たったか(trigger) 一応取っといている
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Clear"))
        // {
        //     if(!main.Killed) SceneManager.LoadScene("End");
        //     else SceneManager.LoadScene("End2");
        // }

        // if (other.CompareTag("Enemy"))
        // {
        //     SceneManager.LoadScene("End3");
        // } 
    }
}

