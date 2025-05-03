using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseLeft : MonoBehaviour
{
    public float speed = 2.5f;
    public bool canMove = false;
    Animator animator; //アニメーション変数
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            MoveAnimator(1,0);
            transform.position -= Vector3.right*speed*Time.deltaTime;
        }
        
    }

    public void MoveAnimator(float x, float y)
    {
        if (x != 0 || y != 0)
        {
            animator.SetFloat("InputX", x);
            animator.SetFloat("InputY", y);
        }
    }
}
