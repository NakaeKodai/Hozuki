using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public GameManager gameManager;

    private Rigidbody2D rb;
    public float speed = 100f; //動く速さ
    public float distance = 1f;//移動する距離
    private float nowDistance = 0f;//現在移動した距離
    Vector2 moveDirection; //移動の向き
    private bool canPush;//押し出し可能かを判定する
    private bool nowMove;//移動中かを判定する
    private Vector2 playerDirection;//プレイヤーの向き

    [SerializeField] LayerMask limitLayer; //壁判定
    private bool holeIn = false;//穴に入ったか
    private Hole holeScript;//触れた穴のスクリプト

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //ここから作る
        if(canPush && !nowMove)
        {
            if(gameManager.playerInputAction.Player.ActionANDDecision.IsPressed())
            {
                Debug.Log("Space押した");
                nowMove = true;
                // playerDirection = gameManager.playerInputAction.Player.Move.ReadValue<Vector2>();
                // CheckCollisions(); 
                // Debug.Log(playerDirection.x+","+playerDirection.y);
                // Debug.Log("プレイヤーが触れた方向: " + playerDirection);
                // moveDirection = -playerDirection;
                moveDirection.x = System.Math.Sign(playerDirection.x);
                moveDirection.y = System.Math.Sign(playerDirection.y);
                Debug.Log("動く方向: " + moveDirection);
            }
        }

        if(nowMove)
        {
            //この先のために移動のVector3を作る
            Vector3 deltaMove = new Vector3(moveDirection.x*speed*Time.deltaTime, moveDirection.y*speed*Time.deltaTime, 0);
            transform.Translate(deltaMove);//移動
            nowDistance += deltaMove.magnitude;//実際の移動距離を入力
            // Debug.Log("nowDistance:"+nowDistance);
            if(nowDistance >= distance)//設定した距離を移動
            {
                Debug.Log("停止");
                nowMove = false;
                nowDistance = 0f;
                canPush = false;

                //穴に入った時落とす処理
                if(holeIn)
                {
                    holeScript.setRock = true;
                    Debug.Log("消えるぜ");
                    Destroy(gameObject);
                }
            }
        }
        
        // // Rigidbody2D に速度を適用
        // if (canMove)
        // {
        //     if(gameManager.playerInputAction.Player.ActionANDDecision.IsPressed())
        //     {
        //         //ボタン長押しで移動できるよ
        //         moveDirection = gameManager.playerInputAction.Player.Move.ReadValue<Vector2>();
        //         if (moveDirection.magnitude < 0.5f) moveDirection = Vector2.zero;
        //         CheckCollisions(); //無くていいかも
                
        //         rb.velocity = moveDirection * speed; //ここが移動のコード
        //     }
        // }else
        // {
        //     rb.velocity = Vector2.zero; // 停止
        // }
    }

    void CheckCollisions()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.3f, limitLayer) && moveDirection.y > 0)
            playerDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1f, limitLayer) && moveDirection.y < 0)
            playerDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.5f, limitLayer) && moveDirection.x < 0)
            playerDirection.x = 0;
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.5f, limitLayer) && moveDirection.x > 0)
            playerDirection.x = 0;
    }

    //当たり判定に当たったか(Collision)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("押せるで");
            canPush = true;
            // CheckCollisions();
            // Vector2 hitDirection = (transform.position - other.transform.position).normalized;
            Vector2 direction = (transform.position - other.transform.position).normalized;

            Debug.Log("プレイヤーが触れた方向修正前: " + direction);

            // direction.x 
            
            // if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            if(Mathf.Abs(direction.y) < 0.9 && direction.y < 0)
            {
                playerDirection = new Vector2(Mathf.Sign(direction.x), 0);  // 横方向
            }  
            else{
                playerDirection = new Vector2(0, Mathf.Sign(direction.y));  // 縦方向
            }
                
            Debug.Log("プレイヤーが触れた方向: " + playerDirection);
        }
        //穴に入ったか判定
        else if(other.gameObject.CompareTag("HoleObject"))
        {
            Debug.Log("穴判定した");
            holeIn = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("HoleObject"))
        {
            Debug.Log("穴判定した(Enter)");
            holeIn = true;
            holeScript = other.GetComponent<Hole>();
            // holeScript.a();
        }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if(other.gameObject.CompareTag("HoleObject"))
    //     {
    //         Debug.Log("穴判定した(Stay)");
    //         holeIn = true;
    //     }
    // }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("HoleObject"))
        {
            Debug.Log("穴判定した(Stay)");
            holeIn = true;
            // holeScript = other.GetComponent<Hole>();
            // holeScript.a();
        }
    }

    //当たり判定から出たか(Collision)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("押せぬ");
            canPush = false;
        }
    }

}
