using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanMove : MonoBehaviour
{
    public ChaseLeft chaseLeft;
    public bool set = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(set)
            {
                chaseLeft.canMove = true;
            }
            else
            {
                chaseLeft.canMove = false;
            }
        }
    }
}
