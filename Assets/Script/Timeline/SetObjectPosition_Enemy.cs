using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectPosition_Enemy : MonoBehaviour
{
    private bool inPlayer;
    public GameObject startPoint;
    public GameObject enemy;

    void Update()
    {
        if (inPlayer)
        {
            //スタート位置をプレイヤーの位置に合わせる。
            startPoint.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
