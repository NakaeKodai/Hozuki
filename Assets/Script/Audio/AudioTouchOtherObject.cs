using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTouchOtherObject : MonoBehaviour
{
    public GameObject TargetObject;//触れたオブジェクトがこれなら動く
    public string TargetAudio;//鳴らしたいAudio

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == TargetObject)
        {
            SoundManager.instance.PlaySE(TargetAudio);
            gameObject.SetActive(false);
        }
    }
}
