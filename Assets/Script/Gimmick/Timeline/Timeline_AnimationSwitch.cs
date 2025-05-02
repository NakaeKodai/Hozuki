using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline_AnimationSwitch : MonoBehaviour
{
    public GameObject animationSwitcher;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animationSwitcher.SetActive(true);
            Destroy(gameObject);
        }
    }
}
