using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ControlTimeline : MonoBehaviour
{
    public PlayableDirector director;
    private bool waitingForInput = false;

    public Animator targetAnimator;
    public RuntimeAnimatorController defaultController;

    void Update()
    {
        if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered && waitingForInput)
        {
            director.Resume();

            if(targetAnimator != null && defaultController != null)
            {
                targetAnimator.runtimeAnimatorController = defaultController;
            }
            waitingForInput = false;
        }
    }

    public void puase()
    {
        Debug.Log("ポーズ");

        if(targetAnimator != null)
        {
            targetAnimator.runtimeAnimatorController = null;
        }
        director.Pause();
        director.Evaluate(); 
        waitingForInput = true;
    }
}
