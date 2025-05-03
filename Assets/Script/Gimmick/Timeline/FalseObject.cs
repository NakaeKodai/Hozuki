using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseObject : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject o;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // if(gameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
            // {
            //     o.SetActive(false);
            // }
        }
    }
}
