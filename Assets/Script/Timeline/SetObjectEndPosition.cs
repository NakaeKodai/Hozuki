using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectEndPosition : MonoBehaviour
{
    public GameObject endPoint;
    [Header("無視する軸、false：x軸、true：y軸")]
    public bool ignoreShaft = false;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            if(!ignoreShaft)
            {
                //スタート位置をプレイヤーの位置に合わせる。
                endPoint.transform.position = new Vector3(endPoint.transform.position.x, player.gameObject.transform.position.y, 0);
            }
            else
            {
                endPoint.transform.position = new Vector3(player.gameObject.transform.position.x, endPoint.transform.position.y, 0);
            }
            
        }
    }
}
