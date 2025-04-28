using UnityEngine;

public class SetObejctPosition : MonoBehaviour
{
    public GameObject startPoint;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            //スタート位置をプレイヤーの位置に合わせる。
            startPoint.transform.position = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, 0);
        }
    }
}
