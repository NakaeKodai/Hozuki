using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveObject : MonoBehaviour
{
    public List<GameObject> switchObject = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for(int i = 0;i < switchObject.Count;i++)
            {
                switchObject[i].SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
