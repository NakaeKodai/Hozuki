using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator myAnimator;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("あにめーしょん");
            myAnimator.SetTrigger("NewText");
        }
        // if (Input.GetKeyDown(KeyCode.F2))
        // {
        //     myAnimator.SetTrigger("Normal");
        // }
    }
}
