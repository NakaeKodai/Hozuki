using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    public GameManager gameManager;

    private Rigidbody2D rb;
    public float speed = 5f; //動く速さ
    Vector2 moveDirection; //プレイヤーの向き
    private bool canMove;

    [SerializeField] LayerMask limitLayer; //壁判定
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
    }

    // Update is called once per frame
    void Update()
    {
        // if(canMove)
        // {
        //     moveDirection = gameManager.playerInputAction.Player.Move.ReadValue<Vector2>();
        //     if (playerDirection.magnitude < 0.5f) playerDirection = Vector2.zero;

        // }
    }

    void FixedUpdate()
    {
        // Rigidbody2D に速度を適用
        if (canMove)
        {
            if(gameManager.playerInputAction.Player.ActionANDDecision.IsPressed())
            {
                //ボタン長押しで移動できるよ
                moveDirection = gameManager.playerInputAction.Player.Move.ReadValue<Vector2>();
                if (moveDirection.magnitude < 0.5f) moveDirection = Vector2.zero;
                CheckCollisions(); //無くていいかも
                
                rb.velocity = moveDirection * speed; //ここが移動のコード
            }
        }else
        {
            rb.velocity = Vector2.zero; // 停止
        }
    }

    void CheckCollisions()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.3f, limitLayer) && moveDirection.y > 0)
            moveDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1f, limitLayer) && moveDirection.y < 0)
            moveDirection.y = 0;
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.5f, limitLayer) && moveDirection.x < 0)
            moveDirection.x = 0;
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.5f, limitLayer) && moveDirection.x > 0)
            moveDirection.x = 0;
    }

    //当たり判定に当たったか(Collision)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("動けるで");
            canMove = true;
        }
    }

    //当たり判定から出たか(Collision)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("動けぬ");
            canMove = false;
        }
    }
}
