using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTest : MonoBehaviour
{
    
    [SerializeField] Transform player;
    sbyte x,y;
    float pX,pY,eX,eY;
    public float speed  = 1f;
    bool isMoving;
    bool moveFloor;

    Vector2 enemyDirection;
    [SerializeField] LayerMask limitLayer;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving)
        {
            Vector2 playerPosion = player.position;
            Vector2 myPosion = transform.position;

            float distanceX = playerPosion.x - myPosion.x;
            float distanceY = playerPosion.y - myPosion.y;
            // pX = player.transform.position.x;
            // pY = player.transform.position.y;
            // eX = transform.position.x;
            // eY = transform.position.y;

            if(Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
            {
                if(player.position.x > transform.position.x){x = 1;y = 0;}
                else{x = -1;y = 0;}
            }
            else{
                if(player.position.y > transform.position.y){x = 0;y = 1;}
                else{x = 0;y = -1;}
            }



            // if(player.transform.position.x > transform.position.x) 
            // {
            //     x = 1;y = 0;
            // }
            // else if(player.transform.position.x < transform.position.x) 
            // {
            //     x = -1;y = 0;
            // }
            // else if(player.transform.position.y > transform.position.y) 
            // {
            //     x = 0;y = 1;
            // }
            // else if(player.transform.position.y < transform.position.y) 
            // {
            //     x = 0;y = -1;
            // }

            StartCoroutine(Move(new Vector2(x,y)));
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPos = transform.position  + direction;
        if(isWalkable(targetPos) == false)
        {
            isMoving = false;
            yield break;
        }
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if(moveFloor == true)
            {
                targetPos = transform.position;
                transform.position = targetPos;
                isMoving = false;
                moveFloor = false;
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    bool isWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos,0.1f,limitLayer) == false;
    }
}
